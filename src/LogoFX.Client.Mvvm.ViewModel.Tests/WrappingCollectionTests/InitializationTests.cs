using System.Linq;
using System.Reflection;
using FluentAssertions;
using Xunit;

namespace LogoFX.Client.Mvvm.ViewModel.Tests.WrappingCollectionTests
{    
    public class InitializationTests : WrappingCollectionTestsBase
    {
        [Theory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void AddingDataSource_DataSourceContainsModelsAndFactoryMethodIsSpecified_CollectionContainsConcreteTypeViewModelsWithDataSourceModels(bool isBulk, bool isConcurrent)
        {
            var dataSource = new[] {new TestModel(1), new TestModel(2), new TestModel(3)};

            var wrappingCollection = new WrappingCollection(isBulk, isConcurrent) {FactoryMethod = o => new TestViewModel((TestModel)o)};
            wrappingCollection.AddSource(dataSource);

            var viewModels = wrappingCollection.OfType<TestViewModel>().ToArray();
            var actualModels = viewModels.Select(t => t.Model).ToArray();
            actualModels.Should().BeEquivalentTo(dataSource);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void AddingDataSource_DataSourceContainsModelsAndFactoryMethodIsNotSpecified_CollectionContainsViewModelsWithDataSourceModels(bool isBulk, bool isConcurrent)
        {
            var dataSource = new[] { new TestModel(1), new TestModel(2), new TestModel(3) };

            var wrappingCollection = new WrappingCollection(isBulk, isConcurrent);
            wrappingCollection.AddSource(dataSource);

            var viewModels = wrappingCollection.OfType<object>().ToArray();
            var viewModelType = viewModels.First().GetType();
            var modelPropertyInfo = viewModelType.GetTypeInfo().GetDeclaredProperty("Model");
            var actualModels = viewModels.Select(modelPropertyInfo.GetValue).ToArray();
            actualModels.Should().BeEquivalentTo(dataSource);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void AddingDataSource_DataSourceContainsModelsAndSelectionModeIsOne_FirstViewModelIsSelected(bool isBulk, bool isConcurrent)
        {
            var dataSource = new[] { new TestModel(1), new TestModel(2), new TestModel(3) };

            var wrappingCollection = new WrappingCollection.WithSelection(SelectionMode.One, isBulk, isConcurrent) { FactoryMethod = o => new TestViewModel((TestModel)o) };
            wrappingCollection.AddSource(dataSource);

            var viewModels = wrappingCollection.OfType<TestViewModel>().ToArray();
            var firstViewModel = viewModels.First();
            var selectedViewModel = wrappingCollection.SelectedItem;
            selectedViewModel.Should().BeSameAs(firstViewModel);            
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void AddingDataSource_DataSourceContainsModelsAndSelectionModeIsZeroOrMore_NoViewModelIsSelected(bool isBulk, bool isConcurrent)
        {
            var dataSource = new[] { new TestModel(1), new TestModel(2), new TestModel(3) };

            var wrappingCollection = new WrappingCollection.WithSelection(SelectionMode.ZeroOrMore, isBulk, isConcurrent) { FactoryMethod = o => new TestViewModel((TestModel)o) };
            wrappingCollection.AddSource(dataSource);
            
            var selectedViewModel = wrappingCollection.SelectedItem;
            selectedViewModel.Should().BeNull();            
        }
    }
}
