using Gstc.Collections.ObservableDictionary.ComponentModel;
using Gstc.Collections.ObservableDictionary.NotificationCollectionDictionary.Gstc.Collections.ObservableDictionary.Notification;
using Gstc.Collections.ObservableLists.Interface;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

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
        public NotifyDictionaryCollection Notify { get; protected set; }

        #endregion

        #region Constructors
        protected ObservableKeyedCollection() {
            Notify = new NotifyDictionaryCollection(this);
        }
        #endregion

        #region Methods
        public new TItem this[TKey i] {
            get => base[i];
            set => AddOrReplace(value);
        }

        public void AddOrReplace(TItem item) {
            if (TryGetValue(GetKeyForItem(item), out var oldItem)) Remove(oldItem);
            Add(item);
        }

        //TODO: Polyfill for .Net Framework 4.8, remove if upgrading to .Net Standard 2.1+ or .Net 5+
        public bool TryGetValue(TKey key, out TItem item) {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (Dictionary != null) return Dictionary.TryGetValue(key, out item!);

            foreach (TItem itemInItems in Items) {
                var keyInItems = GetKeyForItem(itemInItems);
                if (keyInItems == null || Comparer.Equals(key, keyInItems)) continue;
                item = itemInItems;
                return true;
            }
            item = default;
            return false;
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
            Notify.OnDictionaryAdd(GetKeyForItem(item), item); //TODO: Decide if we need checking to ensure a non-null key.
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
            Notify.OnDictionaryReplace(GetKeyForItem(item), oldItem, item);
        }
        #endregion

    }
}
