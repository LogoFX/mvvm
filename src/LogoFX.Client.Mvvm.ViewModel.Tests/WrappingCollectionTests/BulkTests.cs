using System.Linq;
using FluentAssertions;
using LogoFX.Client.Mvvm.Model;
using LogoFX.Client.Mvvm.Model.Contracts;
using Xunit;

namespace LogoFX.Client.Mvvm.ViewModel.Tests.WrappingCollectionTests
{    
    public class BulkTests : WrappingCollectionTestsBase
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GivenCollectionIsBulkAndSourceWithTwoItemsIsAdded_WhenSecondItemIsRemoved_ThenCollectionContainsOneItem(bool isConcurrent)
        {
            var source = new RangeModelsCollection<TestModel>();
            var modelOne = new TestModel(4);
            var modelTwo = new TestModel(5);            

            var wrappingCollection =
                new WrappingCollection.WithSelection(SelectionMode.ZeroOrOne, isBulk: true, isConcurrent: isConcurrent)
                {
                    FactoryMethod = o => o
                }.WithSource(((IReadModelsCollection<TestModel>) source).Items);
            source.AddRange(new[] { modelOne, modelTwo });
            source.Remove(modelTwo);

            var expectedModels = new[] {modelOne};
            wrappingCollection.OfType<TestModel>().ToArray().Should().BeEquivalentTo(expectedModels);            
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GivenCollectionIsBulkAndSourceWithTwoItemsIsAdded_WhenAllItemsAreRemoved_ThenCollectionContainsNoItem(bool isConcurrent)
        {
            var source = new RangeModelsCollection<TestModel>();
            var modelOne = new TestModel(4);
            var modelTwo = new TestModel(5);            

            var wrappingCollection =
                new WrappingCollection.WithSelection(SelectionMode.ZeroOrOne, isBulk: true, isConcurrent: isConcurrent)
                {
                    FactoryMethod = o => o
                }.WithSource(((IReadModelsCollection<TestModel>) source).Items);
            source.AddRange(new[] { modelOne, modelTwo });
            source.RemoveRange(new[] { modelOne, modelTwo });

            wrappingCollection.OfType<TestModel>().ToArray().Should().BeEmpty();
        }
    }
}
