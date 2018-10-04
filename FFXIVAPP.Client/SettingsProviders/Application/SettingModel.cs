using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using FFXIVAPP.Client.Models;
using Newtonsoft.Json;
using ReactiveUI;

[JsonObject(MemberSerialization.OptOut)]
internal class SettingModel: ReactiveObject
{
    public SettingModel(){
        DefaultSettings();
    } 

    internal void DefaultSettings()
    {
        this.Application_UpgradeRequired = true;
        this.Top = 0;
        this.Left = 0;
        this.Width = 720;
        this.Height = 480;
        this.Culture = new CultureInfo("en");
        this.CultureSet = false;
        this.ThemeList = new string[] {
        "Red|Light",
        "Green|Light",
        "Blue|Light",
        "Purple|Light",
        "Orange|Light",
        "Brown|Light",
        "Cobalt|Light",
        "Crimson|Light",
        "Cyan|Light",
        "Emerald|Light",
        "Indigo|Light",
        "Magenta|Light",
        "Mauve|Light",
        "Olive|Light",
        "Sienna|Light",
        "Steel|Light",
        "Teal|Light",
        "Violet|Light",
        "Amber|Light",
        "Yellow|Light",
        "Lime|Light",
        "Pink|Light",
        "Red|Dark",
        "Green|Dark",
        "Blue|Dark",
        "Purple|Dark",
        "Orange|Dark",
        "Brown|Dark",
        "Cobalt|Dark",
        "Crimson|Dark",
        "Cyan|Dark",
        "Emerald|Dark",
        "Indigo|Dark",
        "Magenta|Dark",
        "Mauve|Dark",
        "Olive|Dark",
        "Sienna|Dark",
        "Steel|Dark",
        "Teal|Dark",
        "Violet|Dark",
        "Amber|Dark",
        "Yellow|Dark",
        "Lime|Dark",
        "Pink|Dark"};
        this.Theme = "Blue|Light";
        this.ServerList = new string[] {
        "Adamantoise",
        "Aegis",
        "Alexander",
        "Anima",
        "Asura",
        "Atomos",
        "Bahamut",
        "Balmung",
        "Behemoth",
        "Belias",
        "Brynhildr",
        "Cactuar",
        "Carbuncle",
        "Cerberus",
        "Chocobo",
        "Coeurl",
        "Diabolos",
        "Durandal",
        "Excalibur",
        "Exodus",
        "Faerie",
        "Famfrit",
        "Fenrir",
        "Garuda",
        "Gilgamesh",
        "Goblin",
        "Gungnir",
        "Hades",
        "Hyperion",
        "Ifrit",
        "Ixion",
        "Jenova",
        "Kujata",
        "Lamia",
        "Leviathan",
        "Lich",
        "Malboro",
        "Mandragora",
        "Masamune",
        "Mateus",
        "Midgardsormr",
        "Moogle",
        "Odin",
        "Pandaemonium",
        "Phoenix",
        "Ragnarok",
        "Ramuh",
        "Ridill",
        "Sargatanas",
        "Shinryu",
        "Shiva",
        "Siren",
        "Tiamat",
        "Titan",
        "Tonberry",
        "Typhon",
        "Ultima",
        "Ultros",
        "Unicorn",
        "Valefor",
        "Yojimbo",
        "Zalera",
        "Zeromus"};
        this.GameLanguageList = new string[] {
        "English",
        "Japanese",
        "French",
        "German",
        "Chinese",
        "Korean"};
        this.GameLanguage = "English";
        /* TODO: this.LicenseFont = Segoe UI*/
        this.SaveLog = true;
        this.EnableNLog = false;
        this.AllowXIVDBIntegration = true;
        this.EnableHelpLabels = true;
        this.TopMost = false;
        /* TODO: this.ChatBackgroundColor = #FF000000*/
        /* TODO: this.TimeStampColor = #FF800080*/
        /* TODO: this.ChatFont = Microsoft Sans Serif, 12pt*/
        this.HomePlugin = "Parse";
        this.HomePluginList = new string[] {
        "None"};
        this.UIScale = "1.0";
        this.UIScaleList = new string[] {
        "0.8",
        "0.9",
        "1.0",
        "1.1",
        "1.2",
        "1.3",
        "1.4",
        "1.5"};
        this.DefaultAudioDevice = "System Default";
        this.DefaultAudioDeviceList = new string[] {
        "System Default"};
        this.ActorWorkerRefresh = "100";
        this.ChatLogWorkerRefresh = "250";
        this.PartyInfoWorkerRefresh = "1000";
        this.PlayerInfoWorkerRefresh = "1000";
        this.TargetWorkerRefresh = "100";
        this.InventoryWorkerRefresh = "1000";
        this.EnableNetworkReading = false;
        this.NetworkUseWinPCap = false;
        this.UseLocalMemoryJSONDataCache = false;
        this.HotBarRecastWorkerRefresh = "100";
        this.PluginSources = new ObservableCollection<PluginSourceItem>();
    }

    public ObservableCollection<PluginSourceItem> PluginSources { get; set; }
    public bool Application_UpgradeRequired { get; set; }
    public int Top { get; set; }
    public int Left { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public CultureInfo Culture { get; set; }
    public bool CultureSet { get; set; }
    public string[] ThemeList { get; set; }
    public string Theme { get; set; }
    public string[] ServerList { get; set; }
    public string[] GameLanguageList { get; set; }
    public string GameLanguage { get; set; }
    public string CICUID { get; set; }
    public string ServerName { get; set; }
    public string CharacterName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    // Unknown type: [System.Windows.Media.FontFamily] public ??? LicenseFont { get; set; }
    public bool SaveLog { get; set; }
    public bool EnableNLog { get; set; }
    public bool AllowXIVDBIntegration { get; set; }
    public bool EnableHelpLabels { get; set; }
    public bool TopMost { get; set; }
    // Unknown type: [System.Windows.Media.Color] public ??? ChatBackgroundColor { get; set; }
    // Unknown type: [System.Windows.Media.Color] public ??? TimeStampColor { get; set; }
    // Unknown type: [System.Drawing.Font] public ??? ChatFont { get; set; }
    public string HomePlugin { get; set; }
    public string[] HomePluginList { get; set; }
    public string UIScale { get; set; }
    public string[] UIScaleList { get; set; }
    public string DefaultAudioDevice { get; set; }
    public string[] DefaultAudioDeviceList { get; set; }
    public string ActorWorkerRefresh { get; set; }
    public string ChatLogWorkerRefresh { get; set; }
    public string PartyInfoWorkerRefresh { get; set; }
    public string PlayerInfoWorkerRefresh { get; set; }
    public string TargetWorkerRefresh { get; set; }
    public string InventoryWorkerRefresh { get; set; }
    public string DefaultNetworkInterface { get; set; }
    public bool EnableNetworkReading { get; set; }
    public string UILanguage { get; set; }
    public bool NetworkUseWinPCap { get; set; }
    public bool UseLocalMemoryJSONDataCache { get; set; }
    public string HotBarRecastWorkerRefresh { get; set; }

    protected void RaisePropertyChanged([CallerMemberName] string caller = "") {
        IReactiveObjectExtensions.RaisePropertyChanged(this, caller);
    }
}
