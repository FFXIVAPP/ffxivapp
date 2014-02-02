// FFXIVAPP.Client
// BuildUtilities.cs
// 
// © 2013 Ryan Wilson

using System;
using FFXIVAPP.Client.Models;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Utilities
{
    [DoNotObfuscate]
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
