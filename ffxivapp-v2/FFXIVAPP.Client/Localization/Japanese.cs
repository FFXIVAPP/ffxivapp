// FFXIVAPP.Client
// Japanese.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System.Windows;

#endregion

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
            Dictionary.Add("app_PLACEHOLDER", "*PH*");
            Dictionary.Add("app_AllowPluginCommandsHeader", "プラグインコマンドを許可します");
            Dictionary.Add("app_AttachProcessButtonText", "選択した手順を貼付けます");
            Dictionary.Add("app_ChangeThemeHeader", "テーマを変更します");
            Dictionary.Add("app_CharacterInformationHeader", "キャラクターの情報");
            Dictionary.Add("app_CharacterSettingsTabHeader", "キャラクターの設定");
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
            Dictionary.Add("app_DescriptionHeader", "記述");
            Dictionary.Add("app_DescriptionLabel", "記述:");
            Dictionary.Add("app_EnableNLogHeader", "NLogでログを有効にする");
            Dictionary.Add("app_FirstNameLabel", "名前");
            Dictionary.Add("app_GameLanguageLabel", "ゲームの言語");
            Dictionary.Add("app_ImportLodestoneIDButtonText", "Lodestone IDを取得");
            Dictionary.Add("app_InformationMessage", "情報!");
            Dictionary.Add("app_LastNameLabel", "名字");
            Dictionary.Add("app_LatestLabel", "最近:");
            Dictionary.Add("app_LodestoneIDLabel", "Lodestone ID:");
            Dictionary.Add("app_MainToolTip", "メイン");
            Dictionary.Add("app_MainSettingsTabHeader", "メイン設定");
            Dictionary.Add("app_NoButtonText", "いいえ");
            Dictionary.Add("app_PluginsToolTip", "プラグイン");
            Dictionary.Add("app_PluginSettingsTabHeader", "プラグインの設定");
            Dictionary.Add("app_PluginWarningText", "ロードされた全てのプラグインのコマンドをあなたのゲームに送信する。信頼できる場合はこれを有効にする。");
            Dictionary.Add("app_ProcessIDHeader", "現在のプロセスID");
            Dictionary.Add("app_RefreshProcessButtonText", "プロセスリストをリフレッシュする");
            Dictionary.Add("app_SaveCharacterButtonText", "キャラクターを保存します");
            Dictionary.Add("app_SaveHistoryMessage", "チャット記録をエクスポートする場合は\"はい\"を押してください。\"いいえ\"を押して戻る。 エクスポートしたあとアプリケーションを閉じる");
            Dictionary.Add("app_SaveLogHeader", "ログを保存する");
            Dictionary.Add("app_ScreenshotToolTip", "スクリーンショット");
            Dictionary.Add("app_ServerLabel", "サーバ:");
            Dictionary.Add("app_SettingsToolTip", "設定");
            Dictionary.Add("app_TabSettingsTabHeader", "タブの設定");
            Dictionary.Add("app_UpdateColorButtonText", "色の更新");
            Dictionary.Add("app_VersionHeader", "バージョン情報");
            Dictionary.Add("app_VersionLabel", "バージョン:");
            Dictionary.Add("app_WarningMessage", "注意！");
            Dictionary.Add("app_YesButtonText", "はい");
            Dictionary.Add("app_OtherOptionsTabHeader", "別の選択肢");
            return Dictionary;
        }
    }
}
