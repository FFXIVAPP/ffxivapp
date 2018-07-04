// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuildUtilities.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   BuildUtilities.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.Utilities {
    using System;

    using FFXIVAPP.Client.Models;

    internal static class BuildUtilities {
        public static bool NeedsUpdate(string latest, string current, ref BuildNumber latestBuildNumber, ref BuildNumber currentBuildNumber) {
            string[] latestSplit = latest.Split('.');
            string[] currentSplit = current.Split('.');
            try {
                latestBuildNumber.Major = int.Parse(latestSplit[0]);
                latestBuildNumber.Minor = int.Parse(latestSplit[1]);
                latestBuildNumber.Build = int.Parse(latestSplit[2]);
                latestBuildNumber.Revision = int.Parse(latestSplit[3]);
                currentBuildNumber.Major = int.Parse(currentSplit[0]);
                currentBuildNumber.Minor = int.Parse(currentSplit[1]);
                currentBuildNumber.Build = int.Parse(currentSplit[2]);
                currentBuildNumber.Revision = int.Parse(currentSplit[3]);
            }
            catch (Exception) {
                return false;
            }

            if (latestBuildNumber.Major <= currentBuildNumber.Major) {
                if (latestBuildNumber.Minor <= currentBuildNumber.Minor) {
                    if (latestBuildNumber.Build == currentBuildNumber.Build) {
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