using System.ComponentModel;

namespace Gstc.Collections.ObservableDictionary.ComponentModel {

    public interface INotifyDictionaryChanged<TKey, TValue> : INotifyPropertyChanged {
        event NotifyDictionaryDictionaryChangedEventHandler<TKey, TValue> DictionaryChanged;
        /// <summary>
        /// Triggers events when an item or items are removed. 
        /// </summary>
        event NotifyDictAddEventHandler<TKey, TValue> AddedKvp;
        /// <summary>
        /// Triggers events when an item or items are removed. 
        /// </summary>
        event NotifyDictRemoveEventHandler<TKey, TValue> RemovedKvp;
        /// <summary>
        /// Triggers events when an item has been replaced. 
        /// </summary>
        event NotifyDictReplaceEventHandler<TKey, TValue> ReplacedKvp;
        /// <summary>
        /// Triggers events when an the list has changed substantially such as a Clear(). 
        /// </summary>
        event NotifyDictResetEventHandler<TKey, TValue> ResetKvp;
    }

}
