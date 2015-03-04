// FFXIVAPP.Client
// Japanese.cs
// 
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions are met: 
// 
//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright 
//    notice, this list of conditions and the following disclaimer in the 
//    documentation and/or other materials provided with the distribution. 
//  * Neither the name of SyndicatedLife nor the names of its contributors may 
//    be used to endorse or promote products derived from this software 
//    without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE. 

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
            return Dictionary;
        }
    }
}
