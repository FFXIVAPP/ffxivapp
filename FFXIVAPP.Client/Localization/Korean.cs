// FFXIVAPP.Client
// English.cs
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
    public abstract class English
    {
        private static readonly ResourceDictionary Dictionary = new ResourceDictionary();

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public static ResourceDictionary Context()
        {
            Dictionary.Clear();
            Dictionary.Add("app_", "*PH*");
            Dictionary.Add("app_AllowPluginCommandsHeader", "플러그인이 게임에 간섭할 수 있도록 허용");
            Dictionary.Add("app_AttachProcessButtonText", "선택한 프로세스에 연결");
            Dictionary.Add("app_ChangeThemeHeader", "테마 변경");
            Dictionary.Add("app_CharacterInformationHeader", "캐릭터 정보");
            Dictionary.Add("app_CharacterSettingsTabHeader", "캐릭터 설정");
            Dictionary.Add("app_CodeHeader", "코드");
            Dictionary.Add("app_CodeLabel", "코드:");
            Dictionary.Add("app_ColorHeader", "색상");
            Dictionary.Add("app_ColorLabel", "색상:");
            Dictionary.Add("app_ColorSettingsTabHeader", "색상 설정");
            Dictionary.Add("app_ComingSoonText", "곧 준비됩니다!");
            Dictionary.Add("app_CopyrightLabel", "저작권:");
            Dictionary.Add("app_CurrentLabel", "현재:");
            Dictionary.Add("app_DefaultSettingsButtonText", "기본 설정");
            Dictionary.Add("app_DeleteMessage", "삭제");
            Dictionary.Add("app_DescriptionHeader", "설명");
            Dictionary.Add("app_DescriptionLabel", "설명:");
            Dictionary.Add("app_EnableNLogHeader", "NLog를 사용해 정보 기록하기");
            Dictionary.Add("app_CharacterNameLabel", "캐릭터 이름:");
            Dictionary.Add("app_FirstNameLabel", "이름:");
            Dictionary.Add("app_GameLanguageLabel", "게임 언어:");
            Dictionary.Add("app_ImportLodestoneIDButtonText", "로드스톤 ID 가져오기");
            Dictionary.Add("app_InformationMessage", "정보!");
            Dictionary.Add("app_LastNameLabel", "성:");
            Dictionary.Add("app_LatestLabel", "최신:");
            Dictionary.Add("app_LodestoneIDLabel", "로드스톤 ID");
            Dictionary.Add("app_MainToolTip", "기본");
            Dictionary.Add("app_MainSettingsTabHeader", "기본 설정");
            Dictionary.Add("app_CancelButtonText", "취소");
            Dictionary.Add("app_PluginsToolTip", "플러그인");
            Dictionary.Add("app_PluginSettingsTabHeader", "플러그인 실정");
            Dictionary.Add("app_PluginWarningText", "이 설정을 사용하면 플러그인이 게임에 간섭할 수 있게 됩니다. 플러그인을 신뢰할 수 있을 때만 사용하세요.");
            Dictionary.Add("app_ProcessIDHeader", "현재 프로세스 ID");
            Dictionary.Add("app_RefreshProcessButtonText", "프로세스 목록 새로고침");
            Dictionary.Add("app_SaveCharacterButtonText", "캐릭터 저장");
            Dictionary.Add("app_SaveAndClearHistoryToolTip", "채팅 기록 저장 후 비우기");
            Dictionary.Add("app_SaveLogHeader", "기록 저장하기");
            Dictionary.Add("app_ScreenShotToolTip", "스크린샷");
            Dictionary.Add("app_ServerLabel", "서버:");
            Dictionary.Add("app_SettingsToolTip", "설정");
            Dictionary.Add("app_TabSettingsTabHeader", "탭 설정");
            Dictionary.Add("app_UpdateColorButtonText", "색상 변경");
            Dictionary.Add("app_VersionInformationHeader", "버전 정보");
            Dictionary.Add("app_VersionLabel", "버전:");
            Dictionary.Add("app_WarningMessage", "경고!");
            Dictionary.Add("app_YesButtonText", "네");
            Dictionary.Add("app_OtherOptionsTabHeader", "기타 설정");
            Dictionary.Add("app_AboutToolTip", "정보");
            Dictionary.Add("app_ManualUpdateButtonText", "수동 업데이트");
            Dictionary.Add("app_TranslationsHeader", "번역");
            Dictionary.Add("app_DonationsContributionsHeader", "기부자 & 기여자");
            Dictionary.Add("app_SpecialThanksHeader", "고마운 분들");
            Dictionary.Add("app_DownloadNoticeHeader", "새 업데이트가 있습니다!");
            Dictionary.Add("app_DownloadNoticeMessage", "다운로드 할까요?");
            Dictionary.Add("app_IntegrationWarningText", "Enabling this option means no personally identifable information (game or real life) is sent to the server.  You would be authorization the collection of game related data only.\n\nThe information processed is monster deaths, loot, monster spawn locations, npc and gathering locations.\n\nThis is completely optional and can be turned on or off at any time.");
            Dictionary.Add("app_EnableHelpLabelsHeader", "도움말 표시");
            Dictionary.Add("app_OKButtonText", "확인");
            Dictionary.Add("app_TopMostHeader", "항상 위에 표시");
            Dictionary.Add("app_OfficialPluginsTabHeader", "공식 플러그인");
            Dictionary.Add("app_ThirdPartyPluginsTabHeader", "서드파티 플러그인");
            Dictionary.Add("app_IntegrationSettingsTabHeader", "연동 설정");
            Dictionary.Add("app_NoPluginsLineOneTextBlock", "You might have recently turned on or off all plugins; or just have nothing loaded at all.");
            Dictionary.Add("app_NoPluginsLineTwoTextBlock", "Confirm your settings and if loaded choose a plugin icon from the tab menu.");
            Dictionary.Add("app_AlwaysReadUpdatesMessage", "Always read the update history for all changes.");
            Dictionary.Add("app_UpdateNotesHeader", "업데이트 사항");
            Dictionary.Add("app_ChangesOnRestartMessage", "프로그램을 재시작하면 변경사항이 적용됩니다.");
            Dictionary.Add("app_AvailablePluginsTabHeader", "사용할 수 있는 플러그인");
            Dictionary.Add("app_PluginSourcesTabHeader", "플러그인 소스");
            Dictionary.Add("app_SourceLabel", "소스:");
            Dictionary.Add("app_EnabledHeader", "사용 중");
            Dictionary.Add("app_VersionHeader", "버전");
            Dictionary.Add("app_StatusHeader", "상태");
            Dictionary.Add("app_FilesHeader", "파일");
            Dictionary.Add("app_SourceURIHeader", "소스 URI");
            Dictionary.Add("app_AddUpdateSourceButtonText", "소스 추가 및 업데이트");
            Dictionary.Add("app_RefreshPluginsButtonText", "플러그인 새로고침");
            Dictionary.Add("app_UnInstallButtonText", "삭제");
            Dictionary.Add("app_InstallButtonText", "설치");
            Dictionary.Add("app_AddOrUpdateSourceButtonText", "소스 추가 및 업데이트");
            Dictionary.Add("app_NameHeader", "이름");
            Dictionary.Add("app_UpdateToolTip", "업데이트");
            Dictionary.Add("app_pluginUpdateTitle", "플러그인 업데이트가 있습니다!");
            Dictionary.Add("app_pluginUpdateMessageText", "업데이트가 있는 플러그인이 있습니다. 호환성과 편의를 위해 \"업데이트\" 탭에서 업데이트 하세요.");
            Dictionary.Add("app_CurrentVersionHeader", "현재");
            Dictionary.Add("app_LatestVersionHeader", "최신");
            Dictionary.Add("app_UILanguageChangeWarningGeneral", "UI 언어와 같이 게임 언어도 변경할까요? 취소할 경우 설정에서 수동으로 게임 언어를 변경해야 합니다.");
            Dictionary.Add("app_UILanguageChangeWarningChinese", " 중국어를 사용하는 경우 재시작이 필요합니다.");
            Dictionary.Add("app_UILanguageChangeWarningKorean", " 한국어를 사용하는 경우 재시작이 필요합니다.");
            Dictionary.Add("app_UILanguageChangeWarningNoGameLanguage", "선택한 UI 언어에 맞는 게임 언어가 없습니다. 설정에서 게임 언어를 선택해주세요.");
            Dictionary.Add("app_UIScaleHeader", "UI 크기");
            Dictionary.Add("app_HomePluginLabel", "Home Plugin");
            Dictionary.Add("app_ProcessSelectedInfo", "*게임을 재시작했거나 동시에 실행 중일때만 사용하세요.");
            Dictionary.Add("app_PALSettingsTabHeader", "성능 및 기록");
            Dictionary.Add("app_DefNetInterfaceLabel", "기본 네트워크 인터페이스 (패킷 읽기)");
            Dictionary.Add("app_EnableNetReadingLabel", "네트워크 읽기 사용");
            Dictionary.Add("app_BTNResNetWorker", "네트워크 작업 초기화");
            Dictionary.Add("app_DefAudioDeviceLabel", "기본 재생 장치");
            Dictionary.Add("app_MemScanSpeedLabel", "메모리 읽기 속도 (밀리세컨드 단위)");
            Dictionary.Add("app_ActorMSSLabel", "배우 (선택 가능한 것들)");
            Dictionary.Add("app_ChatLogMSSLabel", "채팅 기록");
            Dictionary.Add("app_PartyInfMSSLabel", "파티 정보");
            Dictionary.Add("app_PlayerInfMSSLabel", "내 정보");
            Dictionary.Add("app_TargEnmMSSLabel", "대상 &amp; 적개심");
            Dictionary.Add("app_InvMSSLabel", "소지품");
            Dictionary.Add("app_NetworkUseWinPCapLabel", "네트워크 읽을 때 WinPCap 사용하기");
            return Dictionary;
        }
    }
}
