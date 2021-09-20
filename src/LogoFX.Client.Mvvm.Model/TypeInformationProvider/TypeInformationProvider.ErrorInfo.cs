using System;
using System.Linq;

namespace LogoFX.Client.Mvvm.Model
{
    internal partial class TypeInformationProvider
    {        
        private static void AddErrorInfoDictionaryInternal<TErrorInfo>(Type type,
            IErrorInfoManager errorInfoManager)
        {
            var props = type.GetDeclaredTypeInfoProperties();
            var dataErrorInfoDictionary =
                props.Where(t => t.PropertyType.GetInterfaces().Contains(typeof(TErrorInfo)))
                    .ToDictionary(t => t.Name, t => t);
            errorInfoManager.Add(type, dataErrorInfoDictionary);
        }
    }
}
