using Attest.Testing.Lifecycle;
using JetBrains.Annotations;
using LogoFX.Client.Testing.Shared.Caliburn.Micro;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions.Specs.Infra
{
    [UsedImplicitly]
    internal sealed class TeardownService : ITeardownService
    {
        public void Teardown()
        {
            TestHelper.Teardown();
        }
    }
}
