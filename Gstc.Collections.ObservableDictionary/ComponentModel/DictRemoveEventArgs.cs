using System;

namespace Gstc.Collections.ObservableDictionary.ComponentModel {
    public class DictRemoveEventArgs<TKey, TValue> : INotifyDictionaryChangedEventArgs<TKey, TValue> {

        #region Properties
        public NotifyDictionaryChangedAction Action => NotifyDictionaryChangedAction.Remove;
        public TKey Key { get; }
        public TValue OldValue { get; }
        TValue INotifyDictionaryChangedEventArgs<TKey, TValue>.NewValue => throw DictionaryEventArgsException.Create(nameof(DictRemoveEventArgs<TKey, TValue>));
        #endregion

        public DictRemoveEventArgs(TKey key, TValue oldValue) {
            Key = key ?? throw new ArgumentException("KeyMustNotBeNull", "Key");
            OldValue = oldValue;
        }


    }
}
