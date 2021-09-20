using FluentAssertions;
using LogoFX.Client.Mvvm.ViewModel;
using LogoFX.Practices.IoC;
using Xunit;

namespace LogoFX.Client.Mvvm.ViewModelFactory.SimpleContainer.Tests
{    
    public class ViewModelFactoryTests
    {
        [Fact]
        public void
            GivenDependencyHasOneParameterNamedModelAndDependencyIsRegisteredPerRequest_WhenModelWrapperIsCreated_ThenModelWrapperIsNotNull
            ()
        {
            const string model = "6";           
            var container = new ExtendedSimpleContainer();
            container.RegisterPerRequest(typeof (ObjectViewModel<string>), null, typeof (ObjectViewModel<string>));

            var viewModelFactory = new ViewModelFactory(container);
            var modelWrapper = viewModelFactory.CreateModelWrapper<string, ObjectViewModel<string>>(model);

            modelWrapper.Model.Should().Be(model);            
        }
    }
}
