param($rootPath, $toolsPath, $package, $project)

$configFileFrom = $toolsPath + "\..\src\dotnet.watchr.rb"
$configFileTo = "dotnet.watchr.rb"

$watchrFileFrom = $toolsPath + "\..\src\watcher_dot_net.rb"
$watchrFileTo = "watcher_dot_net.rb"

$redFileFrom = $toolsPath + "\..\src\red.png"
$redFileTo = "red.png"

$greenFileFrom = $toolsPath + "\..\src\green.png"
$greenFileTo = "green.png"

if(!(Test-Path $configFileTo))
{
  Copy-Item $configFileFrom $configFileTo
  Copy-Item $watchrFileFrom $watchrFileTo
  Copy-Item $redFileFrom $redFileTo
  Copy-Item $greenFileFrom $greenFileTo
}

