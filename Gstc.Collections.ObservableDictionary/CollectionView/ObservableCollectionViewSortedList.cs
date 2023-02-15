using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Gstc.Collections.ObservableDictionary.CollectionView {
    public class ObservableCollectionViewSortedList<TKey, TValue> : IObservableListView<TKey, TValue> {

        #region Events
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event NotifyCollectionChangedEventHandler Adding;
        public event NotifyCollectionChangedEventHandler Moving;
        public event NotifyCollectionChangedEventHandler Removing;
        public event NotifyCollectionChangedEventHandler Replacing;
        public event NotifyCollectionChangedEventHandler Resetting;
        public event NotifyCollectionChangedEventHandler CollectionChanging;
        public event NotifyCollectionChangedEventHandler Added;
        public event NotifyCollectionChangedEventHandler Moved;
        public event NotifyCollectionChangedEventHandler Removed;
        public event NotifyCollectionChangedEventHandler Replaced;
        public event NotifyCollectionChangedEventHandler Reset;
        #endregion
        private SortedList<TKey, TValue> _sortedList;
        internal int _removeKeyIndex;

        public KvpEnumeratorType EnumeratorType { get; set; } = KvpEnumeratorType.Value;

        //public int Count => _sortedList.Count;

        internal ObservableCollectionViewSortedList(SortedList<TKey, TValue> sortedList) => _sortedList = sortedList;

        #region Methods
        /// <summary>
        /// Need benchmarking to compare kvp with c(d())
        /// </summary>
        public TValue this[int index] {
            get {
                //Todo: Test to ensure enumerator indexis aligned correctly.
                if (index >= _sortedList.Count) throw new IndexOutOfRangeException();
                using var enumerator = _sortedList.GetEnumerator();
                for (int index2 = 0; index < index2; index++) enumerator.MoveNext();
                return enumerator.Current.Value;
            }
        }
        //public TValue this[int index] => _obvDict[_orderedCollection[index]]; 2*O(1) //Needs benchmark
        /// <summary>
        /// O(1)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newItem"></param>
        void OnCollectionChanged_Add(TKey key, TValue newItem) {
            var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItem, _sortedList.IndexOfKey(key));
            CollectionChanged.Invoke(this, eventArgs);
        }

        /// <summary>
        /// O(1)
        /// </summary>
        void OnCollectionChanged_Clear() {
            var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
            CollectionChanged.Invoke(this, eventArgs);
        }

        /// <summary>
        /// O(1)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newItem"></param>
        void OnCollectionChanged_Insert(TKey key, TValue newItem, int index)
            => throw new NotSupportedException("ObservableCollectionViewSortedList does not support insert.");

        /// <summary>
        /// Index of key needs to be read before removal.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="oldItem"></param>
        void OnCollectionChanged_Removing(TKey key, TValue oldItem)
            => _removeKeyIndex = _sortedList.IndexOfKey(key);
        /// <summary>
        /// O(n) for no _keyIndexDictionary
        /// O(1) for _keyIndexDictionary
        /// </summary>
        /// <param name="key"></param>
        /// <param name="oldItem"></param>
        void OnCollectionChanged_Remove(TKey key, TValue oldItem) {
            var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItem, _removeKeyIndex);
            CollectionChanged.Invoke(this, eventArgs);
        }

        /// <summary>
        ///  O(1) for _keyIndexDictionary
        ///  O(N) for no _keyIndexDictionary
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newItem"></param>
        /// <param name="oldItem"></param>
        void OnCollectionChanged_Replace(TKey key, TValue newItem, TValue oldItem) {
            var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItem, oldItem, _sortedList.IndexOfKey(key));
            CollectionChanged.Invoke(this, eventArgs);
        }

        /// <summary>
        /// O(n)
        /// </summary>
        void OnCollectionChanged_Reset() {
            var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
            CollectionChanged.Invoke(this, eventArgs);
        }
        #endregion

        #region Enumerators
        public IEnumerator<TValue> GetEnumerator() => _sortedList.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => EnumeratorType switch {
            KvpEnumeratorType.Value => _sortedList.Values.GetEnumerator(),
            KvpEnumeratorType.Key => _sortedList.Keys.GetEnumerator(),
            KvpEnumeratorType.KeyValuePair => _sortedList.GetEnumerator()
        };
        #endregion
    }
}
