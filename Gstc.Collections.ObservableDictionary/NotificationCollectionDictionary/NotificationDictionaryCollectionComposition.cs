using System.Collections;
using System.Collections.Specialized;
using Gstc.Collections.ObservableDictionary.NotificationDictionary;
using Gstc.Collections.ObservableLists.Base.Notify;

namespace Gstc.Collections.ObservableDictionary.NotificationCollectionDictionary {

    namespace Gstc.Collections.ObservableDictionary.Notification {

        public class NotifyDictionaryCollectionComposition<TDictionary> :
            NotifyPropertyComposition,
            INotifyDictionaryChanged,
            INotifyDictionaryChangedExtended,
            INotifyListChanged,
            INotifyCollectionChanged{

            #region Fields and Properties

            public event NotifyCollectionChangedEventHandler CollectionChanged;
            public event NotifyDictionaryChangedEventHandler DictionaryChanged;

            public event NotifyCollectionChangedEventHandler Added;
            public event NotifyCollectionChangedEventHandler Removed;
            public event NotifyCollectionChangedEventHandler Moved;
            public event NotifyCollectionChangedEventHandler Replaced;
            public event NotifyCollectionChangedEventHandler Reset;

            public event NotifyDictionaryChangedEventHandler AddedDictionary;
            public event NotifyDictionaryChangedEventHandler RemovedDictionary;
            public event NotifyDictionaryChangedEventHandler ReplacedDictionary;
            public event NotifyDictionaryChangedEventHandler ResetDictionary;
            #endregion

            #region Constructor
            public NotifyDictionaryCollectionComposition(TDictionary parent) : base(parent) { Parent = parent; }
            #endregion

            #region Collection Methods
            public void OnCollectionChangedReset() {
                var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
                using (BlockReentrancy()) {
                    CollectionChanged?.Invoke(Parent, eventArgs);
                    Reset?.Invoke(Parent, eventArgs);
                }
            }
            public void OnCollectionChangedAdd(object value, int index) {
                var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, index);
                using (BlockReentrancy()) {
                    CollectionChanged?.Invoke(Parent, eventArgs);
                    Added?.Invoke(Parent, eventArgs);
                }
            }
            //TODO: "Range actions" in WPF is not supported. This is a WPF problem, not a GSTC problem. Perhaps a workaround could be good. 
            public void OnCollectionChangedAddMany(IList valueList, int index) {
                var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, valueList, index);
                using (BlockReentrancy()) {
                    CollectionChanged?.Invoke(Parent, eventArgs);
                    Added?.Invoke(Parent, eventArgs);
                }
            }

            public void OnCollectionChangedRemove(object value, int index) {
                var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, value, index);
                using (BlockReentrancy()) {
                    CollectionChanged?.Invoke(Parent, eventArgs);
                    Removed?.Invoke(Parent, eventArgs);
                }
            }

            public void OnCollectionChangedMove(object value, int index, int oldIndex) {
                var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, value, index, oldIndex);
                using (BlockReentrancy()) {
                    CollectionChanged?.Invoke(Parent, eventArgs);
                    Moved?.Invoke(Parent, eventArgs);
                }
            }

            public void OnCollectionChangedReplace(object oldValue, object newValue, int index) {
                var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newValue, oldValue, index);
                using (BlockReentrancy()) {
                    CollectionChanged?.Invoke(Parent, eventArgs);
                    Replaced?.Invoke(Parent, eventArgs);
                }
            }
            #endregion

            #region Dictionary Methods
            public void OnDictionaryReset() {
                var eventArgs = new NotifyDictionaryChangedEventArgs(NotifyDictionaryChangedAction.Reset);
                using (BlockReentrancy()) {
                    DictionaryChanged?.Invoke(Parent, eventArgs);
                    ResetDictionary?.Invoke(this, eventArgs);
                }
            }
            public void OnDictionaryAdd(object key, object item) {
                var eventArgs = new NotifyDictionaryChangedEventArgs(NotifyDictionaryChangedAction.Add, key, item);
                using (BlockReentrancy()) {
                    DictionaryChanged?.Invoke(Parent, eventArgs);
                    AddedDictionary?.Invoke(this, eventArgs);
                }
            }
            public void OnDictionaryRemove(object key, object item) {
                var eventArgs = new NotifyDictionaryChangedEventArgs(NotifyDictionaryChangedAction.Remove, key, item);
                using (BlockReentrancy()) {
                    DictionaryChanged?.Invoke(Parent, eventArgs);
                    RemovedDictionary?.Invoke(this, eventArgs);
                }
            }

            public void OnDictionaryReplace(object key, object oldItem, object newItem) {
                var eventArgs = new NotifyDictionaryChangedEventArgs(NotifyDictionaryChangedAction.Replace, key, oldItem, newItem);
                using (BlockReentrancy()) {
                    DictionaryChanged?.Invoke(Parent, eventArgs);
                    ReplacedDictionary?.Invoke(this, eventArgs);
                }
            }
            #endregion

           
        }
    }


}
