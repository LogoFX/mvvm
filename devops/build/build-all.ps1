Install-Module VSSetup -Scope CurrentUser -Force
Install-Module BuildUtils -Scope CurrentUser -Force

$msbuildLocation = Get-LatestMsbuildLocation
set-alias msb $msbuildLocation
msb "../../src/LogoFX.Client.Mvvm.sln" -property:Configuration=Release /t:Clean,Build