// FFXIVAPP.Client
// Russian.cs
// 
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
// Modified by Yaguar666 ak Yaguar Kuro
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
    public abstract class Russian
    {
        private static readonly ResourceDictionary Dictionary = new ResourceDictionary();

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public static ResourceDictionary Context()
        {
            Dictionary.Clear();
            Dictionary.Add("app_", "*PH*");
            Dictionary.Add("app_AllowPluginCommandsHeader", "Включить команды Плагина");
            Dictionary.Add("app_AttachProcessButtonText", "Добавить выбранный Процесс");
            Dictionary.Add("app_ChangeThemeHeader", "Изменить Тему");
            Dictionary.Add("app_CharacterInformationHeader", "Информация о Персонаже");
            Dictionary.Add("app_CharacterSettingsTabHeader", "Настройки Персонажа");
            Dictionary.Add("app_CodeHeader", "Код");
            Dictionary.Add("app_CodeLabel", "Код:");
            Dictionary.Add("app_ColorHeader", "Цвет");
            Dictionary.Add("app_ColorLabel", "Цвет:");
            Dictionary.Add("app_ColorSettingsTabHeader", "Настройки Цвета");
            Dictionary.Add("app_ComingSoonText", "Скоро!");
            Dictionary.Add("app_CopyrightLabel", "Права принадлежат:");
            Dictionary.Add("app_CurrentLabel", "Текущая версия:");
            Dictionary.Add("app_DefaultSettingsButtonText", "Стандартные Настройки");
            Dictionary.Add("app_DeleteMessage", "Удалить");
            Dictionary.Add("app_DescriptionHeader", "Описание");
            Dictionary.Add("app_DescriptionLabel", "Описание:");
            Dictionary.Add("app_EnableNLogHeader", "Разрешен вход через NLog");
            Dictionary.Add("app_CharacterNameLabel", "Имя Персонажа:");
            Dictionary.Add("app_FirstNameLabel", "Имя:");
            Dictionary.Add("app_GameLanguageLabel", "Язык Игры:");
            Dictionary.Add("app_ImportLodestoneIDButtonText", "Добавить Lodestone ID");
            Dictionary.Add("app_InformationMessage", "Информация!");
            Dictionary.Add("app_LastNameLabel", "Фамилия:");
            Dictionary.Add("app_LatestLabel", "Последняя версия:");
            Dictionary.Add("app_LodestoneIDLabel", "Lodestone ID");
            Dictionary.Add("app_MainToolTip", "Главная");
            Dictionary.Add("app_MainSettingsTabHeader", "Главные настройки");
            Dictionary.Add("app_CancelButtonText", "Отменить");
            Dictionary.Add("app_PluginsToolTip", "Плагины");
            Dictionary.Add("app_PluginSettingsTabHeader", "Настройки Плагина");
            Dictionary.Add("app_PluginWarningText", "Это Опция разрешит Плагину посылать команды в Игру. Включайте эту Опцию, только если вы доверяете этому Плагину.");
            Dictionary.Add("app_ProcessIDHeader", "ID выбранного Процесса");
            Dictionary.Add("app_RefreshProcessButtonText", "Обновить Лист Процессов");
            Dictionary.Add("app_SaveCharacterButtonText", "Сохранить Персонажа");
            Dictionary.Add("app_SaveAndClearHistoryToolTip", "Сохранить и Очистить Историю Чата");
            Dictionary.Add("app_SaveLogHeader", "Сохранить Лог");
            Dictionary.Add("app_ScreenShotToolTip", "Скриншот");
            Dictionary.Add("app_ServerLabel", "Сервер:");
            Dictionary.Add("app_SettingsToolTip", "Настройки");
            Dictionary.Add("app_TabSettingsTabHeader", "Настройки Вкладки");
            Dictionary.Add("app_UpdateColorButtonText", "Обновить Цвет");
            Dictionary.Add("app_VersionInformationHeader", "Информация о версии приложения");
            Dictionary.Add("app_VersionLabel", "Версия:");
            Dictionary.Add("app_WarningMessage", "Внимание!");
            Dictionary.Add("app_YesButtonText", "Да");
            Dictionary.Add("app_OtherOptionsTabHeader", "Другии Опции");
            Dictionary.Add("app_AboutToolTip", "О Программе");
            Dictionary.Add("app_ManualUpdateButtonText", "Обновить программу сейчас");
            Dictionary.Add("app_TranslationsHeader", "Переводы");
            Dictionary.Add("app_DonationsContributionsHeader", "Пожертвования");
            Dictionary.Add("app_SpecialThanksHeader", "Специальное Спасибо");
            Dictionary.Add("app_DownloadNoticeHeader", "Доступно Обновление!");
            Dictionary.Add("app_DownloadNoticeMessage", "Загрузить?");
            Dictionary.Add("app_IntegrationWarningText", "Включение этой Опции обозначает, что персональная информация (из игры или реальной жизни) будет отослана на сервер. Вы будете авторизованы только для сбора данных игры.\n\nОтосланная информация будет содержать убийство монстров, получение предметов, места появления монстров, npc и места сбора ресурсов.\n\nЭта функция может быть отключена и включена в любое время.");
            Dictionary.Add("app_EnableHelpLabelsHeader", "Включить Подсказки");
            Dictionary.Add("app_OKButtonText", "OK");
            Dictionary.Add("app_TopMostHeader", "Всегда поверх Окон");
            Dictionary.Add("app_OfficialPluginsTabHeader", "Оффициальные Плагины");
            Dictionary.Add("app_ThirdPartyPluginsTabHeader", "Сторонние Плагины");
            Dictionary.Add("app_IntegrationSettingsTabHeader", "Настройки Интеграции");
            Dictionary.Add("app_NoPluginsLineOneTextBlock", "Вы, должно быть, недавно включили или выключили все плагины; или ничего не загрузили.");
            Dictionary.Add("app_NoPluginsLineTwoTextBlock", "Подтвердите настройки и, если хотите загрузить, выберете иконку из меню вкладки.");
            Dictionary.Add("app_AlwaysReadUpdatesMessage", "Разрешить прочтение истории для всех изменений.");
            Dictionary.Add("app_UpdateNotesHeader", "Информация о последнем обновлении");
            Dictionary.Add("app_ChangesOnRestartMessage", "Для применения изменений нужно перезапустить приложение.");
            Dictionary.Add("app_AvailablePluginsTabHeader", "Доступные Плагины");
            Dictionary.Add("app_PluginSourcesTabHeader", "Установка сторонних Плагинов");
            Dictionary.Add("app_SourceLabel", "Адрес для скачки плагина:");
            Dictionary.Add("app_EnabledHeader", "Включить");
            Dictionary.Add("app_VersionHeader", "Версия");
            Dictionary.Add("app_StatusHeader", "Статус");
            Dictionary.Add("app_FilesHeader", "Файлы");
            Dictionary.Add("app_SourceURIHeader", "Добавленные URI адреса плагинов");
            Dictionary.Add("app_AddUpdateSourceButtonText", "Добавить или обновить Источник");
            Dictionary.Add("app_RefreshPluginsButtonText", "Обновить информацию о Плагинах");
            Dictionary.Add("app_UnInstallButtonText", "Удалить");
            Dictionary.Add("app_InstallButtonText", "Установить");
            Dictionary.Add("app_AddOrUpdateSourceButtonText", "Добавить или обновить Источник");
            Dictionary.Add("app_NameHeader", "Имя");
            Dictionary.Add("app_UpdateToolTip", "Обновления");
            Dictionary.Add("app_pluginUpdateTitle", "Обновление Плагина!");
            Dictionary.Add("app_pluginUpdateMessageText", "Это означает, что для каких-то плагинов доступно обновление. Чтобы убедится в доступности обновлений откройте вкладку \"Обновление\".");
            Dictionary.Add("app_CurrentVersionHeader", "Текущая версия");
            Dictionary.Add("app_LatestVersionHeader", "Последняя версия");
            Dictionary.Add("app_UILanguageChangeWarningGeneral", "Вы хотите изменить язык игры, чтобы он совпдал с языком интерфейса? Если ответите 'Нет' то язык игры не изменится, изменить язык игры можно в настройках.");
            Dictionary.Add("app_UILanguageChangeWarningChinese", " При смене языка c/на Китайский требуется перезапуск приложения.");
            Dictionary.Add("app_UILanguageChangeWarningRussian", " При смене языка c/на Русский(ого) требуется перезапуск приложения, язык игры останется прежним, т.к. игра не переведена на русский язык.");
            Dictionary.Add("app_UILanguageChangeWarningNoGameLanguage", "Выбранный язык интерфейса не соответсвует языку игры. Пожалуйста, выберите язык игры в настроках программы.");
            Dictionary.Add("app_UIScaleHeader", "Размер Интерфейса");
            Dictionary.Add("app_HomePluginLabel", "Стартовый Плагин");
            Dictionary.Add("app_ProcessSelectedInfo", "*Используйте эту функцию только если перезапускаете игру или играете в 2 окна.");
            Dictionary.Add("app_PALSettingsTabHeader", "Эффективность и Записи");
            Dictionary.Add("app_DefNetInterfaceLabel", "Стандартный Сетевой Интерфейс (Чтение Пакетов)");
            Dictionary.Add("app_EnableNetReadingLabel", "Включение Чтения Сети");
            Dictionary.Add("app_BTNResNetWorker", "Сбросить Настройки Сетевого Рабочего");
            Dictionary.Add("app_DefAudioDeviceLabel", "Стандартное Устройство Звука");
            Dictionary.Add("app_MemScanSpeedLabel", "Скорочть Чтения Памяти Игры (Миллисекунды)");
            Dictionary.Add("app_ActorMSSLabel", "Субъект (NPCs и Остальные)");
            Dictionary.Add("app_ChatLogMSSLabel", "Запись Чата");
            Dictionary.Add("app_MonstersPCMSSLabel", "Монстры и Игроки");
            Dictionary.Add("app_PartyInfMSSLabel", "Информация о Группе");
            Dictionary.Add("app_PlayerInfMSSLabel", "Информация о персонаже (Вы)");
            Dictionary.Add("app_TargEnmMSSLabel", "Цели и Враждебность");
            Dictionary.Add("app_InvMSSLabel", "Инвентарь");
            return Dictionary;
        }
    }
}