// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MicroTimer.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2020 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   MicroTimer.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.Utilities {
    using System.Threading;

    internal class MicroTimer {
        private long _ignoreEventIfLateBy = long.MaxValue;

        private bool _stopTimer = true;

        private Thread _threadTimer;

        private long _timerIntervalInMicroSec;

        public MicroTimer() { }

        public MicroTimer(long timerIntervalInMicroseconds) {
            this.Interval = timerIntervalInMicroseconds;
        }

        public delegate void MicroTimerElapsedEventHandler(object sender, MicroTimerEventArgs timerEventArgs);

        public event MicroTimerElapsedEventHandler MicroTimerElapsed;

        public bool Enabled {
            get {
                return this._threadTimer != null && this._threadTimer.IsAlive;
            }

            set {
                if (value) {
                    this.Start();
                }
                else {
                    this.Stop();
                }
            }
        }

        public long IgnoreEventIfLateBy {
            get {
                return Interlocked.Read(ref this._ignoreEventIfLateBy);
            }

            set {
                Interlocked.Exchange(
                    ref this._ignoreEventIfLateBy, value <= 0
                                                       ? long.MaxValue
                                                       : value);
            }
        }

        public long Interval {
            get {
                return Interlocked.Read(ref this._timerIntervalInMicroSec);
            }

            set {
                Interlocked.Exchange(ref this._timerIntervalInMicroSec, value);
            }
        }

        public void Abort() {
            this._stopTimer = true;

            if (this.Enabled) {
                this._threadTimer.Abort();
            }
        }

        public void Start() {
            if (this.Enabled || this.Interval <= 0) {
                return;
            }

            this._stopTimer = false;

            ThreadStart threadStart = () => this.NotificationTimer(ref this._timerIntervalInMicroSec, ref this._ignoreEventIfLateBy, ref this._stopTimer);

            this._threadTimer = new Thread(threadStart) {
                Priority = ThreadPriority.Highest,
            };
            this._threadTimer.Start();
        }

        public void Stop() {
            this._stopTimer = true;
        }

        public void StopAndWait() {
            this.StopAndWait(Timeout.Infinite);
        }

        public bool StopAndWait(int timeoutInMilliSec) {
            this._stopTimer = true;

            if (!this.Enabled || this._threadTimer.ManagedThreadId == Thread.CurrentThread.ManagedThreadId) {
                return true;
            }

            return this._threadTimer.Join(timeoutInMilliSec);
        }

        private void NotificationTimer(ref long timerIntervalInMicroSec, ref long ignoreEventIfLateBy, ref bool stopTimer) {
            var timerCount = 0;
            long nextNotification = 0;

            var microStopwatch = new MicroStopwatch();
            microStopwatch.Start();

            while (!stopTimer) {
                var callbackFunctionExecutionTime = microStopwatch.ElapsedMicroseconds - nextNotification;

                var timerIntervalInMicroSecCurrent = Interlocked.Read(ref timerIntervalInMicroSec);
                var ignoreEventIfLateByCurrent = Interlocked.Read(ref ignoreEventIfLateBy);

                nextNotification += timerIntervalInMicroSecCurrent;
                timerCount++;
                long elapsedMicroseconds;

                while ((elapsedMicroseconds = microStopwatch.ElapsedMicroseconds) < nextNotification) {
                    Thread.SpinWait(10);
                }

                var timerLateBy = elapsedMicroseconds - nextNotification;

                if (timerLateBy >= ignoreEventIfLateByCurrent) {
                    continue;
                }

                var microTimerEventArgs = new MicroTimerEventArgs(timerCount, elapsedMicroseconds, timerLateBy, callbackFunctionExecutionTime);
                this.MicroTimerElapsed(this, microTimerEventArgs);
            }

            microStopwatch.Stop();
        }
    }
}