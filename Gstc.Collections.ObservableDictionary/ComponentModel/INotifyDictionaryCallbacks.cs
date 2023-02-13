namespace Gstc.Collections.ObservableDictionary.ComponentModel {
    public interface INotifyDictionaryCallbacks<TKey, TValue> : INotifyDictionaryChanged<TKey, TValue> {
        //todo: make these not generic if stil using. Maybe delete.
        void OnDictionaryReset();
        void OnDictionaryAdd(object key, object item);
        void OnDictionaryRemove(object key, object item);
        void OnDictionaryReplace(object key, object oldItem, object newItem);
        void CheckReentrancy();
    }
}
