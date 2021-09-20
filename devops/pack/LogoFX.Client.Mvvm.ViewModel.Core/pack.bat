cd contents
rmdir /Q /S lib
mkdir lib
cd lib
mkdir netstandard2.0\
robocopy ../../../../../src/Bin/netstandard/Release netstandard2.0 LogoFX.Client.Mvvm.ViewModel.deps.json /E
robocopy ../../../../../src/Bin/netstandard/Release netstandard2.0 LogoFX.Client.Mvvm.ViewModel.dll /E
robocopy ../../../../../src/Bin/netstandard/Release netstandard2.0 LogoFX.Client.Mvvm.ViewModel.xml /E
cd ../../
nuget pack contents/LogoFX.Client.Mvvm.ViewModel.Core.nuspec -OutputDirectory ../../../output