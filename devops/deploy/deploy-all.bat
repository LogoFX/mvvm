rem TODO: Use common source for all version instances
SET version=2.2.5-rc1
rem TODO: Refactor using loop and automatic discovery
call deploy-single.bat LogoFX.Client.Mvvm.Core %version% 
call deploy-single.bat LogoFX.Client.Mvvm.Commanding.Core %version% 
call deploy-single.bat LogoFX.Client.Mvvm.Commanding %version% 
call deploy-single.bat LogoFX.Client.Mvvm.Model %version% 
call deploy-single.bat LogoFX.Client.Mvvm.ViewModel.Contracts %version%
call deploy-single.bat LogoFX.Client.Mvvm.ViewModel.Core %version%
call deploy-single.bat LogoFX.Client.Mvvm.ViewModel %version%
call deploy-single.bat LogoFX.Client.Mvvm.ViewModelFactory %version%
call deploy-single.bat LogoFX.Client.Mvvm.ViewModelFactory.SimpleContainer %version%
call deploy-single.bat LogoFX.Client.Mvvm.ViewModelFactory.Unity %version%
call deploy-single.bat LogoFX.Client.Mvvm.View.Core %version%
call deploy-single.bat LogoFX.Client.Mvvm.View %version%
call deploy-single.bat LogoFX.Client.Mvvm.ViewModel.Shared %version%
call deploy-single.bat LogoFX.Client.Mvvm.ViewModel.Services.Core %version%
call deploy-single.bat LogoFX.Client.Mvvm.ViewModel.Services %version%
call deploy-single.bat LogoFX.Client.Mvvm.ViewModel.Extensions.Core %version%
call deploy-single.bat LogoFX.Client.Mvvm.ViewModel.Extensions %version%
call deploy-single.bat LogoFX.Client.Mvvm.View.Extensions %version%