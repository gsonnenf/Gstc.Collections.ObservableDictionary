namespace Gstc.Collections.ObservableDictionary.ComponentModel {
    public interface INotifyDictionaryChangedEventArgs<TKey, TValue> {
        /// <summary>
        /// The action that caused the event.
        /// </summary>
        public NotifyDictionaryChangedAction Action { get; }
        /// <summary>
        /// The key where the change occurred.
        /// </summary>
        public TKey Key { get; }
        /// <summary>
        /// The old item.
        /// </summary>
        public TValue OldValue { get; }
        /// <summary>
        /// The new item.
        /// </summary>
        public TValue NewValue { get; }
    }
}
