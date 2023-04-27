using System;

namespace Gstc.Collections.ObservableDictionary.ComponentModel {
    public class DictRemoveEventArgs<TKey, TValue> : IDictionaryChangedEventArgs<TKey, TValue> {

        #region Properties
        public NotifyDictionaryChangedAction Action => NotifyDictionaryChangedAction.Remove;
        public TKey Key { get; }
        public TValue OldValue { get; }
        TValue IDictionaryChangedEventArgs<TKey, TValue>.NewValue => throw DictionaryEventArgsException.Create(nameof(DictRemoveEventArgs<TKey, TValue>));
        #endregion

        public DictRemoveEventArgs(TKey key, TValue oldValue) {
            Key = key ?? throw new ArgumentException("KeyMustNotBeNull", "Key");
            OldValue = oldValue;
        }


    }
}
