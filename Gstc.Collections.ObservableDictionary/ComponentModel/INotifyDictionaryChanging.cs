namespace Gstc.Collections.ObservableDictionary.ComponentModel {

    public interface INotifyDictionaryChanging<TKey, TValue> {
        event NotifyDictionaryChangingEventHandler<TKey, TValue> DictionaryChanging;
        /// <summary>
        /// Triggers events when an item or items are removed. 
        /// </summary>
        event NotifyDictAddEventHander<TKey, TValue> AddingDict;
        /// <summary>
        /// Triggers events when an item or items are removed. 
        /// </summary>
        event NotifyDictRemoveEventHander<TKey, TValue> RemovingDict;
        /// <summary>
        /// Triggers events when an item has been replaced. 
        /// </summary>
        event NotifyDictReplaceEventHander<TKey, TValue> ReplacingDict;
        /// <summary>
        /// Triggers events when an the list has changed substantially such as a Clear(). 
        /// </summary>
        event NotifyDictResetEventHander<TKey, TValue> ResetingDict;
    }

}
