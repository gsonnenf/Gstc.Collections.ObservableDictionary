namespace Gstc.Collections.ObservableDictionary.ComponentModel {

    public delegate void NotifyDictionaryChangedEventHandler<TKey, TValue>(object sender, INotifyDictionaryChangedEventArgs<TKey, TValue> e);
    public delegate void NotifyDictionaryChangingEventHandler<TKey, TValue>(object sender, INotifyDictionaryChangedEventArgs<TKey, TValue> e);
    public delegate void NotifyDictAddEventHander<TKey, TValue>(object sender, DictAddEventArgs<TKey, TValue> args);
    public delegate void NotifyDictRemoveEventHander<TKey, TValue>(object sender, DictRemoveEventArgs<TKey, TValue> args);
    public delegate void NotifyDictReplaceEventHander<TKey, TValue>(object sender, DictReplaceEventArgs<TKey, TValue> args);
    public delegate void NotifyDictResetEventHander<TKey, TValue>(object sender, DictResetEventArgs<TKey, TValue> args);
}
