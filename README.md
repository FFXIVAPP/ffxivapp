# FFXIVAPP
Issue tracking and feature request repository.

## Recommended Format (Issues)
**Title**:
*brief description*

**Description**:
Detailed description including any log lines or screenshots to illustrate the issue.

## Recommended Format (Suggestions)
**Title**:
As a **<who>** (I can/want to) **<quick description>** (because/so I can) **<what>**.

**Description**:
This is where you can provide exmaples of what you mean, provide illustrations to support the request (if needed or helpful) and to say if you want; what you ideas of "definition of done" would be.

## Formatting
Formatting can be achieved via example from the follow URL: [Markdown](http://daringfireball.net/projects/markdown/)

## Open-Source Plugins

* http://github.com/icehunter/ffxivapp-plugin-event
* http://github.com/icehunter/ffxivapp-plugin-informer
* http://github.com/icehunter/ffxivapp-plugin-log

Directions on how-to install for FFXIVAPP are found on each repo. Thanks!

## Short Tutorial (Plugin Installation)
<a href="http://youtu.be/QEHYKjp4DjY" target="_blank">
    <img src="http://i1.ytimg.com/vi/QEHYKjp4DjY/mqdefault.jpg" />
</a>

## Directory Structure If Using Plugins

``` text
 Directory of \3.0.5074.41829

11/23/2013  05:34 AM    <DIR>          .
11/23/2013  05:34 AM    <DIR>          ..
11/22/2013  11:14 PM         1,224,704 FFXIVAPP.Client.exe
11/22/2013  06:55 PM            11,162 FFXIVAPP.Client.exe.config
10/18/2013  06:13 PM             1,023 FFXIVAPP.Client.exe.nlog
11/22/2013  10:23 PM            73,728 FFXIVAPP.Common.dll
11/22/2013  10:23 PM             6,144 FFXIVAPP.IPluginInterface.dll
11/22/2013  11:14 PM           899,072 FFXIVAPP.Updater.exe
11/23/2013  05:34 AM                 0 files.txt
11/09/2013  11:28 PM           134,656 HtmlAgilityPack.dll
11/23/2013  04:14 AM    <DIR>          Licenses
11/16/2013  09:53 PM           489,984 MahApps.Metro.dll
11/09/2013  11:28 PM           465,408 Newtonsoft.Json.dll
11/09/2013  11:28 PM           439,808 NLog.dll
11/23/2013  05:30 AM    <DIR>          Plugins
08/28/2013  08:13 AM            55,904 System.Windows.Interactivity.dll
              12 File(s)      3,801,593 bytes

 Directory of \3.0.5074.41829\Licenses

11/23/2013  04:14 AM    <DIR>          .
11/23/2013  04:14 AM    <DIR>          ..
10/18/2013  06:13 PM             2,642 License.DotNetZip.txt
10/18/2013  06:13 PM                57 License.FFXIVAPP.txt
10/18/2013  06:13 PM             2,642 License.HtmlAgilityPack.txt
10/18/2013  06:13 PM             2,642 License.MahApps.Metro.txt
10/18/2013  06:13 PM             1,548 License.NLog.txt
               5 File(s)          9,531 bytes

 Directory of \3.0.5074.41829\Plugins

11/23/2013  05:30 AM    <DIR>          .
11/23/2013  05:30 AM    <DIR>          ..
11/23/2013  04:21 AM    <DIR>          FFXIVAPP.Plugin.Event
11/23/2013  04:22 AM    <DIR>          FFXIVAPP.Plugin.Informer
11/23/2013  04:21 AM    <DIR>          FFXIVAPP.Plugin.Log
               0 File(s)              0 bytes

 Directory of \3.0.5074.41829\Plugins\FFXIVAPP.Plugin.Event

11/23/2013  04:21 AM    <DIR>          .
11/23/2013  04:21 AM    <DIR>          ..
11/22/2013  07:26 PM            14,314 aruba.wav
11/22/2013  10:23 PM            73,728 FFXIVAPP.Common.dll
11/22/2013  10:23 PM             6,144 FFXIVAPP.IPluginInterface.dll
11/22/2013  11:06 PM            69,632 FFXIVAPP.Plugin.Event.dll
11/22/2013  11:06 PM           134,656 FFXIVAPP.Plugin.Event.pdb
11/22/2013  10:22 PM           134,656 HtmlAgilityPack.dll
11/22/2013  10:22 PM           271,872 HtmlAgilityPack.pdb
11/22/2013  10:22 PM           122,991 HtmlAgilityPack.xml
11/22/2013  07:02 PM             3,706 Logo.png
11/16/2013  09:53 PM           489,984 MahApps.Metro.dll
11/22/2013  10:22 PM           465,408 Newtonsoft.Json.dll
11/22/2013  10:22 PM           469,230 Newtonsoft.Json.xml
11/22/2013  06:43 PM               692 NLog.config
11/22/2013  10:22 PM           439,808 NLog.dll
11/22/2013  10:22 PM           767,140 NLog.xml
11/22/2013  06:43 PM               169 PluginInfo.xml
08/28/2013  08:13 AM            55,904 System.Windows.Interactivity.dll
              17 File(s)      3,520,034 bytes

 Directory of \3.0.5074.41829\Plugins\FFXIVAPP.Plugin.Informer

11/23/2013  04:22 AM    <DIR>          .
11/23/2013  04:22 AM    <DIR>          ..
11/22/2013  10:23 PM            73,728 FFXIVAPP.Common.dll
11/22/2013  10:23 PM             6,144 FFXIVAPP.IPluginInterface.dll
11/22/2013  11:07 PM            62,976 FFXIVAPP.Plugin.Informer.dll
11/22/2013  11:07 PM           126,464 FFXIVAPP.Plugin.Informer.pdb
11/22/2013  10:22 PM           134,656 HtmlAgilityPack.dll
11/22/2013  10:22 PM           271,872 HtmlAgilityPack.pdb
11/22/2013  10:22 PM           122,991 HtmlAgilityPack.xml
11/22/2013  07:02 PM             2,776 Logo.png
11/16/2013  09:53 PM           489,984 MahApps.Metro.dll
11/22/2013  10:22 PM           465,408 Newtonsoft.Json.dll
11/22/2013  10:22 PM           469,230 Newtonsoft.Json.xml
11/22/2013  06:45 PM               692 NLog.config
11/22/2013  10:22 PM           439,808 NLog.dll
11/22/2013  10:22 PM           767,140 NLog.xml
11/22/2013  06:45 PM               172 PluginInfo.xml
08/28/2013  08:13 AM            55,904 System.Windows.Interactivity.dll
              16 File(s)      3,489,945 bytes

 Directory of \3.0.5074.41829\Plugins\FFXIVAPP.Plugin.Log

11/23/2013  04:21 AM    <DIR>          .
11/23/2013  04:21 AM    <DIR>          ..
11/22/2013  10:23 PM            73,728 FFXIVAPP.Common.dll
11/22/2013  10:23 PM             6,144 FFXIVAPP.IPluginInterface.dll
11/22/2013  11:08 PM            88,576 FFXIVAPP.Plugin.Log.dll
11/22/2013  11:08 PM           153,088 FFXIVAPP.Plugin.Log.pdb
11/22/2013  10:22 PM           134,656 HtmlAgilityPack.dll
11/22/2013  10:22 PM           271,872 HtmlAgilityPack.pdb
11/22/2013  10:22 PM           122,991 HtmlAgilityPack.xml
11/22/2013  07:03 PM             3,811 Logo.png
11/16/2013  09:53 PM           489,984 MahApps.Metro.dll
11/22/2013  10:22 PM           465,408 Newtonsoft.Json.dll
11/22/2013  10:22 PM           469,230 Newtonsoft.Json.xml
11/22/2013  06:45 PM               692 NLog.config
11/22/2013  10:22 PM           439,808 NLog.dll
11/22/2013  10:22 PM           767,140 NLog.xml
11/22/2013  06:45 PM               167 PluginInfo.xml
08/28/2013  08:13 AM            55,904 System.Windows.Interactivity.dll
              16 File(s)      3,543,199 bytes

     Total Files Listed:
              66 File(s)     14,364,302 bytes
              17 Dir(s)  34,101,559,296 bytes free
```
