// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Chinese.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2020 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   Chinese.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.Localization {
    using System.Windows;

    internal abstract class Chinese {
        private static readonly ResourceDictionary Dictionary = new ResourceDictionary();

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public static ResourceDictionary Context() {
            Dictionary.Clear();
            Dictionary.Add("app_", "*PH*");
            Dictionary.Add("app_AllowPluginCommandsHeader", "允许插件命令");
            Dictionary.Add("app_AttachProcessButtonText", "加载选择进程");
            Dictionary.Add("app_ChangeThemeHeader", "改变主题");
            Dictionary.Add("app_CharacterInformationHeader", "角色信息");
            Dictionary.Add("app_CharacterSettingsTabHeader", "角色设置");
            Dictionary.Add("app_CodeHeader", "代码");
            Dictionary.Add("app_CodeLabel", "代码:");
            Dictionary.Add("app_ColorHeader", "颜色");
            Dictionary.Add("app_ColorLabel", "颜色:");
            Dictionary.Add("app_ColorSettingsTabHeader", "颜色设置");
            Dictionary.Add("app_ComingSoonText", "即将推出!");
            Dictionary.Add("app_CopyrightLabel", "版权:");
            Dictionary.Add("app_CurrentLabel", "目前:");
            Dictionary.Add("app_DefaultSettingsButtonText", "默认设置");
            Dictionary.Add("app_DeleteMessage", "删除");
            Dictionary.Add("app_DescriptionHeader", "描述");
            Dictionary.Add("app_DescriptionLabel", "描述:");
            Dictionary.Add("app_EnableNLogHeader", "用NLog开启记录");
            Dictionary.Add("app_CharacterNameLabel", "Character Name:");
            Dictionary.Add("app_FirstNameLabel", "名:");
            Dictionary.Add("app_GameLanguageLabel", "游戏语言:");
            Dictionary.Add("app_ImportLodestoneIDButtonText", "输入 磁石 ID");
            Dictionary.Add("app_InformationMessage", "信息!");
            Dictionary.Add("app_LastNameLabel", "姓:");
            Dictionary.Add("app_LatestLabel", "最新:");
            Dictionary.Add("app_LodestoneIDLabel", "磁石 ID");
            Dictionary.Add("app_MainToolTip", "主菜单");
            Dictionary.Add("app_MainSettingsTabHeader", "主菜单设置");
            Dictionary.Add("app_CancelButtonText", "取消");
            Dictionary.Add("app_PluginsToolTip", "插件");
            Dictionary.Add("app_PluginSettingsTabHeader", "插件设置");
            Dictionary.Add("app_PluginWarningText", "此选项将允许所有加载的插件发送指令到你的游戏. 请只在你信任的情况下开启此选项.");
            Dictionary.Add("app_ProcessIDHeader", "当前进程 ID");
            Dictionary.Add("app_RefreshProcessButtonText", "刷新进程表");
            Dictionary.Add("app_SaveCharacterButtonText", "保存角色");
            Dictionary.Add("app_SaveAndClearHistoryToolTip", "保存 & 清除聊天记录");
            Dictionary.Add("app_SaveLogHeader", "保存记录");
            Dictionary.Add("app_ScreenShotToolTip", "截屏");
            Dictionary.Add("app_ServerLabel", "服务器:");
            Dictionary.Add("app_SettingsToolTip", "设置");
            Dictionary.Add("app_TabSettingsTabHeader", "Tab 设置");
            Dictionary.Add("app_UpdateColorButtonText", "改变颜色");
            Dictionary.Add("app_VersionInformationHeader", "版本信息");
            Dictionary.Add("app_VersionLabel", "版本:");
            Dictionary.Add("app_WarningMessage", "警告!");
            Dictionary.Add("app_YesButtonText", "是");
            Dictionary.Add("app_OtherOptionsTabHeader", "其他选项");
            Dictionary.Add("app_AboutToolTip", "简介");
            Dictionary.Add("app_ManualUpdateButtonText", "手册更新");
            Dictionary.Add("app_TranslationsHeader", "翻译");
            Dictionary.Add("app_DonationsContributionsHeader", "捐款 & 贡献");
            Dictionary.Add("app_SpecialThanksHeader", "特殊感谢");
            Dictionary.Add("app_DownloadNoticeHeader", "可用更新!");
            Dictionary.Add("app_DownloadNoticeMessage", "下载?");
            Dictionary.Add("app_IntegrationWarningText", "开启此选项不会将个人信息（游戏或现实生活）上传服务器. 你只是对收集游戏数据进行授权.\n\n被处理的信息是关于怪兽死亡，赃物，怪兽卵巢，npc and gathering 地点.\n\n此选项可随时开启或关闭.");
            Dictionary.Add("app_EnableHelpLabelsHeader", "开启帮助标签");
            Dictionary.Add("app_OKButtonText", "确定");
            Dictionary.Add("app_TopMostHeader", "总是悬浮在上面");
            Dictionary.Add("app_OfficialPluginsTabHeader", "官方插件");
            Dictionary.Add("app_ThirdPartyPluginsTabHeader", "第三方插件");
            Dictionary.Add("app_IntegrationSettingsTabHeader", "集成设置");
            Dictionary.Add("app_NoPluginsLineOneTextBlock", "你可能最近开启或关闭了所有插件；或者从味开启.");
            Dictionary.Add("app_NoPluginsLineTwoTextBlock", "如果已经启动请确认你的设置并从tab菜单选择插件图标.");
            Dictionary.Add("app_AlwaysReadUpdatesMessage", "需要阅读关于所有改变的更改历史记录.");
            Dictionary.Add("app_UpdateNotesHeader", "更新记录");
            Dictionary.Add("app_ChangesOnRestartMessage", "重启程序使改变生效.");
            Dictionary.Add("app_AvailablePluginsTabHeader", "可用插件");
            Dictionary.Add("app_PluginSourcesTabHeader", "插件资源");
            Dictionary.Add("app_SourceLabel", "资源:");
            Dictionary.Add("app_EnabledHeader", "开启");
            Dictionary.Add("app_VersionHeader", "版本");
            Dictionary.Add("app_StatusHeader", "状态");
            Dictionary.Add("app_FilesHeader", "文件");
            Dictionary.Add("app_SourceURIHeader", "资源链接");
            Dictionary.Add("app_AddUpdateSourceButtonText", "新添或更改资源");
            Dictionary.Add("app_RefreshPluginsButtonText", "刷新插件");
            Dictionary.Add("app_UnInstallButtonText", "卸载");
            Dictionary.Add("app_InstallButtonText", "安装");
            Dictionary.Add("app_AddOrUpdateSourceButtonText", "新添或更改资源");
            Dictionary.Add("app_NameHeader", "名称");
            Dictionary.Add("app_UpdateToolTip", "更改");
            Dictionary.Add("app_pluginUpdateTitle", "插件更新!");
            Dictionary.Add("app_pluginUpdateMessageText", "看來某些插件有更新。為了確保兼容性, 請在 \"更新\" 選項內更新插件.");
            Dictionary.Add("app_CurrentVersionHeader", "现在");
            Dictionary.Add("app_LatestVersionHeader", "最新");
            Dictionary.Add("app_UILanguageChangeWarningGeneral", "Do you want to change the GameLanguage setting as well to match this applications UILanguage? If you cancel you will manually have to change GameLanguage in Settings later.");
            Dictionary.Add("app_UILanguageChangeWarningChinese", " When changing to or from Chinese an application restart is also required.");
            Dictionary.Add("app_UILanguageChangeWarningNoGameLanguage", "The selected UILanguage does not have a supported GameLanguage. Please choose your game language in Settings.");
            Dictionary.Add("app_UIScaleHeader", "UI.Scale");
            Dictionary.Add("app_HomePluginLabel", "Home Plugin");
            Dictionary.Add("app_ProcessSelectedInfo", "*Only use this if you restarted the game or are dual-boxing.");
            Dictionary.Add("app_PALSettingsTabHeader", "Performance & Logging");
            Dictionary.Add("app_DefNetInterfaceLabel", "Default Network Interface (Packet Reading)");
            Dictionary.Add("app_EnableNetReadingLabel", "Enable Network Reading");
            Dictionary.Add("app_BTNResNetWorker", "Reset Network Worker");
            Dictionary.Add("app_DefAudioDeviceLabel", "Default Audio Device");
            Dictionary.Add("app_MemScanSpeedLabel", "Memory Scanning Speed (Milliseconds)");
            Dictionary.Add("app_ActorMSSLabel", "Actors (Anything Targetable)");
            Dictionary.Add("app_ChatLogMSSLabel", "ChatLog");
            Dictionary.Add("app_PlayerInfMSSLabel", "Player Info (YOU)");
            Dictionary.Add("app_TargEnmMSSLabel", "Targets &amp; Enmity");
            Dictionary.Add("app_InvMSSLabel", "Inventory");
            Dictionary.Add("app_NetworkUseWinPCapLabel", "Use WinPCap For Network Reading");
            Dictionary.Add("app_UseLocalMemoryJSONDataCacheHeader", "Cache Memory JSON Data Locally");
            Dictionary.Add("app_RefreshMemoryWorkersButtonText", "Refresh Memory Workers");
            return Dictionary;
        }
    }
}