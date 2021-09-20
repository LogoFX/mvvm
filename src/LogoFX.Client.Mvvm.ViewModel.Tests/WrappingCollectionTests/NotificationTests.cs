using System.Collections.Specialized;
using FluentAssertions;
using LogoFX.Core;
using Xunit;

namespace LogoFX.Client.Mvvm.ViewModel.Tests.WrappingCollectionTests
{    
    public class NotificationTests : WrappingCollectionTestsBase
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void
            WhenCollectionIsCreatedWithRangeAndSourceIsUpdatedWithAddRange_ThenSingleNotificationIsRaisedWithAllWrappers
            (bool isConcurrent)
        {
            var source = new RangeObservableCollection<object>();
            var items = new[] { new object(), new object() };
            var numberOfTimes = 0;

            var collection = new WrappingCollection(true, isConcurrent)
            {
                FactoryMethod = o => o
            };
            collection.AddSource(source);
            collection.CollectionChanged += (sender, args) =>
            {
                args.NewItems.Should().BeEquivalentTo(items);                
                numberOfTimes++;
                numberOfTimes.Should().Be(1);                
            };
            source.AddRange(items);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void
            WhenCollectionIsCreatedWithRangeAndSingleItemAndSourceIsUpdatedWithRemoveRange_ThenSingleNotificationIsRaisedWithAllWrappers
            (bool isConcurrent)
        {
            var source = new RangeObservableCollection<object>();
            var items = new[] { new object() };
            var numberOfTimes = 0;

            var collection = new WrappingCollection(true, isConcurrent)
            {
                FactoryMethod = o => o
            };
            collection.AddSource(source);            
            source.AddRange(items);
            collection.CollectionChanged += (sender, args) =>
            {
                args.OldItems.Should().BeEquivalentTo(items);
                numberOfTimes++;
                numberOfTimes.Should().Be(1);
            };
            source.RemoveRange(items);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void
            WhenCollectionIsCreatedWithRangeAndMultipleItemsAndSourceIsUpdatedWithRemoveRange_ThenSingleNotificationIsRaisedWithAllWrappersAndActionIsReset
            (bool isConcurrent)
        {
            var source = new RangeObservableCollection<object>();
            var items = new[] { new object(), new object(), new object() };
            var numberOfTimes = 0;

            var collection = new WrappingCollection(true, isConcurrent)
            {
                FactoryMethod = o => o
            };
            collection.AddSource(source);
            source.AddRange(items);
            collection.CollectionChanged += (sender, args) =>
            {
                args.Action.Should().Be(NotifyCollectionChangedAction.Reset);
                numberOfTimes++;
                numberOfTimes.Should().Be(1);
            };
            source.RemoveRange(items);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void
            WhenCollectionIsCreatedWithRangeAndSingleItemAndSourceIsCleared_ThenSingleNotificationIsRaisedWithAllWrappers
            (bool isConcurrent)
        {
            var source = new RangeObservableCollection<object>();
            var items = new[] { new object() };
            var numberOfTimes = 0;

            var collection = new WrappingCollection(true, isConcurrent)
            {
                FactoryMethod = o => o
            };
            collection.AddSource(source);            
            source.AddRange(items);            
            collection.CollectionChanged += (sender, args) =>
            {
                args.OldItems.Should().BeEquivalentTo(items);
                numberOfTimes++;
                numberOfTimes.Should().Be(1);
            };
            source.Clear();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void
            WhenCollectionIsCreatedWithRangeAndMultipleItemsAndSourceIsCleared_ThenSingleNotificationIsRaisedWithAllWrappersAndActionisReset
            (bool isConcurrent)
        {
            var source = new RangeObservableCollection<object>();
            var items = new[] { new object(), new object(), new object() };
            var numberOfTimes = 0;

            var collection = new WrappingCollection(true, isConcurrent)
            {
                FactoryMethod = o => o
            };
            collection.AddSource(source);
            source.AddRange(items);
            collection.CollectionChanged += (sender, args) =>
            {
                args.Action.Should().Be(NotifyCollectionChangedAction.Reset);
                numberOfTimes++;
                numberOfTimes.Should().Be(1);
            };
            source.Clear();
        }
    }
}
