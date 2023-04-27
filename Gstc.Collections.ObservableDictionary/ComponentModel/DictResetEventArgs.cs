namespace Gstc.Collections.ObservableDictionary.ComponentModel {
    public class DictResetEventArgs<TKey, TValue> : IDictionaryChangedEventArgs<TKey, TValue> {

        #region Properties
        public NotifyDictionaryChangedAction Action => NotifyDictionaryChangedAction.Reset;
        #region Interface Implementation
        TKey IDictionaryChangedEventArgs<TKey, TValue>.Key => throw DictionaryEventArgsException.Create(nameof(DictResetEventArgs<TKey, TValue>));
        TValue IDictionaryChangedEventArgs<TKey, TValue>.NewValue => throw DictionaryEventArgsException.Create(nameof(DictResetEventArgs<TKey, TValue>));
        TValue IDictionaryChangedEventArgs<TKey, TValue>.OldValue => throw DictionaryEventArgsException.Create(nameof(DictResetEventArgs<TKey, TValue>));
        #endregion
        #endregion
    }
}
