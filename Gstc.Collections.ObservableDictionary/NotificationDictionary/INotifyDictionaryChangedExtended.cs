namespace Gstc.Collections.ObservableDictionary.NotificationDictionary {
    public interface INotifyDictionaryChangedExtended {
        /// <summary>
        /// Triggers events when an item or items are removed. 
        /// </summary>
        event NotifyDictionaryChangedEventHandler AddedDictionary;
        /// <summary>
        /// Triggers events when an item or items are removed. 
        /// </summary>
        event NotifyDictionaryChangedEventHandler RemovedDictionary;
        /// <summary>
        /// Triggers events when an item has been replaced. 
        /// </summary>
        event NotifyDictionaryChangedEventHandler ReplacedDictionary;
        /// <summary>
        /// Triggers events when an the list has changed substantially such as a Clear(). 
        /// </summary>
        event NotifyDictionaryChangedEventHandler ResetDictionary;
    }
}
