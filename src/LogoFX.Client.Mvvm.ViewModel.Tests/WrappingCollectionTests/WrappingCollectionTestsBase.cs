using System.Threading;
using LogoFX.Client.Testing.Infra;

namespace LogoFX.Client.Mvvm.ViewModel.Tests.WrappingCollectionTests
{
    public abstract class WrappingCollectionTestsBase
    {
        protected WrappingCollectionTestsBase()
        {            
            Dispatch.Current = new SameThreadDispatch();
        }                        
    }
}