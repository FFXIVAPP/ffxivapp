// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Japanese.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2020 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   Japanese.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Client.Localization {
    using System.Windows;

    internal abstract class Japanese {
        private static readonly ResourceDictionary Dictionary = new ResourceDictionary();

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public static ResourceDictionary Context() {
            Dictionary.Clear();
            Dictionary.Add("app_", "*PH*");
            Dictionary.Add("app_AllowPluginCommandsHeader", "プラグインによるコマンドを許可する");
            Dictionary.Add("app_AttachProcessButtonText", "選択したプロセスにアタッチする");
            Dictionary.Add("app_ChangeThemeHeader", "テーマの変更");
            Dictionary.Add("app_CharacterInformationHeader", "キャラクター情報");
            Dictionary.Add("app_CharacterSettingsTabHeader", "キャラクター設定");
            Dictionary.Add("app_CodeHeader", "コード");
            Dictionary.Add("app_CodeLabel", "コード：");
            Dictionary.Add("app_ColorHeader", "色");
            Dictionary.Add("app_ColorLabel", "色：");
            Dictionary.Add("app_ColorSettingsTabHeader", "色設定");
            Dictionary.Add("app_ComingSoonText", "Coming Soon!");
            Dictionary.Add("app_CopyrightLabel", "著作権表記：");
            Dictionary.Add("app_CurrentLabel", "現行：");
            Dictionary.Add("app_DefaultSettingsButtonText", "デフォルト設定");
            Dictionary.Add("app_DeleteMessage", "削除");
            Dictionary.Add("app_DescriptionHeader", "説明");
            Dictionary.Add("app_DescriptionLabel", "説明：");
            Dictionary.Add("app_EnableNLogHeader", "NLogによるロギングを有効にする");
            Dictionary.Add("app_CharacterNameLabel", "キャラクター名");
            Dictionary.Add("app_FirstNameLabel", "名前：");
            Dictionary.Add("app_GameLanguageLabel", "ゲームの言語：");
            Dictionary.Add("app_ImportLodestoneIDButtonText", "Lodestone IDをインポート");
            Dictionary.Add("app_InformationMessage", "情報！");
            Dictionary.Add("app_LastNameLabel", "名字：");
            Dictionary.Add("app_LatestLabel", "最新版：");
            Dictionary.Add("app_LodestoneIDLabel", "Lodestone ID");
            Dictionary.Add("app_MainToolTip", "メイン");
            Dictionary.Add("app_MainSettingsTabHeader", "メイン設定");
            Dictionary.Add("app_CancelButtonText", "キャンセル");
            Dictionary.Add("app_PluginsToolTip", "プラグイン");
            Dictionary.Add("app_PluginSettingsTabHeader", "プラグイン設定");
            Dictionary.Add("app_PluginWarningText", "読み込まれたプラグインはゲームにコマンドを送ります。信用する場合のみ有効にしてください。");
            Dictionary.Add("app_ProcessIDHeader", "現在のプロセスID");
            Dictionary.Add("app_RefreshProcessButtonText", "プロセスリストをリフレッシュ");
            Dictionary.Add("app_SaveCharacterButtonText", "キャラクターを保存");
            Dictionary.Add("app_SaveAndClearHistoryToolTip", "チャット履歴をクリアして保存");
            Dictionary.Add("app_SaveLogHeader", "ログの保存");
            Dictionary.Add("app_ScreenShotToolTip", "スクリーンショット");
            Dictionary.Add("app_ServerLabel", "ワールド：");
            Dictionary.Add("app_SettingsToolTip", "設定");
            Dictionary.Add("app_TabSettingsTabHeader", "タブ設定");
            Dictionary.Add("app_UpdateColorButtonText", "色を更新");
            Dictionary.Add("app_VersionInformationHeader", "バージョン情報");
            Dictionary.Add("app_VersionLabel", "バージョン：");
            Dictionary.Add("app_WarningMessage", "警告！");
            Dictionary.Add("app_YesButtonText", "はい");
            Dictionary.Add("app_OtherOptionsTabHeader", "その他のオプション");
            Dictionary.Add("app_AboutToolTip", "FFXIVAPPについて");
            Dictionary.Add("app_ManualUpdateButtonText", "手動更新");
            Dictionary.Add("app_TranslationsHeader", "翻訳");
            Dictionary.Add("app_DonationsContributionsHeader", "寄付と貢献者");
            Dictionary.Add("app_SpecialThanksHeader", "スペシャルサンクス");
            Dictionary.Add("app_DownloadNoticeHeader", "更新が利用可能です！");
            Dictionary.Add("app_DownloadNoticeMessage", "ダウンロードしますか？");
            Dictionary.Add("app_IntegrationWarningText", "この機能を有効にすることで個人を特定できる情報（ゲーム内やリアルでの行動）をサーバーに送信することはありません。あくまでも、ゲーム関連の情報の収集のみ使います。\n\nここで収集される情報は、モンスターの討伐状況、戦利品、モンスターのスポーン位置、NPCの位置、ギャザリングの位置などです。\n\nこの機能は、完全にオプションで、いつでもオン/オフできます。");
            Dictionary.Add("app_EnableHelpLabelsHeader", "ヘルプラベルを有効化");
            Dictionary.Add("app_OKButtonText", "OK");
            Dictionary.Add("app_TopMostHeader", "常に前面に表示");
            Dictionary.Add("app_OfficialPluginsTabHeader", "公式プラグイン");
            Dictionary.Add("app_ThirdPartyPluginsTabHeader", "サードパーティ製プラグイン");
            Dictionary.Add("app_IntegrationSettingsTabHeader", "連携設定");
            Dictionary.Add("app_NoPluginsLineOneTextBlock", "プラグインが読み込まれていないか、オン・オフ操作を行った直後の可能性があります。");
            Dictionary.Add("app_NoPluginsLineTwoTextBlock", "タブメニューに表示される読み込まれたプラグインのアイコンを選んでください。");
            Dictionary.Add("app_AlwaysReadUpdatesMessage", "必ずすべての更新履歴をお読みください。");
            Dictionary.Add("app_UpdateNotesHeader", "更新ノート");
            Dictionary.Add("app_ChangesOnRestartMessage", "変更はアプリケーションを再起動後有効になります。.");
            Dictionary.Add("app_AvailablePluginsTabHeader", "利用可能なプラグイン");
            Dictionary.Add("app_PluginSourcesTabHeader", "プラグインのソース");
            Dictionary.Add("app_SourceLabel", "ソース：");
            Dictionary.Add("app_EnabledHeader", "有効");
            Dictionary.Add("app_VersionHeader", "バージョン");
            Dictionary.Add("app_StatusHeader", "ステータス");
            Dictionary.Add("app_FilesHeader", "ファイル");
            Dictionary.Add("app_SourceURIHeader", "ソースのURI");
            Dictionary.Add("app_AddUpdateSourceButtonText", "ソースを追加・更新");
            Dictionary.Add("app_RefreshPluginsButtonText", "プラグインの更新");
            Dictionary.Add("app_UnInstallButtonText", "アンインストール");
            Dictionary.Add("app_InstallButtonText", "インストール");
            Dictionary.Add("app_AddOrUpdateSourceButtonText", "ソースを追加・更新");
            Dictionary.Add("app_NameHeader", "名前");
            Dictionary.Add("app_UpdateToolTip", "更新");
            Dictionary.Add("app_pluginUpdateTitle", "プラグインの更新があります！");
            Dictionary.Add("app_pluginUpdateMessageText", "プラグインの更新があるようです。互換性を確保するため、できるだけ早く「更新」タブから更新してください。");
            Dictionary.Add("app_CurrentVersionHeader", "現行");
            Dictionary.Add("app_LatestVersionHeader", "最新");
            Dictionary.Add("app_UILanguageChangeWarningGeneral", "ゲームの言語設定をこのアプリケーションの言語設定と同期してもよろしいですか？あとで手動で言語設定を変更することもできます。");
            Dictionary.Add("app_UILanguageChangeWarningChinese", " 中国語に変更する場合はアプリケーションの再起動が必要になります。");
            Dictionary.Add("app_UILanguageChangeWarningKorean", " 韓国語に変更する場合はアプリケーションの再起動が必要になります。");
            Dictionary.Add("app_UILanguageChangeWarningNoGameLanguage", "選択されたUIの言語はゲームの言語ではサポートされていません。ゲームの言語設定で指定している言語を指定してください。");
            Dictionary.Add("app_UIScaleHeader", "UIの拡大率");
            Dictionary.Add("app_HomePluginLabel", "ホームプラグイン");
            Dictionary.Add("app_ProcessSelectedInfo", "*Only use this if you restarted the game or are dual-boxing.");
            Dictionary.Add("app_PALSettingsTabHeader", "パフォーマンスとロギング");
            Dictionary.Add("app_DefNetInterfaceLabel", "デフォルトのネットワークインターフェース（パケット読み込み）");
            Dictionary.Add("app_EnableNetReadingLabel", "ネットワーク読み込みを有効化");
            Dictionary.Add("app_BTNResNetWorker", "ネットワークのワーカーをリセット");
            Dictionary.Add("app_DefAudioDeviceLabel", "デフォルトのオーディオデバイス");
            Dictionary.Add("app_MemScanSpeedLabel", "メモリスキャンのスピード（ミリ秒）");
            Dictionary.Add("app_ActorMSSLabel", "演者（ターゲット可能なものすべて）");
            Dictionary.Add("app_ChatLogMSSLabel", "チャットログ");
            Dictionary.Add("app_PartyInfMSSLabel", "パーティー情報");
            Dictionary.Add("app_PlayerInfMSSLabel", "プレイヤー情報（あなた）");
            Dictionary.Add("app_TargEnmMSSLabel", "ターゲットと敵視");
            Dictionary.Add("app_InvMSSLabel", "持ち物");
            Dictionary.Add("app_NetworkUseWinPCapLabel", "ネットワーク読み込みにWinPCapを使用する");
            Dictionary.Add("app_UseLocalMemoryJSONDataCacheHeader", "JSONのデータをローカルメモリにキャッシュ");
            Dictionary.Add("app_RefreshMemoryWorkersButtonText", "メモリーワーカーをリフレッシュ");
            return Dictionary;
        }
    }
}