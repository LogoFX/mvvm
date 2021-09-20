using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using FluentAssertions;
using Xunit;

namespace LogoFX.Client.Mvvm.ViewModel.Tests.WrappingCollectionTests
{
    public class ConcurrencyTests
    {
        static ConcurrencyTests()
        {
            Dispatch.Current = new DefaultDispatch();
        }

        [Fact]
        public void Add_ModelIsAddedAndThenCollectionIsClearedAndThenModelIsReAdded_ExceptionIsNotThrown()
        {
            var firstModel = new TestModel(1);
            var middleModel = new TestModel(2);
            var lastModel = new TestModel(3);
            var dataSource =
                new ObservableCollection<TestModel>(new[] {firstModel, middleModel});

            var wc = new WrappingCollection(r => r.UseConcurrent()).WithSource(dataSource);
            var exceptions = new List<Exception>();
            for (int i = 0; i < 1000; i++)
            {                
                var exception = Record.Exception(() =>
                {
                    dataSource.Add(lastModel);
                    dataSource.Clear();
                    dataSource.Add(lastModel);
                    dataSource.Clear();
                    dataSource.Add(firstModel);
                    dataSource.Add(middleModel);
                });
                if (exception != null)
                {
                    exceptions.Add(exception);
                }                
            }

            exceptions.Should().BeEmpty();
        }
    }
}
