using System.Collections.ObjectModel;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace LogoFX.Client.Mvvm.ViewModel.Tests.WrappingCollectionTests
{    
    public class DataSourceModificationsTests : WrappingCollectionTestsBase
    {
        [Theory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void ModelIsAddedThenModelIsRemovedThenModelIsAdded(bool isBulk, bool isConcurrent)
        {
            var firstModel = new TestModel(1);
            var dataSource =
                new ObservableCollection<TestModel>(new[] { firstModel });

            var wrappingCollection = new WrappingCollection(isBulk, isConcurrent) { FactoryMethod = o => new TestViewModel((TestModel)o) };
            wrappingCollection.AddSource(dataSource);
            dataSource.Remove(firstModel);
            dataSource.Add(firstModel);

            var viewModels = wrappingCollection.OfType<TestViewModel>();
            var actualViewModel = viewModels.SingleOrDefault(t => t.Model.Id == 1);
            actualViewModel.Should().NotBeNull();            
        }
    }
}
