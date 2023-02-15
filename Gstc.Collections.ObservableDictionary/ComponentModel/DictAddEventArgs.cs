using System;

namespace Gstc.Collections.ObservableDictionary.ComponentModel {
    public class DictAddEventArgs<TKey, TValue> : INotifyDictionaryChangedEventArgs<TKey, TValue> {

        #region Properties
        public NotifyDictionaryChangedAction Action => NotifyDictionaryChangedAction.Add;
        public TKey Key { get; }
        public TValue NewValue { get; }
        TValue INotifyDictionaryChangedEventArgs<TKey, TValue>.OldValue => throw DictionaryEventArgsException.Create(nameof(DictAddEventArgs<TKey, TValue>));
        #endregion

        public DictAddEventArgs(TKey key, TValue newValue) {
            Key = key ?? throw new ArgumentException("KeyMustNotBeNull", "Key");
            NewValue = newValue;
        }

    }
}
