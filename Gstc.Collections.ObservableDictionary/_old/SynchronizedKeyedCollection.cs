namespace Gstc.Collections.ObservableDictionary._old {
    /*
    public abstract class KeyedICollection<TKey, TItem, TCollection> : ICollection<TItem>
    where TCollection : ICollection<TItem> {
        private KeyedCollection<int, int> a;
        protected abstract TKey GetKeyForItem(TItem item);

        private TCollection _collection;

        #region Fields and Properties
        private IEqualityComparer<TKey> _comparer;
        private Dictionary<TKey, TItem> _dictionary;
        private int _keyCount;
        private int _threshold;

        protected IDictionary<TKey, TItem> Dictionary => (IDictionary<TKey, TItem>)_dictionary;
        #endregion

        #region Constructor
        protected KeyedICollection() {
            _comparer = EqualityComparer<TKey>.Default;
            _threshold = int.MaxValue;
        }

        protected KeyedICollection(object syncRoot) : base(syncRoot) {
            _comparer = EqualityComparer<TKey>.Default;
            _threshold = int.MaxValue;
        }

        protected KeyedICollection(object syncRoot, IEqualityComparer<TKey> comparer) : base(syncRoot) {
            _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
            _threshold = int.MaxValue;
        }

      
        #endregion

        #region Override Methods

        /*
        protected void ClearItems() {
            _collection.ClearItems();
            _dictionary?.Clear();
            _keyCount = 0;
        }
        protected override void InsertItem(int index, TItem item) {
            TKey keyForItem = GetKeyForItem(item);
            if (keyForItem != null) AddKey(keyForItem, item);
            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index) {
            TKey keyForItem = GetKeyForItem(Items[index]);
            if (keyForItem != null) RemoveKey(keyForItem);
            base.RemoveItem(index);
        }

        protected override void SetItem(int index, TItem item) {
            TKey keyForItem1 = GetKeyForItem(item);
            TKey keyForItem2 = GetKeyForItem(Items[index]);
            if (_comparer.Equals(keyForItem1, keyForItem2)) {
                if (keyForItem1 != null && _dictionary != null) _dictionary[keyForItem1] = item;
            } else {
                if (keyForItem1 != null) AddKey(keyForItem1, item);
                if (keyForItem2 != null) RemoveKey(keyForItem2);
            }
            base.SetItem(index, item);
        }
        
        #endregion

        #region Methods
        public TItem this[TKey key] {
            get {
                if (key == null) throw new ArgumentNullException(nameof(key));
                lock (SyncRoot) {
                    if (_dictionary != null) return _dictionary[key];
                    for (var index = 0; index < Items.Count; ++index) {
                        TItem obj = Items[index];
                        if (_comparer.Equals(key, GetKeyForItem(obj))) return obj;
                    }
                    throw (Exception)new KeyNotFoundException();
                }
            }
        }

        private void AddKey(TKey key, TItem item) {
            if (_dictionary != null) _dictionary.Add(key, item);
            else if (_keyCount == _threshold) {
                CreateDictionary();
                _dictionary.Add(key, item);
            } 
            else {
                if (Contains(key)) throw new ArgumentException("Cannot add two items with the same key to SynchronizedKeyedCollection.");
                ++_keyCount;
            }
        }

        protected void ChangeItemKey(TItem item, TKey newKey) {
            TKey k = ContainsItem(item) ? GetKeyForItem(item) : throw new ArgumentException("Item Does Not Exist In SynchronizedKeyedCollection.");
            if (_comparer.Equals(newKey, k)) return;
            if (newKey != null) AddKey(newKey, item);
            if (k == null) return;
            RemoveKey(k);
        }

        public bool Contains(TKey key) {
            if (key == null) throw new ArgumentNullException(nameof(key));
            lock (SyncRoot) {
                if (_dictionary != null) return _dictionary.ContainsKey(key);
                if (key != null) {
                    Items.Contains(items);
                    for (int index = 0; index < Items.Count; ++index) {
                        TItem obj = Items[index];
                        if (_comparer.Equals(key, GetKeyForItem(obj))) return true;
                    }
                }
                return false;
            }
        }

        private bool ContainsItem(TItem item) {
            TKey keyForItem;
            if (_dictionary == null || (keyForItem = GetKeyForItem(item)) == null)  return Items.Contains(item);
            return _dictionary.TryGetValue(keyForItem, out TItem y) && EqualityComparer<TItem>.Default.Equals(item, y);
        }

        private void CreateDictionary() {
            _dictionary = new System.Collections.Generic.Dictionary<TKey, TItem>(_comparer);
            foreach (TItem obj in Items) {
                TKey keyForItem = GetKeyForItem(obj);
                if (keyForItem != null) _dictionary.Add(keyForItem, obj);
            }
        }

        public bool Remove(TKey key) {
            if (key == null) throw new ArgumentNullException(nameof(key));
            lock (SyncRoot) {
                if (_dictionary != null) return _dictionary.ContainsKey(key) && Remove(_dictionary[key]);
                for (int index = 0; index < Items.Count; ++index) {
                    if (_comparer.Equals(key, GetKeyForItem(Items[index]))) {
                        RemoveItem(index);
                        return true;
                    }
                }
                return false;
            }
        }

        private void RemoveKey(TKey key) {
            if ( key == null) throw new ArgumentNullException(nameof(key));
            if (_dictionary != null) _dictionary.Remove(key);
            else --_keyCount;
        }
        #endregion
    }*/
}
