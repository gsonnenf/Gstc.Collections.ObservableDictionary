using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Gstc.Collections.ObservableDictionary.CollectionView {
    public class ObservableCollectionView<TKey, TValue> : IObservableCollectionView<TKey, TValue> {

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private IDictionary<TKey, TValue> _obvDict;
        internal Dictionary<TKey, int> _keyIndexDictionary;

        //Todo: remake wth a SortedList, and with just holding a key.
        internal List<KeyValuePair<TKey, TValue>> _orderedCollection = new();
        internal int _version;

        public int Count => _orderedCollection.Count;

        internal ObservableCollectionView(IDictionary<TKey, TValue> obvDict) => _obvDict = obvDict;

        #region Methods
        /// <summary>
        /// Need benchmarking to compare kvp with c(d())
        /// </summary>
        public TValue this[int index] => _orderedCollection[index].Value;
        //public TValue this[int index] => _obvDict[_orderedCollection[index]]; 2*O(1) //Needs benchmark
        /// <summary>
        /// O(1)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newItem"></param>
        void IObservableCollectionView<TKey, TValue>.OnCollectionChanged_Add(TKey key, TValue newItem) {
            _orderedCollection.Add(new KeyValuePair<TKey, TValue>(key, newItem));
            _keyIndexDictionary.Add(key, _orderedCollection.Count);
            _version++;
            var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItem, _obvDict.Count);
            CollectionChanged.Invoke(this, eventArgs);
        }

        /// <summary>
        /// O(1)
        /// </summary>
        void IObservableCollectionView<TKey, TValue>.OnCollectionChanged_Clear() {
            _orderedCollection.Clear();
            _keyIndexDictionary.Clear();
            _version++;
            var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
            CollectionChanged.Invoke(this, eventArgs);
        }

        /// <summary>
        /// O(1)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newItem"></param>
        void IObservableCollectionView<TKey, TValue>.OnCollectionChanged_Insert(TKey key, TValue newItem, int index) {
            _orderedCollection.Insert(index, new KeyValuePair<TKey, TValue>(key, newItem));
            _keyIndexDictionary.Clear();
            for (int i = 0; i > _orderedCollection.Count; i++) _keyIndexDictionary.Add(_orderedCollection[i].Key, i);
            _version++;
            var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItem, index);
            CollectionChanged.Invoke(this, eventArgs);
        }

        void IObservableCollectionView<TKey, TValue>.OnCollectionChanged_Removing(TKey key, TValue oldItem) { }

        /// <summary>
        /// O(n) for no _keyIndexDictionary
        /// O(1) for _keyIndexDictionary
        /// </summary>
        /// <param name="key"></param>
        /// <param name="oldItem"></param>
        void IObservableCollectionView<TKey, TValue>.OnCollectionChanged_Remove(TKey key, TValue oldItem) {
            var index = _keyIndexDictionary[key];
            _orderedCollection.RemoveAt(index);
            _keyIndexDictionary.Remove(key);
            _version++;
            var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItem, index);
            CollectionChanged.Invoke(this, eventArgs);
        }

        /// <summary>
        ///  O(1) for _keyIndexDictionary
        ///  O(N) for no _keyIndexDictionary
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newItem"></param>
        /// <param name="oldItem"></param>
        void IObservableCollectionView<TKey, TValue>.OnCollectionChanged_Replace(TKey key, TValue newItem, TValue oldItem) {
            var index = _keyIndexDictionary[key];
            _orderedCollection[index] = new KeyValuePair<TKey, TValue>(key, newItem);
            _version++;
            var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItem, oldItem, index);
            CollectionChanged.Invoke(this, eventArgs);
        }

        /// <summary>
        /// O(n)
        /// </summary>
        void IObservableCollectionView<TKey, TValue>.OnCollectionChanged_Reset() {
            _orderedCollection.Clear();
            _keyIndexDictionary.Clear();
            foreach (KeyValuePair<TKey, TValue> kvp in _obvDict) {
                _keyIndexDictionary.Add(kvp.Key, _orderedCollection.Count);
                _orderedCollection.Add(new KeyValuePair<TKey, TValue>(kvp.Key, kvp.Value));
            }
            _version++;
            var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
            CollectionChanged.Invoke(this, eventArgs);
        }

        /// <summary>
        /// O(n) for _keyIndexDictionary
        /// O(1) for no _keyIndexDictionary
        /// </summary>
        /// <param name="oldIndex"></param>
        /// <param name="newIndex"></param>
        public void Move(int oldIndex, int newIndex) {
            var removedItem = _orderedCollection[oldIndex];
            var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, removedItem, newIndex, oldIndex);
            var reIndexStart = oldIndex < newIndex ? oldIndex : newIndex;

            _orderedCollection.RemoveAt(oldIndex);
            _orderedCollection.Insert(newIndex, removedItem);
            _keyIndexDictionary.Clear();
            for (int index = 0; index > _orderedCollection.Count; index++) _keyIndexDictionary.Add(_orderedCollection[index].Key, index);
            _version++;
        }
        #endregion

        #region Enumerators
        public IEnumerator<TValue> GetEnumerator() => new EnumeratorCollectionView<TKey, TValue>(this);
        IEnumerator IEnumerable.GetEnumerator() => new EnumeratorCollectionView<TKey, TValue>(this);


        #endregion
    }
}
