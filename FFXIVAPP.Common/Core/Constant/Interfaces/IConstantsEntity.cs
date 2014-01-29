// FFXIVAPP.Common
// IConstantsEntity.cs
// 
// © 2013 Ryan Wilson

using System.Collections.Generic;
using System.Globalization;

namespace FFXIVAPP.Common.Core.Constant.Interfaces
{
    public interface IConstantsEntity
    {
        string Theme { get; set; }
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
