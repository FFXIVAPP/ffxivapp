// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MicroTimerEventArgs.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2020 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   MicroTimerEventArgs.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.Utilities {
    using System;

    internal class MicroTimerEventArgs : EventArgs {
        // Simple counter, number times timed event (callback function) executed
        public MicroTimerEventArgs(int timerCount, long elapsedMicroseconds, long timerLateBy, long callbackFunctionExecutionTime) {
            this.TimerCount = timerCount;
            this.ElapsedMicroseconds = elapsedMicroseconds;
            this.TimerLateBy = timerLateBy;
            this.CallbackFunctionExecutionTime = callbackFunctionExecutionTime;
        }

        // Time it took to execute previous call to callback function (OnTimedEvent)
        public long CallbackFunctionExecutionTime { get; private set; }

        // Time when timed event was called since timer started
        public long ElapsedMicroseconds { get; private set; }

        public int TimerCount { get; private set; }

        // How late the timer was compared to when it should have been called
        public long TimerLateBy { get; private set; }
    }
}