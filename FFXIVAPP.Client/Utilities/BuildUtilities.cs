// FFXIVAPP.Client ~ BuildUtilities.cs
// 
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
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
using FFXIVAPP.Client.Models;

namespace FFXIVAPP.Client.Utilities
{
    public static class BuildUtilities
    {
        public static bool NeedsUpdate(string latest, string current, ref BuildNumber latestBuildNumber, ref BuildNumber currentBuildNumber)
        {
            var latestSplit = latest.Split('.');
            var currentSplit = current.Split('.');
            try
            {
                latestBuildNumber.Major = Int32.Parse(latestSplit[0]);
                latestBuildNumber.Minor = Int32.Parse(latestSplit[1]);
                latestBuildNumber.Build = Int32.Parse(latestSplit[2]);
                latestBuildNumber.Revision = Int32.Parse(latestSplit[3]);
                currentBuildNumber.Major = Int32.Parse(currentSplit[0]);
                currentBuildNumber.Minor = Int32.Parse(currentSplit[1]);
                currentBuildNumber.Build = Int32.Parse(currentSplit[2]);
                currentBuildNumber.Revision = Int32.Parse(currentSplit[3]);
            }
            catch (Exception ex)
            {
                return false;
            }
            if (latestBuildNumber.Major <= currentBuildNumber.Major)
            {
                if (latestBuildNumber.Minor <= currentBuildNumber.Minor)
                {
                    if (latestBuildNumber.Build == currentBuildNumber.Build)
                    {
                        return latestBuildNumber.Revision > currentBuildNumber.Revision;
                    }
                    return latestBuildNumber.Build > currentBuildNumber.Build;
                }
                return true;
            }
            return true;
        }
    }
}
