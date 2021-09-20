call test-specs-single LogoFX.Client.Mvvm.Model.Specs
call test-tests-single LogoFX.Client.Mvvm.ViewModel.Tests
call test-tests-single LogoFX.Client.Mvvm.ViewModelFactory.SimpleContainer.Tests
call test-tests-single LogoFX.Client.Mvvm.ViewModel.Extensions.Tests
call test-specs-single LogoFX.Client.Mvvm.ViewModel.Extensions.Specs
rem provide more generic way for non-global packages case
%UserProfile%/.nuget/packages/xunit.runner.console/2.4.1/tools/net461/xunit.console.exe ../../src/LogoFX.Client.Mvvm.View.Platform.Tests/bin/Release/LogoFX.Client.Mvvm.View.Platform.Tests.dll