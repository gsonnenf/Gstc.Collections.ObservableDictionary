namespace Gstc.Collections.ObservableDictionary.Base.Notify {
    public interface INotifyDictionaryChangedExtended : INotifyDictionaryChanged {
        /// <summary>
        /// Triggers events when an item or items are removed. 
        /// </summary>
        event NotifyDictionaryChangedEventHandler Added;
        /// <summary>
        /// Triggers events when an item or items are removed. 
        /// </summary>
        event NotifyDictionaryChangedEventHandler Removed;
        /// <summary>
        /// Triggers events when an item has been replaced. 
        /// </summary>
        event NotifyDictionaryChangedEventHandler Replaced;
        /// <summary>
        /// Triggers events when an the list has changed substantially such as a Clear(). 
        /// </summary>
        event NotifyDictionaryChangedEventHandler Reset;
    }
}
