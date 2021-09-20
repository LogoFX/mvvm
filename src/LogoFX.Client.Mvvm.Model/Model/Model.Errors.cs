using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using LogoFX.Core;

namespace LogoFX.Client.Mvvm.Model
{    
    partial class Model<T>
    {
        private readonly Dictionary<string, string> _externalErrors =
            new Dictionary<string, string>();

        [NonSerialized]
        private IErrorInfoExtractionStrategy _errorInfoExtractionStrategy;

        private void InitErrorListener()
        {
            var interfaces = Type.GetInterfaces().ToArray();
            //TODO: Add Chain-Of-Command
            if (interfaces.Contains(typeof(INotifyDataErrorInfo)))
            {
                _errorInfoExtractionStrategy = new NotifyDataErrorInfoExtractionStrategy();                
            }
            else if (interfaces.Contains(typeof(IDataErrorInfo)))
            {
                _errorInfoExtractionStrategy = new DataErrorInfoExtractionStrategy();                
            }
            var propertyNames = _errorInfoExtractionStrategy.GetPropertyInfoSources(Type);
            foreach (var propertyName in propertyNames)
            {
                SubscribeToInnerChange(propertyName);
            }
            ListenToPropertyChange();                        
        }

        private void ListenToPropertyChange()
        {
            PropertyChanged += WeakDelegate.From(OnPropertyChanged);
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var changedPropertyName = e.PropertyName;
            if (IsSpecialName(e.PropertyName))
            {
                return;
            }            

            if (_errorInfoExtractionStrategy.IsPropertyErrorInfoSource(Type, changedPropertyName))
            {             
                SubscribeToInnerChange(changedPropertyName);
            }
            NotifyOfPropertyChange(() => Error);
        }

        private bool IsSpecialName(string propertyName)
        {
            switch (propertyName)
            {
                case "IsDirty": case "CanCancelChanges":
                case "Error": case "HasErrors" :
                    return true;
                default:  return false;
            }
        }

        private void SubscribeToInnerChange(string propertyName)
        {
            var propertyValue = _errorInfoExtractionStrategy.GetErrorInfoSourceValue(Type, propertyName, this);
            if (propertyValue is INotifyPropertyChanged innerSource)
            {
                innerSource.PropertyChanged += WeakDelegate.From(InnerSourceOnPropertyChanged);
            }            
        }

        private void InnerSourceOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "Error")
            {
                NotifyOfPropertyChange(() => Error);
            }
        }

        /// <inheritdoc />       
        public virtual string this[string columnName]
        {
            get
            {
                var errors = GetErrorsByPropertyName(columnName);
                return CreateErrorsPresentation(errors);              
            }
        }

        private IEnumerable<string> GetErrorsByPropertyName(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
            {
                foreach (var error in _externalErrors.Values)
                {
                    yield return error;
                }
            }
            else
            {
                var externalError = _externalErrors.ContainsKey(columnName) ? _externalErrors[columnName] : string.Empty;
                if (string.IsNullOrEmpty(externalError) == false)
                {
                    yield return externalError;
                }    
            }
            
            var validationErrors = GetInternalValidationErrorsByPropertyName(columnName);
            if (validationErrors != null)
            {
                foreach (var validationError in validationErrors)
                {
                    yield return validationError;
                }
            }            
        }

        private IEnumerable<string> GetInternalValidationErrorsByPropertyName(string columnName)
        {
            return ErrorService.GetValidationErrorsByPropertyName(Type, columnName, this);
        }

        /// <inheritdoc />        
        public virtual string Error
        {
            get
            {
                var errors = GetAllErrors();
                return CreateErrorsPresentation(errors);                
            }
        }

        private IEnumerable<string> CalculateOwnErrors()
        {            
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var entry in TypeInformationProvider.GetValidationInfoCollection(Type))
            {
                var propErrors = GetErrorsByPropertyName(entry.Key);
                if (propErrors != null)
                {
                    foreach (var propError in propErrors)
                    {
                        yield return propError;
                    }
                }
            }            
        }

        /// <inheritdoc />        
        public IEnumerable GetErrors(string propertyName)
        {
            return string.IsNullOrEmpty(propertyName) ? GetAllErrors() : GetErrorsByPropertyName(propertyName);
        }

        private IEnumerable<string> GetAllErrors()
        {
            var ownErrors = CalculateOwnErrors();
            var childrenErrors = _errorInfoExtractionStrategy.ExtractChildrenErrors(Type, this);
            var errors = ownErrors?.Concat(childrenErrors) ?? childrenErrors;
            return errors;
        }
        
        /// <summary>
        /// Gets a value indicating whether this instance has errors.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has errors; otherwise, <c>false</c>.
        /// </value>
        public bool HasErrors => string.IsNullOrWhiteSpace(Error) == false;

        /// <summary>
        /// Fires ErrorsChanged event from the INotifyDataErrorInfo interface
        /// </summary>
        /// <param name="name"></param>
        protected void RaiseErrorsChanged([CallerMemberName] string name = "")
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(name));
        }

        /// <inheritdoc />       
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <inheritdoc />       
        public void SetError(string error, string propertyName)
        {
            if (_externalErrors.ContainsKey(propertyName))
            {
                if (string.IsNullOrEmpty(error))
                {
                    _externalErrors.Remove(propertyName);
                }
                else
                {
                    _externalErrors[propertyName] = error;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(error) == false)
                {
                    _externalErrors.Add(propertyName, error);
                }
            }
            NotifyOfPropertyChange(() => HasErrors);
        }

        /// <inheritdoc />        
        public void ClearError(string propertyName)
        {
            if (_externalErrors.ContainsKey(propertyName))
            {
                _externalErrors.Remove(propertyName);
            }
            NotifyOfPropertyChange(() => HasErrors);
        }

        /// <summary>
        /// Override this method to use custom errors collection presentation.
        /// </summary>
        /// <param name="errors">The collection of errors.</param>
        /// <returns></returns>
        protected virtual string CreateErrorsPresentation(IEnumerable<string> errors)
        {
            var errorsArray = errors?.ToArray();
            if (errorsArray == null || errorsArray.Length == 0)
            {
                return string.Empty;
            }
            var stringBuilder = new StringBuilder();
            foreach (var error in errorsArray)
            {
                AppendErrorIfNeeded(error, stringBuilder);
            }
            return stringBuilder.ToString().TrimEnd('\r', '\n');
        }

        private static void AppendErrorIfNeeded(string error, StringBuilder stringBuilder)
        {
            if (string.IsNullOrWhiteSpace(error) == false)
            {
                stringBuilder.AppendLine(error);
            }
        }
    }
}
