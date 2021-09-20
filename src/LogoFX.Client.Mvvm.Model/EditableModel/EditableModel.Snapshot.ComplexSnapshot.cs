using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using LogoFX.Client.Core;
using LogoFX.Core;
using Solid.Patterns.ChainOfResponsibility;

namespace LogoFX.Client.Mvvm.Model
{
    public partial class EditableModel<T>
    {
        sealed class ComplexSnapshot : ISnapshot
        {
            private class InputData
            {
                public object Value { get; }
                public IDictionary<object, SnapshotValue> HashTable { get; }
                public bool IsInitOnly { get; }

                public InputData(
                    object value,
                    IDictionary<object, SnapshotValue> hashTable, 
                    bool isInitOnly
                    )
                {
                    Value = value;
                    HashTable = hashTable;
                    IsInitOnly = isInitOnly;
                }
            }

            private abstract class SnapshotValue
            {
                public static ClassSnapshotValue Create(object model)
                {
                    var hashTable = new Dictionary<object, SnapshotValue>();
                    var result = new ClassSnapshotValue(model, hashTable);
                    hashTable.Clear();
                    return result;
                }

                protected static SnapshotValue Create(object value, IDictionary<object, SnapshotValue> hashTable, bool isInitOnly)
                {
                    var chain = BuildChain();
                    return chain.Handle(new InputData(value, hashTable, isInitOnly));
                }

                private static IChainElement<InputData, SnapshotValue> BuildChain()
                {
                    var commander = new NullHandler();
                    commander
                        .SetSuccessor(new ExistingValueHandler())
                        .SetSuccessor(new DictionaryValueHandler())
                        .SetSuccessor(new ListValueHandler())
                        .SetSuccessor(new SimpleValueHandler())
                        .SetSuccessor(new DefaultValueHandler());

                    return commander;
                }

                private sealed class NullHandler : ChainElementBase<InputData, SnapshotValue>
                {
                    private static readonly NullSnapshotValue NullSnapshotValue =
                        new NullSnapshotValue();

                    protected override bool IsMine(InputData data)
                    {
                        return data.Value == null;
                    }

                    protected override SnapshotValue HandleData(InputData data)
                    {
                        return data.IsInitOnly ? null : NullSnapshotValue;
                    }
                }

                private sealed class ExistingValueHandler : ChainElementBase<InputData, SnapshotValue>
                {
                    protected override bool IsMine(InputData data)
                    {
                        return data.HashTable.ContainsKey(data.Value);
                    }

                    protected override SnapshotValue HandleData(InputData data)
                    {
                        return data.HashTable[data.Value];
                    }
                }

                private sealed class DictionaryValueHandler : ChainElementBase<InputData, SnapshotValue>
                {
                    protected override bool IsMine(InputData data)
                    {
                        return data.Value is IDictionary dictionary && !dictionary.IsReadOnly && !dictionary.IsFixedSize;
                    }

                    protected override SnapshotValue HandleData(InputData data)
                    {
                        return new DictionarySnapshotValue(data.Value as IDictionary, data.HashTable);
                    }
                }

                private sealed class ListValueHandler : ChainElementBase<InputData, SnapshotValue>
                {
                    protected override bool IsMine(InputData data)
                    {
                        return data.Value is IList list && !list.IsReadOnly && !list.IsFixedSize;
                    }

                    protected override SnapshotValue HandleData(InputData data)
                    {
                        return new ListSnapshotValue(data.Value as IList, data.HashTable);
                    }
                }

                private sealed class SimpleValueHandler : ChainElementBase<InputData, SnapshotValue>
                {
                    protected override bool IsMine(InputData data)
                    {
                        return IsSimpleType(data.Value) || data.Value.GetType().IsBclType();
                    }

                    protected override SnapshotValue HandleData(InputData data)
                    {
                        var type = data.Value.GetType();
                        bool isSimpleType = IsSimpleTypeImpl(data.Value);
                        if (!isSimpleType && type.IsBclType())
                        {
                            if (type.IsSerializable && !data.IsInitOnly)
                            {
                                return new SerializingSnapshotValue(data.Value, data.HashTable);
                            }

                            isSimpleType = true;
                        }

                        if (isSimpleType)
                        {
                            if (data.IsInitOnly)
                            {
                                return null;
                            }

                            return new SimpleSnapshotValue(data.Value);
                        }

                        return null;
                    }

                    private bool IsSimpleType(object value)
                    {
                        return IsSimpleTypeImpl(value);
                    }

                    private bool IsSimpleTypeImpl(object value)
                    {
                        var type = value.GetType();
                        var isSimpleType = value is ValueType || value is string || type.IsArray;
                        return isSimpleType;
                    }
                }

