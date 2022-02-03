using System;
using Gstc.Collections.ObservableDictionary.ComponentModel;
using Gstc.Collections.ObservableLists.ComponentModel;
using Gstc.Collections.ObservableLists.Notify;
using System.Collections;
using System.Collections.Specialized;
using Gstc.Collections.ObservableDictionary.Notify;

namespace Gstc.Collections.ObservableDictionary.NotificationCollectionDictionary {

    namespace Gstc.Collections.ObservableDictionary.Notification {

        public class NotifyDictionaryCollection :
            NotifyProperty,
            INotifyCollection,
            INotifyDictionary,
            INotifyDictionaryChangedExtended,
            INotifyCollectionChangedExtended {

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

            public NotifyDictionaryCollection() { }

            public NotifyDictionaryCollection(object sender) : base(sender) { Sender = sender; }
            #endregion

            #region Collection Methods
            public void OnCollectionChangedReset() {
                var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
                using (BlockReentrancy()) {
                    CollectionChanged?.Invoke(Sender, eventArgs);
                    Reset?.Invoke(Sender, eventArgs);
                }
            }
            public void OnCollectionChangedAdd(object value, int index) {
                var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, index);
                using (BlockReentrancy()) {
                    CollectionChanged?.Invoke(Sender, eventArgs);
                    Added?.Invoke(Sender, eventArgs);
                }
            }
            //TODO: "Range actions" in WPF is not supported. This is a WPF problem, not a GSTC problem. Perhaps a workaround could be good. 
            public void OnCollectionChangedAddMany(IList valueList, int index) {
                var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, valueList, index);
                using (BlockReentrancy()) {
                    CollectionChanged?.Invoke(Sender, eventArgs);
                    Added?.Invoke(Sender, eventArgs);
                }
            }

            public void OnCollectionChangedRemove(object value, int index) {
                var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, value, index);
                using (BlockReentrancy()) {
                    CollectionChanged?.Invoke(Sender, eventArgs);
                    Removed?.Invoke(Sender, eventArgs);
                }
            }

            public void OnCollectionChangedMove(object value, int index, int oldIndex) {
                var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, value, index, oldIndex);
                using (BlockReentrancy()) {
                    CollectionChanged?.Invoke(Sender, eventArgs);
                    Moved?.Invoke(Sender, eventArgs);
                }
            }

            public void OnCollectionChangedReplace(object oldValue, object newValue, int index) {
                var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newValue, oldValue, index);
                using (BlockReentrancy()) {
                    CollectionChanged?.Invoke(Sender, eventArgs);
                    Replaced?.Invoke(Sender, eventArgs);
                }
            }
            #endregion

            #region Dictionary Methods
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

            #region .NET Monitor
            public SimpleMonitor ReentrancyMonitor => _monitor ??= new SimpleMonitor(this);
            private SimpleMonitor _monitor; // Lazily allocated only when a subclass calls BlockReentrancy() or during serialization. 
            private int _blockReentrancyCount;

            public void CheckReentrancy() {
                if (_blockReentrancyCount <= 0) return;
                if (DictionaryChanged?.GetInvocationList().Length > 1)
                    throw new InvalidOperationException("ObservableCollectionReentrancyNotAllowed");
            }
            protected IDisposable BlockReentrancy() {
                _blockReentrancyCount++;
                return ReentrancyMonitor;
            }

            public class SimpleMonitor : IDisposable {
                private readonly NotifyDictionaryCollection _notify;
                public SimpleMonitor(NotifyDictionaryCollection notify) => _notify = notify;
                public void Dispose() => _notify._blockReentrancyCount--;
            }
            #endregion

        }
    }


}
