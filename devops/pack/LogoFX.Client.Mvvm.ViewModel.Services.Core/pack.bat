cd contents
rmdir /Q /S lib
mkdir lib
cd lib
mkdir netstandard2.0\
robocopy ../../../../../src/Bin/netstandard/Release netstandard2.0 LogoFX.Client.Mvvm.ViewModel.Services.* /E
cd ../../
nuget pack contents/LogoFX.Client.Mvvm.ViewModel.Services.Core.nuspec -OutputDirectory ../../../output