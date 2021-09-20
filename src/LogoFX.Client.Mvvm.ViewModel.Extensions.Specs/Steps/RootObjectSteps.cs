using Attest.Testing.Lifecycle;
using Attest.Testing.Modularity;
using JetBrains.Annotations;
using Solid.Practices.IoC;
using TechTalk.SpecFlow;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions.Specs.Steps
{
    [Binding, UsedImplicitly]
    internal sealed class RootObjectSteps
    {
        private readonly IStartDynamicApplicationModuleService _startDynamicApplicationModuleService;
        private readonly IDependencyResolver _dependencyResolver;

        public RootObjectSteps(
            IStartDynamicApplicationModuleService startDynamicApplicationModuleService,
            IDependencyResolver dependencyResolver)
        {
            _startDynamicApplicationModuleService = startDynamicApplicationModuleService;
            _dependencyResolver = dependencyResolver;
        }

        [When(@"I open the application")]
        public void WhenIOpenTheApplication()
        {
            _startDynamicApplicationModuleService.StartCollection(_dependencyResolver
                .ResolveAll<IDynamicApplicationModule>());
        }
    }
}