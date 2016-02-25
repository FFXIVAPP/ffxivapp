// FFXIVAPP.Client ~ NotifyIconHelper.cs
// 
// Copyright © 2007 - 2016 Ryan Wilson - All Rights Reserved
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Windows.Forms;

namespace FFXIVAPP.Client.Helpers
{
    internal static class NotifyIconHelper
    {
        /// <summary>
        /// </summary>
        /// <param name="title"> </param>
        /// <param name="message"> </param>
        /// <param name="eventHandler"></param>
        public static void ShowBalloonMessage(string title = "Information!", string message = "Unassigned Message", EventHandler eventHandler = null)
        {
            if (eventHandler != null)
            {
                AppViewModel.Instance.NotifyIcon.BalloonTipClicked += eventHandler;
            }
            AppViewModel.Instance.NotifyIcon.ShowBalloonTip(3000, title, message, ToolTipIcon.Info);
        }
    }
}
