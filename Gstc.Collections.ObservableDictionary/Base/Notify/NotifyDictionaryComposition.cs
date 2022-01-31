using Gstc.Collections.ObservableLists.Base.Notify;

namespace Gstc.Collections.ObservableDictionary.Base.Notify {

    public class NotifyDictionaryComposition<TDictionary> :
        NotifyPropertyComposition,
        INotifyDictionaryChanged {

        #region Fields and Properties
        public TDictionary Parent { get; protected set; }

        public event NotifyDictionaryChangedEventHandler DictionaryChanged;

        public event NotifyDictionaryChangedEventHandler Added;

        public event NotifyDictionaryChangedEventHandler Removed;

        public event NotifyDictionaryChangedEventHandler Replaced;

        public event NotifyDictionaryChangedEventHandler Reset;
        #endregion

        #region Constructor
        public NotifyDictionaryComposition(TDictionary parent) { Parent = parent; }
        #endregion

        #region Methods
        public void OnDictionaryReset() {
            var eventArgs = new NotifyDictionaryChangedEventArgs(NotifyDictionaryChangedAction.Reset);
            using (BlockReentrancy()) {
                DictionaryChanged?.Invoke(Parent, eventArgs);
                Reset?.Invoke(this, eventArgs);
            }
        }

        public void OnDictionaryAdd(object key, object item) {
            var eventArgs = new NotifyDictionaryChangedEventArgs(NotifyDictionaryChangedAction.Add, key, item);
            using (BlockReentrancy()) {
                DictionaryChanged?.Invoke(Parent, eventArgs);
                Added?.Invoke(this, eventArgs);
            }
        }
        public void OnDictionaryRemove(object key, object item) {
            var eventArgs = new NotifyDictionaryChangedEventArgs(NotifyDictionaryChangedAction.Remove, key, item);
            using (BlockReentrancy()) {
                DictionaryChanged?.Invoke(Parent, eventArgs);
                Removed?.Invoke(this, eventArgs);
            }
        }

        public void OnDictionaryReplace(object key, object oldItem, object newItem) {
            var eventArgs = new NotifyDictionaryChangedEventArgs(NotifyDictionaryChangedAction.Replace, key, oldItem, newItem);
            using (BlockReentrancy()) {
                DictionaryChanged?.Invoke(Parent, eventArgs);
                Replaced?.Invoke(this, eventArgs);
            }
        }
        #endregion
    }
}

