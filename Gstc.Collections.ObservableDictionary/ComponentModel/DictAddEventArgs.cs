using System;

namespace Gstc.Collections.ObservableDictionary.ComponentModel {
    public class DictAddEventArgs<TKey, TValue> : INotifyDictionaryChangedEventArgs<TKey, TValue> {

        #region Properties
        public NotifyDictionaryChangedAction Action => NotifyDictionaryChangedAction.Add;
        public TKey Key { get; }
        public TValue NewValue { get; }
        public TValue OldValue => throw new NotSupportedException("Dictionary Add event does not support oldValue.");
        #endregion

        public DictAddEventArgs(TKey key, TValue newValue) {
            Key = key ?? throw new ArgumentException("KeyMustNotBeNull", "Key");
            NewValue = newValue;
        }

    }
}
