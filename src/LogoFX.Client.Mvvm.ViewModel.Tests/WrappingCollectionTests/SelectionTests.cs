using System;
using System.Collections.ObjectModel;
using System.Linq;
using FluentAssertions;
using LogoFX.Core;
using Xunit;

namespace LogoFX.Client.Mvvm.ViewModel.Tests.WrappingCollectionTests
{        
    public class SelectionTests : WrappingCollectionTestsBase
    {
        [Theory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void Selection_ItemIsSelectedAndDeselected_SelectionIsEmpty(bool isBulk, bool isConcurrent)
        {
            var originalDataSource =
                new ObservableCollection<TestModel>(new[] {new TestModel(1), new TestModel(2), new TestModel(3)});

            var wrappingCollection =
                new WrappingCollection.WithSelection(SelectionMode.ZeroOrMore, isBulk, isConcurrent)
                    {FactoryMethod = o => new TestViewModel((TestModel) o)}.WithSource(
                    originalDataSource);
            var firstItem = wrappingCollection.OfType<TestViewModel>().First();
            wrappingCollection.Select(firstItem);
            wrappingCollection.Unselect(firstItem);

            AssertEmptySelection(wrappingCollection);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void Selection_SelectionModeIsMultipleItemIsSelectedAndAnotherItemIsSelected_BothItemsAreSelected(
            bool isBulk, bool isConcurrent)
        {
            var originalDataSource =
                new ObservableCollection<TestModel>(new[] {new TestModel(1), new TestModel(2), new TestModel(3)});

            var wrappingCollection =
                new WrappingCollection.WithSelection(SelectionMode.ZeroOrMore, isBulk, isConcurrent)
                {
                    FactoryMethod = o => new TestViewModel((TestModel) o)
                }.WithSource(originalDataSource);
            var firstItem = wrappingCollection.OfType<TestViewModel>().First();
            var secondItem = wrappingCollection.OfType<TestViewModel>().Skip(1).First();
            wrappingCollection.Select(firstItem);
            wrappingCollection.Select(secondItem);

            wrappingCollection.SelectedItem.Should().Be(firstItem);
            var expectedSelection = new[] {firstItem, secondItem};
            wrappingCollection.SelectedItems.Should().BeEquivalentTo(expectedSelection);
            wrappingCollection.SelectionCount.Should().Be(2);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void Selection_SelectionModeIsSingleItemIsSelectedAndAnotherItemIsSelected_OnlySecondItemIsSelected(
            bool isBulk, bool isConcurrent)
        {
            var originalDataSource =
                new ObservableCollection<TestModel>(new[] {new TestModel(1), new TestModel(2), new TestModel(3)});

            var wrappingCollection = new WrappingCollection.WithSelection(SelectionMode.One, isBulk, isConcurrent)
                {FactoryMethod = o => new TestViewModel((TestModel) o)};
            wrappingCollection.AddSource(originalDataSource);
            var firstItem = wrappingCollection.OfType<TestViewModel>().First();
            var secondItem = wrappingCollection.OfType<TestViewModel>().Skip(1).First();
            wrappingCollection.Select(firstItem);
            wrappingCollection.Select(secondItem);

            wrappingCollection.SelectedItem.Should().Be(secondItem);
            var expectedSelection = new[] {secondItem};
            wrappingCollection.SelectedItems.Should().BeEquivalentTo(expectedSelection);
            wrappingCollection.SelectionCount.Should().Be(1);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void Selection_ItemIsSelectedThenItemIsRemoved_SelectionIsEmpty(bool isBulk, bool isConcurrent)
        {
            var originalDataSource =
                new ObservableCollection<TestModel>(new[] {new TestModel(1), new TestModel(2), new TestModel(3)});

            var wrappingCollection =
                new WrappingCollection.WithSelection(SelectionMode.ZeroOrMore, isBulk, isConcurrent)
                {
                    FactoryMethod = o => new TestViewModel((TestModel) o)
                }.WithSource(originalDataSource);
            var firstItem = wrappingCollection.OfType<TestViewModel>().First();
            wrappingCollection.Select(firstItem);
            originalDataSource.RemoveAt(0);

            AssertEmptySelection(wrappingCollection);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void Selection_ItemIsSelectedThenAllItemsAreRemoved_SelectionIsEmpty(bool isBulk, bool isConcurrent)
        {
            var originalDataSource =
                new RangeObservableCollection<TestModel>(new[] {new TestModel(1), new TestModel(2), new TestModel(3)});

            var wrappingCollection =
                new WrappingCollection.WithSelection(SelectionMode.ZeroOrMore, isBulk, isConcurrent)
                {
                    FactoryMethod = o => new TestViewModel((TestModel) o)
                }.WithSource(originalDataSource);
            var firstItem = wrappingCollection.OfType<TestViewModel>().First();
            wrappingCollection.Select(firstItem);
            originalDataSource.RemoveRange(originalDataSource.ToArray());

            AssertEmptySelection(wrappingCollection);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void ClearSelection_CollectionContainsTwoSelectedItems_SelectionIsEmpty(bool isBulk, bool isConcurrent)
        {
            var originalDataSource =
                new ObservableCollection<TestModel>(new[] {new TestModel(1), new TestModel(2), new TestModel(3)});

            var wrappingCollection =
                new WrappingCollection.WithSelection(SelectionMode.ZeroOrMore, isBulk, isConcurrent)
                {
                    FactoryMethod = o => new TestViewModel((TestModel) o)
                }.WithSource(originalDataSource);
            var firstItem = wrappingCollection.OfType<TestViewModel>().First();
            wrappingCollection.Select(firstItem);
            var secondItem = wrappingCollection.OfType<TestViewModel>().Skip(1).First();
            wrappingCollection.Select(secondItem);
            wrappingCollection.ClearSelection();

            AssertEmptySelection(wrappingCollection);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void CollectionIsCreated_SelectionPredicateIsSetAndOriginalSourceContainsItemsThatMatchThePredicate_ItemsAreSelected(bool isBulk, bool isConcurrent)
        {
            var originalDataSource =
                new ObservableCollection<TestModel>(new[] { new TestModel(1), new TestModel(2), new TestModel(3)});

            var wrappingCollection =
                new WrappingCollection.WithSelection(wr => ((TestViewModel) wr).Model.Id >= 2, isBulk, isConcurrent)
                {
                    FactoryMethod = o => new TestViewModel((TestModel) o)
                }.WithSource(originalDataSource);
            var secondItem = wrappingCollection.OfType<TestViewModel>().ElementAt(1);
            var lastItem = wrappingCollection.OfType<TestViewModel>().Last();
            wrappingCollection.SelectedItem.Should().Be(secondItem);
            var expectedSelection = new[] { lastItem, secondItem };
            wrappingCollection.SelectedItems.Should().BeEquivalentTo(expectedSelection);
            wrappingCollection.SelectionCount.Should().Be(2);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void Select_SelectionPredicateIsSet_ExceptionIsThrown(bool isBulk, bool isConcurrent)
        {
            var originalDataSource =
                new ObservableCollection<TestModel>(new[] {new TestModel(1), new TestModel(2), new TestModel(3)});

            var wrappingCollection =
                new WrappingCollection.WithSelection(wr => ((TestViewModel) wr).Model.Id >= 2, isBulk, isConcurrent)
                {
                    FactoryMethod = o => new TestViewModel((TestModel) o)
                }.WithSource(originalDataSource);
            var exception = Record.Exception(() => wrappingCollection.Select(wrappingCollection.SelectedItem));

            exception.Should().BeOfType<InvalidOperationException>().Which.Message.Should()
                .Be("Explicit selection status change cannot be used together with selection predicate");
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void Unselect_SelectionPredicateIsSet_ExceptionIsThrown(bool isBulk, bool isConcurrent)
        {
            var originalDataSource =
                new ObservableCollection<TestModel>(new[] { new TestModel(1), new TestModel(2), new TestModel(3) });

            var wrappingCollection =
                new WrappingCollection.WithSelection(wr => ((TestViewModel) wr).Model.Id >= 2, isBulk, isConcurrent)
                {
                    FactoryMethod = o => new TestViewModel((TestModel) o)
                }.WithSource(originalDataSource);
            var exception = Record.Exception(() => wrappingCollection.Unselect(wrappingCollection.SelectedItem));

            exception.Should().BeOfType<InvalidOperationException>().Which.Message.Should()
                .Be("Explicit selection status change cannot be used together with selection predicate");
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void
            ModelPropertyIsChanged_SelectionPredicateIsSetAndItemDoesntMatchPredicateAfterModelChange_ItemIsUnselected(
                bool isBulk, bool isConcurrent)
        {
            var originalDataSource =
                new ObservableCollection<TestModel>(new[]
                {
                    new TestModel(1) {Name = "First"},
                    new TestModel(2) {Name = "Second"},
                    new TestModel(3) {Name = "Third"}
                });

            var wrappingCollection =
                new WrappingCollection.WithSelection(wr => ((TestViewModel) wr).Model.Name.Length <= 5, isBulk,
                    isConcurrent)
                {
                    FactoryMethod = o => new TestViewModel((TestModel) o)
                }.WithSource(originalDataSource);
            var firstItem = wrappingCollection.OfType<TestViewModel>().ElementAt(0);
            firstItem.Model.Name = "FirstOne";

            var lastItem = wrappingCollection.OfType<TestViewModel>().Last();
            wrappingCollection.SelectedItem.Should().Be(lastItem);
            var expectedSelection = new[] {lastItem};
            wrappingCollection.SelectedItems.Should().BeEquivalentTo(expectedSelection);
            wrappingCollection.SelectionCount.Should().Be(1);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void
            SelectionPredicateIsSet_SelectionPredicateWasSetInitiallyAndItemDoesnMatchTheNewPredicate_SelectionChanges(
                bool isBulk, bool isConcurrent)
        {
            var originalDataSource =
                new ObservableCollection<TestModel>(new[]
                {
                    new TestModel(1) {Name = "First"},
                    new TestModel(2) {Name = "Second"},
                    new TestModel(3) {Name = "Third"}
                });

            var wrappingCollection =
                new WrappingCollection.WithSelection(wr => ((TestViewModel) wr).Model.Name.Length <= 5, isBulk,
                    isConcurrent)
                {
                    FactoryMethod = o => new TestViewModel((TestModel) o)
                }.WithSource(originalDataSource);
            wrappingCollection.SelectionPredicate = wr => ((TestViewModel) wr).Model.Name.Length == 6;

            var secondItem = wrappingCollection.OfType<TestViewModel>().ElementAt(1);
            wrappingCollection.SelectedItem.Should().Be(secondItem);
            var expectedSelection = new[] {secondItem};
            wrappingCollection.SelectedItems.Should().BeEquivalentTo(expectedSelection);
            wrappingCollection.SelectionCount.Should().Be(1);
        }

        private static void AssertEmptySelection(WrappingCollection.WithSelection wrappingCollection)
        {
            wrappingCollection.SelectedItem.Should().BeNull();
            wrappingCollection.SelectedItems.Cast<object>().Should().BeEmpty();
            wrappingCollection.SelectionCount.Should().Be(0);            
        }
    }
}
