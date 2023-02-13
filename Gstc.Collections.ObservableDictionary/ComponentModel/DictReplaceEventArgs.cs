using System;

namespace Gstc.Collections.ObservableDictionary.ComponentModel {
    public class DictReplaceEventArgs<TKey, TValue> : INotifyDictionaryChangedEventArgs<TKey, TValue> {

        #region Properties
        public NotifyDictionaryChangedAction Action => NotifyDictionaryChangedAction.Replace;
        public TKey Key { get; }
        public TValue OldValue { get; }
        public TValue NewValue { get; }
        #endregion

        public DictReplaceEventArgs(TKey key, TValue oldValue, TValue newValue) {
            Key = key ?? throw new ArgumentException("KeyMustNotBeNull", "Key");
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
