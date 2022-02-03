using Gstc.Collections.ObservableDictionary.ComponentModel;
using Gstc.Collections.ObservableLists.Notify;

namespace Gstc.Collections.ObservableDictionary.Notify {
    public class NotifyDictionary :
        NotifyProperty,
        INotifyDictionary,
        INotifyDictionaryChangedExtended {

        #region Fields and Properties

        public event NotifyDictionaryChangedEventHandler DictionaryChanged;

        public event NotifyDictionaryChangedEventHandler AddedDictionary;
        public event NotifyDictionaryChangedEventHandler RemovedDictionary;
        public event NotifyDictionaryChangedEventHandler ReplacedDictionary;
        public event NotifyDictionaryChangedEventHandler ResetDictionary;
        #endregion

        #region Constructor

        public NotifyDictionary() { }
        public NotifyDictionary(object sender) : base(sender) { Sender = sender; }
        #endregion

        #region Methods
        public void OnDictionaryReset() {
            var eventArgs = new NotifyDictionaryChangedEventArgs(NotifyDictionaryChangedAction.Reset);
            using (BlockReentrancy()) {
                DictionaryChanged?.Invoke(Sender, eventArgs);
                ResetDictionary?.Invoke(this, eventArgs);
            }
        }
        public void OnDictionaryAdd(object key, object item) {
            var eventArgs = new NotifyDictionaryChangedEventArgs(NotifyDictionaryChangedAction.Add, key, item);
            using (BlockReentrancy()) {
                DictionaryChanged?.Invoke(Sender, eventArgs);
                AddedDictionary?.Invoke(this, eventArgs);
            }
        }
        public void OnDictionaryRemove(object key, object item) {
            var eventArgs = new NotifyDictionaryChangedEventArgs(NotifyDictionaryChangedAction.Remove, key, item);
            using (BlockReentrancy()) {
                DictionaryChanged?.Invoke(Sender, eventArgs);
                RemovedDictionary?.Invoke(this, eventArgs);
            }
        }
        public void OnDictionaryReplace(object key, object oldItem, object newItem) {
            var eventArgs = new NotifyDictionaryChangedEventArgs(NotifyDictionaryChangedAction.Replace, key, oldItem, newItem);
            using (BlockReentrancy()) {
                DictionaryChanged?.Invoke(Sender, eventArgs);
                ReplacedDictionary?.Invoke(this, eventArgs);
            }
        }
        #endregion
    }
}

