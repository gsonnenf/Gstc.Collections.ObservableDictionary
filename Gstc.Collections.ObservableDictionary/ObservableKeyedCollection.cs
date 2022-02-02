using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Gstc.Collections.ObservableDictionary.Interface;
using Gstc.Collections.ObservableDictionary.NotificationCollectionDictionary.Gstc.Collections.ObservableDictionary.Notification;
using Gstc.Collections.ObservableDictionary.NotificationDictionary;
using Gstc.Collections.ObservableLists.Interface;

namespace Gstc.Collections.ObservableDictionary {
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TItem"></typeparam>
    public abstract class ObservableKeyedCollection<TKey, TItem> : 
        KeyedCollection<TKey, TItem>,
        IObservableCollection<TItem>,
        INotifyDictionaryChanged {
        
        #region Events
        public event PropertyChangedEventHandler PropertyChanged {
            add => Notify.PropertyChanged += value;
            remove => Notify.PropertyChanged -= value;
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged {
            add => Notify.CollectionChanged += value;
            remove => Notify.CollectionChanged -= value;
        }

        public event NotifyDictionaryChangedEventHandler DictionaryChanged {
            add => Notify.DictionaryChanged += value;
            remove => Notify.DictionaryChanged -= value;
        }
        #endregion

        #region Fields and Properties
         public NotifyDictionaryCollectionComposition<ObservableKeyedCollection<TKey, TItem>> Notify { get; protected set; }

        #endregion

        #region Constructors
        protected ObservableKeyedCollection() {
            Notify = new NotifyDictionaryCollectionComposition<ObservableKeyedCollection<TKey, TItem>>(this);
        }
        #endregion

        #region Override Methods
        protected override void ClearItems() {
            //TODO: CheckReentrancy();
            base.ClearItems();
            Notify.OnCollectionChangedReset();
            Notify.OnPropertyChangedCountAndIndex();
            Notify.OnDictionaryReset();
        }
        protected override void InsertItem(int index, TItem item) {
            base.InsertItem(index, item);
            Notify.OnCollectionChangedAdd(item, index);
            Notify.OnPropertyChangedCountAndIndex();
            Notify.OnDictionaryAdd(GetKeyForItem(item),item); //TODO: Decide if we need checking to ensure a non-null key.
        }

        protected override void RemoveItem(int index) {
            var oldItem = this[index];
            base.RemoveItem(index);
            Notify.OnCollectionChangedRemove(oldItem, index);
            Notify.OnPropertyChangedCountAndIndex();
            Notify.OnDictionaryRemove(GetKeyForItem(oldItem), oldItem);

        }

        protected override void SetItem(int index, TItem item) {
            var oldItem = this[index];
            base.SetItem(index, item);
            Notify.OnCollectionChangedReplace(oldItem, item, index);
            Notify.OnPropertyChangedIndex();
            Notify.OnDictionaryReplace(GetKeyForItem(item),oldItem,item);
        }
        #endregion
    }
}
