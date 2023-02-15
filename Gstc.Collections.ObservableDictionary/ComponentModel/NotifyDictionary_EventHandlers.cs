namespace Gstc.Collections.ObservableDictionary.ComponentModel {
    public delegate void NotifyDictionaryDictionaryChangedEventHandler<TKey, TValue>(object sender, INotifyDictionaryChangedEventArgs<TKey, TValue> e);
    public delegate void NotifyDictionaryChangingEventHandler<TKey, TValue>(object sender, INotifyDictionaryChangedEventArgs<TKey, TValue> e);
    public delegate void NotifyDictAddEventHandler<TKey, TValue>(object sender, DictAddEventArgs<TKey, TValue> args);
    public delegate void NotifyDictRemoveEventHandler<TKey, TValue>(object sender, DictRemoveEventArgs<TKey, TValue> args);
    public delegate void NotifyDictReplaceEventHandler<TKey, TValue>(object sender, DictReplaceEventArgs<TKey, TValue> args);
    public delegate void NotifyDictResetEventHandler<TKey, TValue>(object sender, DictResetEventArgs<TKey, TValue> args);
}
