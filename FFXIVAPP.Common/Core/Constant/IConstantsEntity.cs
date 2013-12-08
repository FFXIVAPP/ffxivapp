// FFXIVAPP.Common
// IConstantEntry.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.Collections.Generic;
using System.Globalization;

#endregion

namespace FFXIVAPP.Common.Core.Constant
{
    public interface IConstantsEntity
    {
        Dictionary<string, string> AutoTranslate { get; set; }
        Dictionary<string, string> ChatCodes { get; set; }
        string ChatCodesXml { get; set; }
        Dictionary<string, string[]> Colors { get; set; }
        CultureInfo CultureInfo { get; set; }
        string CharacterName { get; set; }
        string ServerName { get; set; }
        string GameLanguage { get; set; }
        bool EnableHelpLabels { get; set; }
    }
}
