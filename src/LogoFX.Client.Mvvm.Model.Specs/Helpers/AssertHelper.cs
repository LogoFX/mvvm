using System.ComponentModel;
using System.Linq;
using FluentAssertions;

namespace LogoFX.Client.Mvvm.Model.Specs.Helpers
{
    internal static class AssertHelper
    {
        internal static void AssertModelHasErrorIsFalse<T>(T model) where T : INotifyDataErrorInfo, IDataErrorInfo
        {
            var hasErrors = model.HasErrors;
            var collectionOfErrorsIsEmpty = model.GetErrors(null).OfType<string>().Any() == false;
            model.Error.Should().BeNullOrEmpty();
            hasErrors.Should().BeFalse();
            collectionOfErrorsIsEmpty.Should().BeTrue();
        }

        internal static void AssertModelHasErrorIsTrue<T>(T model) where T : INotifyDataErrorInfo, IDataErrorInfo
        {
            var hasErrors = model.HasErrors;
            var collectionOfErrorsIsEmpty = model.GetErrors(null).OfType<string>().Any() == false;
            model.Error.Should().NotBeNullOrEmpty();
            hasErrors.Should().BeTrue();
            collectionOfErrorsIsEmpty.Should().BeFalse();
        }
    }
}