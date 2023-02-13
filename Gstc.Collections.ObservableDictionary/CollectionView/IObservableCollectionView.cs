using System.Collections.Generic;
using System.Collections.Specialized;

namespace Gstc.Collections.ObservableDictionary.CollectionView {
    public interface IObservableCollectionView<TKey, TValue> :
        IEnumerable<TValue>,
        INotifyCollectionChanged {
        internal void OnCollectionChanged_Add(TKey key, TValue newItem);
        internal void OnCollectionChanged_Clear();
        internal void OnCollectionChanged_Insert(TKey key, TValue newItem, int index);
        internal void OnCollectionChanged_Remove(TKey key, TValue oldItem);
        internal void OnCollectionChanged_Removing(TKey key, TValue oldItem);
        internal void OnCollectionChanged_Replace(TKey key, TValue newItem, TValue oldItem);
        internal void OnCollectionChanged_Reset();
    }
}
