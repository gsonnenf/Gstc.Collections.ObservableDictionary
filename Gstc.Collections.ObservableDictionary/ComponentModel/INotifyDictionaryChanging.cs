namespace Gstc.Collections.ObservableDictionary.ComponentModel {

    public interface INotifyDictionaryChanging<TKey, TValue> {
        event NotifyDictionaryChangingEventHandler<TKey, TValue> DictionaryChanging;
        /// <summary>
        /// Triggers events when an item or items are removed. 
        /// </summary>
        event NotifyDictAddEventHandler<TKey, TValue> AddingKvp;
        /// <summary>
        /// Triggers events when an item or items are removed. 
        /// </summary>
        event NotifyDictRemoveEventHandler<TKey, TValue> RemovingKvp;
        /// <summary>
        /// Triggers events when an item has been replaced. 
        /// </summary>
        event NotifyDictReplaceEventHandler<TKey, TValue> ReplacingKvp;
        /// <summary>
        /// Triggers events when an the list has changed substantially such as a Clear(). 
        /// </summary>
        event NotifyDictResetEventHandler<TKey, TValue> ResettingKvp;
    }

}
