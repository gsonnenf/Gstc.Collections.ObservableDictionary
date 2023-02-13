using System;

namespace Gstc.Collections.ObservableDictionary.ComponentModel {
    public class DictRemoveEventArgs<TKey, TValue> : INotifyDictionaryChangedEventArgs<TKey, TValue> {

        #region Properties
        public NotifyDictionaryChangedAction Action => NotifyDictionaryChangedAction.Remove;
        public TKey Key { get; }
        public TValue OldValue { get; }
        public TValue NewValue => throw new NotSupportedException("Dictionary Reset event does not support NewValue.");
        #endregion

        public DictRemoveEventArgs(TKey key, TValue oldValue) {
            Key = key ?? throw new ArgumentException("KeyMustNotBeNull", "Key");
            OldValue = oldValue;
        }


    }
}