                private sealed class DefaultValueHandler : ChainElementBase<InputData, SnapshotValue>
                {
                    protected override bool IsMine(InputData data)
                    {
                        return true;
                    }

                    protected override SnapshotValue HandleData(InputData data)
                    {
                        return new ClassSnapshotValue(data.Value, data.HashTable);
                    }
                }

                protected abstract void RestorePropertiesOverride(object model, Dictionary<SnapshotValue, object> cache);

                public void RestoreProperties(object model)
                {
                    var cache = new Dictionary<SnapshotValue, object> {{this, model}};
                    RestoreProperties(model, cache);
                    cache.Clear();
                }

                internal void RestoreProperties(object model, Dictionary<SnapshotValue, object> cache)
                {
                    RestorePropertiesOverride(model, cache);
                }
                
                protected abstract object GetValueOverride(Dictionary<SnapshotValue, object> cache);

                internal object GetValue(Dictionary<SnapshotValue, object> cache)
                {
                    if (cache.TryGetValue(this, out var result))
                    {
                        return result;
                    }

                    result = GetValueOverride(cache);
                    
                    return result;
                }
            }

            private class SerializingSnapshotValue : SnapshotValue
            {
                private readonly byte[] _data;

                public SerializingSnapshotValue(object value, IDictionary<object, SnapshotValue> hashTable)
                {
                    hashTable.Add(value, this);

                    var formatter = new BinaryFormatter();
                    using (var stream = new MemoryStream())
                    {
                        formatter.Serialize(stream, value);
                        _data = stream.ToArray();
                    }
                }

                protected override void RestorePropertiesOverride(object model, Dictionary<SnapshotValue, object> cache)
                {
                    throw new NotImplementedException();
                }

                protected override object GetValueOverride(Dictionary<SnapshotValue, object> cache)
                {
                    object result;

                    var formatter = new BinaryFormatter();
                    using (var stream = new MemoryStream(_data))
                    {
                        result = formatter.Deserialize(stream);
                    }

                    cache.Add(this, result);
                    return result;
                }
            }
            
            private class SimpleSnapshotValue : SnapshotValue
            {
                private readonly object _boxingValue;

                public SimpleSnapshotValue(object value)
                {
                    _boxingValue = value;
                }

                protected override void RestorePropertiesOverride(object model, Dictionary<SnapshotValue, object> cache)
                {
                    throw new InvalidOperationException();
                }

                protected override object GetValueOverride(Dictionary<SnapshotValue, object> cache)
                {
                    return _boxingValue;
                }
            }

            private sealed class NullSnapshotValue : SimpleSnapshotValue
            {
                public NullSnapshotValue()
                    : base(null)
                {}
            }

            private class ClassSnapshotValue : SnapshotValue
            {
                private readonly bool _isOwnDirty;
                private readonly object _referencedModel;
                private readonly Dictionary<FieldInfo, SnapshotValue> _fieldSnapshots = 
                    new Dictionary<FieldInfo, SnapshotValue>();

                public ClassSnapshotValue(object model, IDictionary<object, SnapshotValue> hashTable)
                    : this(model, hashTable, false)
                {}

                protected ClassSnapshotValue(object model, IDictionary<object, SnapshotValue> hashTable, bool isList)
                {
                    if (model is EditableModel<T> editableModel)
                    {
                        _isOwnDirty = editableModel.OwnDirty;
                    }

                    _referencedModel = model;
                    hashTable.Add(model, this);

                    if (isList)
                    {
                        return;
                    }

                    var type = model.GetType();

                    var storableFields = TypeInformationProvider.GetStorableFields(type);
                    foreach (var fieldInfo in storableFields.Where(x => !x.IsNotSerialized))
                    {
                        var value = fieldInfo.GetValue(model);
                        var snapshot = Create(value, hashTable, fieldInfo.IsInitOnly);
                        
                        if (snapshot != null)
                        {
                            _fieldSnapshots.Add(fieldInfo, snapshot);
                        }
                    }
                }

                private bool CheckBaseType(Type type, Type baseType)
                {
                    if (baseType == typeof(object))
                    {
                        return true;
                    }

                    if (baseType.IsGenericTypeDefinition)
                    {
                        if (type.IsGenericType && type.GetGenericTypeDefinition() == baseType)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (type == baseType)
                        {
                            return true;
                        }
                    }

                    if (type.BaseType == typeof(object))
                    {
                        return false;
                    }

                    return CheckBaseType(type.BaseType, baseType);
                }

