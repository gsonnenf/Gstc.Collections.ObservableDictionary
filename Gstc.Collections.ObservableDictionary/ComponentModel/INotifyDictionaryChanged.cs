using System.ComponentModel;

namespace Gstc.Collections.ObservableDictionary.ComponentModel {

    public interface INotifyDictionaryChanged<TKey, TValue> : INotifyPropertyChanged {
        event NotifyDictionaryChangedEventHandler<TKey, TValue> DictionaryChanged;
        /// <summary>
        /// Triggers events when an item or items are removed. 
        /// </summary>
        event NotifyDictAddEventHander<TKey, TValue> AddedDict;
        /// <summary>
        /// Triggers events when an item or items are removed. 
        /// </summary>
        event NotifyDictRemoveEventHander<TKey, TValue> RemovedDict;
        /// <summary>
        /// Triggers events when an item has been replaced. 
        /// </summary>
        event NotifyDictReplaceEventHander<TKey, TValue> ReplacedDict;
        /// <summary>
        /// Triggers events when an the list has changed substantially such as a Clear(). 
        /// </summary>
        event NotifyDictResetEventHander<TKey, TValue> ResetDict;
    }

}
