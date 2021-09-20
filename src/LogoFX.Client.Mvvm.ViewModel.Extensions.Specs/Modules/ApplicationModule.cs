using Attest.Testing.Lifecycle;
using Attest.Testing.Modularity;
using JetBrains.Annotations;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions.Specs.Modules
{
    [UsedImplicitly]
    class SystemUnderTestModule : IDynamicApplicationModule
    {
        private readonly IStartLocalApplicationService _startLocalApplicationService;

        public SystemUnderTestModule(
            IStartLocalApplicationService startLocalApplicationService)
        {
            _startLocalApplicationService = startLocalApplicationService;
        }

        public string Id => "SystemUnderTest";

        public string RelativePath => string.Empty;

        public void Start()
        {
            _startLocalApplicationService.Start();
        }

        public void Stop() {}
    }
}