                private void NotifyAllPropertiesChanged(INotifyPropertyChanged model)
                {
                    var type = model.GetType();

                    Action<object, PropertyInfo> notifyAction = null;

                    if (CheckBaseType(type, typeof(NotifyPropertyChangedBase<>)))
                    {
                        var method = type.GetMethod(
                            "NotifyOfPropertyChange",
                            BindingFlags.Instance | BindingFlags.NonPublic,
                            null,
                            new[] {typeof(PropertyInfo)},
                            null);
                        if (method != null)
                        {
                            notifyAction = (o, info) => method.Invoke(o, new object[] {info});
                        }
                    }

                    if (notifyAction == null)
                    {
                        var notifyEvents = TypeInformationProvider.GetPropertyChangedEventHandlers(type)
                            .Select(x => x.GetValue(model) as PropertyChangedEventHandler)
                            .Where(x => !ReferenceEquals(x, null))
                            .ToArray();

                        if (notifyEvents.Length == 0)
                        {
                            return;
                        }

                        notifyAction = (o, info) =>
                        {
                            foreach (var notifyEvent in notifyEvents)
                            {
                                notifyEvent(model, new PropertyChangedEventArgs(info.Name));
                            }
                        };
                    }

                    var storableProperties = TypeInformationProvider.GetStorableProperties(model.GetType());
                    foreach (var property in storableProperties.Where(x => x.CanWrite && x.SetMethod != null))
                    {
                        notifyAction(model, property);
                    }
                }

                protected sealed override object GetValueOverride(Dictionary<SnapshotValue, object> cache)
                {
                    var model = _referencedModel;
                    cache.Add(this, model);
                    RestoreProperties(model, cache);
                    return model;
                }

                protected override void RestorePropertiesOverride(object model, Dictionary<SnapshotValue, object> cache)
                {
                    foreach (var infoPair in _fieldSnapshots)
                    {
                        var memberInfo = infoPair.Key;
                        var snapshot = infoPair.Value;

                        if (memberInfo.IsInitOnly)
                        {
                            var currentModel = memberInfo.GetValue(model);
                            snapshot.RestoreProperties(currentModel, cache);
                        }
                        else
                        {
                            var value = snapshot.GetValue(cache);
                            memberInfo.SetValue(model, value);
                        }
                    }

                    if (model is INotifyPropertyChanged notifyPropertyChanged)
                    {
                        NotifyAllPropertiesChanged(notifyPropertyChanged);
                    }

                    if (model is EditableModel<T> editableModel)
                    {
                        editableModel.OwnDirty = _isOwnDirty;
                    }
                }
            }

            private class DictionarySnapshotValue : ClassSnapshotValue
            {
                private readonly Dictionary<object, SnapshotValue> _values = new Dictionary<object, SnapshotValue>();

                public DictionarySnapshotValue(IDictionary dictionary, IDictionary<object, SnapshotValue> hashTable)
                    : base(dictionary, hashTable, true)
                {
                    //This has been added to avoid cases where the collection of keys is changed during enumeration
                    //concurrent threads, etc.
                    var keys = new object[dictionary.Count];
                    dictionary.Keys.CopyTo(keys, 0);                    
                    foreach (var key in keys)
                    {
                        if (dictionary.Contains(key))
                        {
                            var value = dictionary[key];
                            _values.Add(key, Create(value, hashTable, false));
                        }                        
                    }
                }

                protected override void RestorePropertiesOverride(object model, Dictionary<SnapshotValue, object> cache)
                {
                    var dictionary = (IDictionary) model;
                    dictionary.Clear();
                    _values.ForEach(x => dictionary.Add(x.Key, x.Value.GetValue(cache)));
                    
                    base.RestorePropertiesOverride(model, cache);
                }
            }

            private class ListSnapshotValue : ClassSnapshotValue
            {
                private readonly List<SnapshotValue> _values = new List<SnapshotValue>();

                public ListSnapshotValue(IList list, IDictionary<object, SnapshotValue> hashTable)
                    : base(list, hashTable, true)
                {
                    var snapshotValues = list.OfType<object>().ToArray().Select(x => Create(x, hashTable, false));
                    _values.AddRange(snapshotValues);
                }

                protected override void RestorePropertiesOverride(object model, Dictionary<SnapshotValue, object> cache)
                {
                    var list = (IList) model;
                    list.Clear();
                    _values.ForEach(x => list.Add(x.GetValue(cache)));
                    
                    base.RestorePropertiesOverride(model, cache);
                }
            }

            private readonly ClassSnapshotValue _snapshotValue;
            private readonly bool _isOwnDirty;

            public ComplexSnapshot(EditableModel<T> model)
            {
                _snapshotValue = SnapshotValue.Create(model);
                _isOwnDirty = model.OwnDirty;
            }

            public void Restore(EditableModel<T> model)
            {
                _snapshotValue.RestoreProperties(model);
                model.OwnDirty = _isOwnDirty;
            }
        }
    }
}