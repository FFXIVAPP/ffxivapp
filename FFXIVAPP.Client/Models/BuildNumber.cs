// FFXIVAPP.Client
// BuildNumber.cs
// 
// © 2013 Ryan Wilson

using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Models
{
    [DoNotObfuscate]
    public class BuildNumber
    {
        public BuildNumber()
        {
            Major = 0;
            Minor = 0;
            Build = 0;
            Revision = 0;
        }

        public int Major { get; set; }
        public int Minor { get; set; }
        public int Build { get; set; }
        public int Revision { get; set; }
    }
}
