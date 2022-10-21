cd contents
rmdir /Q /S lib
mkdir lib
cd lib
mkdir net461\
robocopy ../../../../../src/Bin/netframework/Release net461 LogoFX.Client.Mvvm.Commanding.Platform.* /E
mkdir net6.0
robocopy ../../../../../src/Bin/net/Release net6.0 LogoFX.Client.Mvvm.Commanding.Platform.* /E
cd net6.0
rmdir /Q /S ref
cd ..
mkdir netcoreapp3.1
robocopy ../../../../../src/Bin/netcore/Release netcoreapp3.1 LogoFX.Client.Mvvm.Commanding.Platform.* /E
cd ../../
nuget pack contents/LogoFX.Client.Mvvm.Commanding.nuspec -OutputDirectory ../../../output