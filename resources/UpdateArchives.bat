cd "..\distribution"
del Release.zip /q
"C:\Program Files\7-Zip\7z.exe" a Release.zip FFXIVAPP.Client.exe
"C:\Program Files\7-Zip\7z.exe" a Release.zip FFXIVAPP.Client.exe.config
"C:\Program Files\7-Zip\7z.exe" a Release.zip FFXIVAPP.Client.exe.nlog
"C:\Program Files\7-Zip\7z.exe" a Release.zip FFXIVAPP.Updater.exe
"C:\Program Files\7-Zip\7z.exe" a Release.zip Licenses\*.txt