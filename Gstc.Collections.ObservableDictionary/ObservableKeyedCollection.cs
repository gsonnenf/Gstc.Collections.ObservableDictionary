using Gstc.Collections.ObservableLists.Base.Notify;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Gstc.Collections.ObservableDictionary {
    public abstract class ObservableKeyedCollection<TKey, TItem> : KeyedCollection<TKey, TItem>, INotifyCollectionChanged, INotifyPropertyChanged {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged {
            add => NotifyProperty.PropertyChanged += value;
            remove => NotifyProperty.PropertyChanged -= value;
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged {
            add => NotifyCollection.CollectionChanged += value;
            remove => NotifyCollection.CollectionChanged -= value;
        }
        #endregion

        #region Fields and Properties
        public NotifyCollectionComposition<ObservableKeyedCollection<TKey, TItem>> NotifyCollection { get; protected set; }
        public NotifyPropertyComposition NotifyProperty { get; set; }
        #endregion

        #region Constructors
        protected ObservableKeyedCollection() {
            NotifyCollection = new NotifyCollectionComposition<ObservableKeyedCollection<TKey, TItem>>(this);
        }
        #endregion

        #region Override Methods
        protected override void ClearItems() {
            //TODO: CheckReentrancy();
            base.ClearItems();
            NotifyCollection.OnCollectionChangedReset();
            NotifyProperty.OnPropertyChangedCountAndIndex();
        }
        protected override void InsertItem(int index, TItem item) {
            base.InsertItem(index, item);
            NotifyCollection.OnCollectionChangedAdd(item, index);
            NotifyProperty.OnPropertyChangedCountAndIndex();
        }

        protected override void RemoveItem(int index) {
            var oldItem = this[index];
            base.RemoveItem(index);
            NotifyCollection.OnCollectionChangedRemove(oldItem, index);
            NotifyProperty.OnPropertyChangedCountAndIndex();
        }

        protected override void SetItem(int index, TItem item) {
            var oldItem = this[index];
            base.SetItem(index, item);
            NotifyCollection.OnCollectionChangedReplace(oldItem, item, index);
            NotifyProperty.OnPropertyChangedIndex();
        }
        #endregion
    }
}
