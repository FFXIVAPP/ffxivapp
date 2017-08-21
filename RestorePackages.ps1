$nuget = $args[0]

Set-Location $PSScriptRoot\packages

Dir |
Where-Object { $_.Name -match "^(Sharlayan|Machina|FFXIVAPP)" } | 
Remove-Item -Recurse

Set-Location $PSScriptRoot

iex "$nuget restore"