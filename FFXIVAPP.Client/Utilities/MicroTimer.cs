// FFXIVAPP.Client
// MicroTimer.cs
// 
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions are met: 
// 
//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright 
//    notice, this list of conditions and the following disclaimer in the 
//    documentation and/or other materials provided with the distribution. 
//  * Neither the name of SyndicatedLife nor the names of its contributors may 
//    be used to endorse or promote products derived from this software 
//    without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE. 

using System.Threading;

namespace FFXIVAPP.Client.Utilities
{
    public class MicroTimer
    {
        public delegate void MicroTimerElapsedEventHandler(object sender, MicroTimerEventArgs timerEventArgs);

        private long _ignoreEventIfLateBy = long.MaxValue;
        private bool _stopTimer = true;
        private Thread _threadTimer;
        private long _timerIntervalInMicroSec;

        public MicroTimer()
        {
        }

        public MicroTimer(long timerIntervalInMicroseconds)
        {
            Interval = timerIntervalInMicroseconds;
        }

        public long Interval
        {
            get { return Interlocked.Read(ref _timerIntervalInMicroSec); }
            set { Interlocked.Exchange(ref _timerIntervalInMicroSec, value); }
        }

        public long IgnoreEventIfLateBy
        {
            get { return Interlocked.Read(ref _ignoreEventIfLateBy); }
            set { Interlocked.Exchange(ref _ignoreEventIfLateBy, value <= 0 ? long.MaxValue : value); }
        }

        public bool Enabled
        {
            set
            {
                if (value)
                {
                    Start();
                }
                else
                {
                    Stop();
                }
            }
            get { return (_threadTimer != null && _threadTimer.IsAlive); }
        }

        public event MicroTimerElapsedEventHandler MicroTimerElapsed;

        public void Start()
        {
            if (Enabled || Interval <= 0)
            {
                return;
            }

            _stopTimer = false;

            ThreadStart threadStart = () => NotificationTimer(ref _timerIntervalInMicroSec, ref _ignoreEventIfLateBy, ref _stopTimer);

            _threadTimer = new Thread(threadStart)
            {
                Priority = ThreadPriority.Highest
            };
            _threadTimer.Start();
        }

        public void Stop()
        {
            _stopTimer = true;
        }

        public void StopAndWait()
        {
            StopAndWait(Timeout.Infinite);
        }

        public bool StopAndWait(int timeoutInMilliSec)
        {
            _stopTimer = true;

            if (!Enabled || _threadTimer.ManagedThreadId == Thread.CurrentThread.ManagedThreadId)
            {
                return true;
            }

            return _threadTimer.Join(timeoutInMilliSec);
        }

        public void Abort()
        {
            _stopTimer = true;

            if (Enabled)
            {
                _threadTimer.Abort();
            }
        }

        private void NotificationTimer(ref long timerIntervalInMicroSec, ref long ignoreEventIfLateBy, ref bool stopTimer)
        {
            var timerCount = 0;
            long nextNotification = 0;

            var microStopwatch = new MicroStopwatch();
            microStopwatch.Start();

            while (!stopTimer)
            {
                var callbackFunctionExecutionTime = microStopwatch.ElapsedMicroseconds - nextNotification;

                var timerIntervalInMicroSecCurrent = Interlocked.Read(ref timerIntervalInMicroSec);
                var ignoreEventIfLateByCurrent = Interlocked.Read(ref ignoreEventIfLateBy);

                nextNotification += timerIntervalInMicroSecCurrent;
                timerCount++;
                long elapsedMicroseconds;

                while ((elapsedMicroseconds = microStopwatch.ElapsedMicroseconds) < nextNotification)
                {
                    Thread.SpinWait(10);
                }

                var timerLateBy = elapsedMicroseconds - nextNotification;

                if (timerLateBy >= ignoreEventIfLateByCurrent)
                {
                    continue;
                }

                var microTimerEventArgs = new MicroTimerEventArgs(timerCount, elapsedMicroseconds, timerLateBy, callbackFunctionExecutionTime);
                MicroTimerElapsed(this, microTimerEventArgs);
            }

            microStopwatch.Stop();
        }
    }
}
