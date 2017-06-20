// FFXIVAPP.Client ~ Japanese.cs
// 
// Copyright © 2007 - 2017 Ryan Wilson - All Rights Reserved
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

using System.Windows;

namespace FFXIVAPP.Client.Localization
{
    public abstract class Japanese
    {
        private static readonly ResourceDictionary Dictionary = new ResourceDictionary();

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public static ResourceDictionary Context()
        {
            Dictionary.Clear();
            Dictionary.Add("app_", "*PH*");
            Dictionary.Add("app_AllowPluginCommandsHeader", "プラグインコマンドを許可する");
            Dictionary.Add("app_AttachProcessButtonText", "選択したプロセスIDで利用する");
            Dictionary.Add("app_ChangeThemeHeader", "テーマを変更する");
            Dictionary.Add("app_CharacterInformationHeader", "キャラクター情報");
            Dictionary.Add("app_CharacterSettingsTabHeader", "キャラクター設定");
            Dictionary.Add("app_CodeHeader", "コード");
            Dictionary.Add("app_CodeLabel", "コード：");
            Dictionary.Add("app_ColorHeader", "色");
            Dictionary.Add("app_ColorLabel", "色:");
            Dictionary.Add("app_ColorSettingsTabHeader", "色の設定");
            Dictionary.Add("app_ComingSoonText", "近日公開");
            Dictionary.Add("app_CopyrightLabel", "著作権:");
            Dictionary.Add("app_CurrentLabel", "最近:");
            Dictionary.Add("app_DefaultSettingsButtonText", "デフォルト設定");
            Dictionary.Add("app_DeleteMessage", "削除する");
            Dictionary.Add("app_DescriptionHeader", "内容");
            Dictionary.Add("app_DescriptionLabel", "内容:");
            Dictionary.Add("app_EnableNLogHeader", "NLogでのログ取得を有効にする");
            Dictionary.Add("app_CharacterNameLabel", "名:");
            Dictionary.Add("app_FirstNameLabel", "名前:");
            Dictionary.Add("app_GameLanguageLabel", "表示言語:");
            Dictionary.Add("app_ImportLodestoneIDButtonText", "Lodestone IDを取得する");
            Dictionary.Add("app_InformationMessage", "お知らせ!");
            Dictionary.Add("app_LastNameLabel", "名字:");
            Dictionary.Add("app_LatestLabel", "最新:");
            Dictionary.Add("app_LodestoneIDLabel", "Lodestone ID");
            Dictionary.Add("app_MainToolTip", "メイン");
            Dictionary.Add("app_MainSettingsTabHeader", "メイン設定");
            Dictionary.Add("app_CancelButtonText", "いいえ");
            Dictionary.Add("app_PluginsToolTip", "プラグイン");
            Dictionary.Add("app_PluginSettingsTabHeader", "プラグインの設定");
            Dictionary.Add("app_PluginWarningText", "ロードされた全てのプラグインのコマンドをあなたのゲームに送信する。信頼できる場合はこれを有効にする。");
            Dictionary.Add("app_ProcessIDHeader", "現在のプロセスID");
            Dictionary.Add("app_RefreshProcessButtonText", "プロセスリストを更新する");
            Dictionary.Add("app_SaveCharacterButtonText", "このキャラクターで保存する");
            Dictionary.Add("app_SaveAndClearHistoryToolTip", "チャット記録を保存して消去する");
            Dictionary.Add("app_SaveLogHeader", "ログを保存する");
            Dictionary.Add("app_ScreenShotToolTip", "スクリーンショット");
            Dictionary.Add("app_ServerLabel", "ワールド:");
            Dictionary.Add("app_SettingsToolTip", "設定");
            Dictionary.Add("app_TabSettingsTabHeader", "タブ設定");
            Dictionary.Add("app_UpdateColorButtonText", "この色設定で更新する");
            Dictionary.Add("app_VersionInformationHeader", "バージョン情報");
            Dictionary.Add("app_VersionLabel", "バージョン:");
            Dictionary.Add("app_WarningMessage", "注意！");
            Dictionary.Add("app_YesButtonText", "はい");
            Dictionary.Add("app_OtherOptionsTabHeader", "別の選択肢");
            Dictionary.Add("app_AboutToolTip", "約");
            Dictionary.Add("app_ManualUpdateButtonText", "手動アップデート");
            Dictionary.Add("app_TranslationsHeader", "翻訳");
            Dictionary.Add("app_DonationsContributionsHeader", "寄付＆貢献");
            Dictionary.Add("app_SpecialThanksHeader", "スペシャルサンクス");
            Dictionary.Add("app_DownloadNoticeHeader", "利用可能な更新があります！");
            Dictionary.Add("app_DownloadNoticeMessage", "ダウンロードしますか？");
            Dictionary.Add("app_IntegrationWarningText", "この設定を許可しても個人を特定する情報（ゲーム内・外問わず）はサーバーには一切送信されません。ゲームに関する情報の送信を許可するだけです。\n\n送られる情報はモンスター討伐、ドロップアイテム、モンスターの出現位置、NPCと採集場所に関する情報です。\n\nこの設定は全くのオプションでいつでもオン・オフ出来ます。");
            Dictionary.Add("app_EnableHelpLabelsHeader", "ラベルヘルプを有効にする");
            Dictionary.Add("app_OKButtonText", "OK");
            Dictionary.Add("app_TopMostHeader", "常に上に表示する");
            Dictionary.Add("app_OfficialPluginsHeader", "公式プラグイン");
            Dictionary.Add("app_ThirdPartyPluginsHeader", "サードパーティプラグイン");
            Dictionary.Add("app_IntegrationSettingsTabHeader", "連動設定");
            Dictionary.Add("app_NoPluginsLineOneTextBlock", "すべてのプラグインをオン・オフしてあるか、プラグインがロードされていません");
            Dictionary.Add("app_NoPluginsLineTwoTextBlock", "設定確認後にタブメニューから読み込まれたプラグインアイコンを選んで下さい。");
            Dictionary.Add("app_AttachProcessHelptextLabel", "*ゲーム本体を再起動あるいは二重起動している場合のみ選択");
            Dictionary.Add("app_UpdateNotesHeader", "更新情報");
            Dictionary.Add("app_ChangesOnRestartMessage", "変更はアプリケーション再起動後に有効になります");
            Dictionary.Add("app_AvailablePluginsTabHeader", "利用可能なプラグイン");
            Dictionary.Add("app_PluginSourcesTabHeader", "プラグインのソース");
            Dictionary.Add("app_SourceLabel", "ソース:");
            Dictionary.Add("app_EnabledHeader", "有効済み");
            Dictionary.Add("app_VersionHeader", "バージョン");
            Dictionary.Add("app_StatusHeader", "状態");
            Dictionary.Add("app_FilesHeader", "ファイル");
            Dictionary.Add("app_SourceURIHeader", "SourceURI");
            Dictionary.Add("app_AddUpdateSourceButtonText", "ソースを追加または更新する");
            Dictionary.Add("app_RefreshPluginsButtonText", "プラグイン情報を更新する");
            Dictionary.Add("app_UnInstallButtonText", "削除する");
            Dictionary.Add("app_InstallButtonText", "インストール");
            Dictionary.Add("app_AddOrUpdateSourceButtonText", "ソースを追加または更新する");
            Dictionary.Add("app_NameHeader", "名前");
            Dictionary.Add("app_UpdateToolTip", "更新する");
            Dictionary.Add("app_pluginUpdateTitle", "プラグインの更新があります!");
            Dictionary.Add("app_pluginUpdateMessageText", "いくつかのプラグインに利用可能な最新の更新があります。全体の動作を保証するために\"更新する\"タブから更新を行って下さい。");
            Dictionary.Add("app_CurrentVersionHeader", "現在");
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
            Dictionary.Add("app_CacheMemoryJSONDataHeader", "Cache Memory JSON Data Locally");
            Dictionary.Add("app_RefreshMemoryWorkersButtonText", "Refresh Memory Workers");
            return Dictionary;
        }
    }
}
