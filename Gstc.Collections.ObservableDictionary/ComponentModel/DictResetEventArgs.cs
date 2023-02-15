namespace Gstc.Collections.ObservableDictionary.ComponentModel {
    public class DictResetEventArgs<TKey, TValue> : INotifyDictionaryChangedEventArgs<TKey, TValue> {

        #region Properties
        public NotifyDictionaryChangedAction Action => NotifyDictionaryChangedAction.Reset;
        #region Interface Implementation
        TKey INotifyDictionaryChangedEventArgs<TKey, TValue>.Key => throw DictionaryEventArgsException.Create(nameof(DictResetEventArgs<TKey, TValue>));
        TValue INotifyDictionaryChangedEventArgs<TKey, TValue>.NewValue => throw DictionaryEventArgsException.Create(nameof(DictResetEventArgs<TKey, TValue>));
        TValue INotifyDictionaryChangedEventArgs<TKey, TValue>.OldValue => throw DictionaryEventArgsException.Create(nameof(DictResetEventArgs<TKey, TValue>));
        #endregion
        #endregion
    }
}
