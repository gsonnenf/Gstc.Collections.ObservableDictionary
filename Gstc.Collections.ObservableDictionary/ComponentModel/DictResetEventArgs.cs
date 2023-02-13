using System;

namespace Gstc.Collections.ObservableDictionary.ComponentModel {
    public class DictResetEventArgs<TKey, TValue> : INotifyDictionaryChangedEventArgs<TKey, TValue> {

        #region Properties
        public NotifyDictionaryChangedAction Action => NotifyDictionaryChangedAction.Reset;
        #region Interface Implementation
        public TKey Key => throw new NotSupportedException("Dictionary Reset event does not support Key.");
        public TValue NewValue => throw new NotSupportedException("Dictionary Reset event does not support NewValue.");
        public TValue OldValue => throw new NotSupportedException("Dictionary Reset event does not support oldValue.");
        #endregion
        #endregion
    }
}
