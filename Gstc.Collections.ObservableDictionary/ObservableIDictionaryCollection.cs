//Todo: Add insert to a non-generic ObservableIDictionaryCollection that is not sorted.
namespace Gstc.Collections.ObservableDictionary {
    /*
    public class ObservableIDictionaryCollection<TKey, TValue, TDictionary>
        : AbstractDictionaryUpcast<TKey, TValue>,
        IObservableDictionary<TKey, TValue>
        where TDictionary : IDictionary<TKey, TValue>, IDictionary, new() {

        #region Events Dictionary Changing
        public event NotifyDictionaryChangingEventHandler<TKey, TValue> DictionaryChanging;
        public event NotifyDictAddEventHander<TKey, TValue> AddingKvp;
        public event NotifyDictRemoveEventHander<TKey, TValue> RemovingKvp;
        public event NotifyDictReplaceEventHander<TKey, TValue> ReplacingKvp;
        public event NotifyDictResetEventHander<TKey, TValue> ResettingKvp;
        #endregion

        #region Events Dictionary Changed
        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyDictionaryChangedEventHandler<TKey, TValue> DictionaryChanged;
        public event NotifyDictAddEventHander<TKey, TValue> AddedDKvp;
        public event NotifyDictRemoveEventHander<TKey, TValue> RemovedKvp;
        public event NotifyDictReplaceEventHander<TKey, TValue> ReplacedKvp;
        public event NotifyDictResetEventHander<TKey, TValue> ResetKvp;
        #endregion

        #region Fields and Properties
        protected TDictionary _dictionary;
        protected override IDictionary<TKey, TValue> _internalDictionaryKv => _dictionary;
        protected override IDictionary _InternalDictionary => _dictionary;

        public IObservableCollectionView<TKey, TValue> CollectionView { get; protected set; }
        /// <summary>
        /// Notification handler for INotifyPropertyChanged and INotifyDictionaryChanged events and callbacks.
        /// </summary>
        public TDictionary Dictionary {
            get => _dictionary;
            set {
                using (BlockReentrancy()) {
                    var eventArgs = new DictResetEventArgs<TKey, TValue>();
                    DictionaryChanging?.Invoke(this, eventArgs);
                    ResettingKvp?.Invoke(this, eventArgs);

                    _dictionary = value;

                    OnPropertyChangedCountAndIndex();
                    DictionaryChanged?.Invoke(this, eventArgs);
                    ResetKvp?.Invoke(this, eventArgs);
                    CollectionView.OnCollectionChanged_Reset();
                }
            }
        }
        #endregion

        #region Constructors
        public ObservableIDictionaryCollection() {
            _dictionary = new TDictionary();
            CollectionView = new ObservableCollectionView<TKey, TValue>(this);
        }

        public ObservableIDictionaryCollection(TDictionary dictionary) {
            _dictionary = dictionary;
            CollectionView = new ObservableCollectionView<TKey, TValue>(this);
        }
        #endregion

        #region Methods
        public override TValue this[TKey key] {
            get => _dictionary[key];
            set {
                if (!ContainsKey(key)) {
                    Add(key, value);
                    return;
                }
                using (BlockReentrancy()) {
                    var oldValue = _dictionary[key];
                    var newValue = value;

                    var eventArgs = new DictReplaceEventArgs<TKey, TValue>(key, oldValue, newValue);
                    DictionaryChanging?.Invoke(this, eventArgs);
                    ReplacingKvp?.Invoke(this, eventArgs);

                    _dictionary[key] = newValue;

                    OnPropertyChangedIndex();
                    DictionaryChanged?.Invoke(this, eventArgs);
                    ReplacedKvp?.Invoke(this, eventArgs);
                    CollectionView.OnCollectionChanged_Replace(key, oldValue, newValue);
                }
            }
        }

        public override void Add(TKey key, TValue value) {
            using (BlockReentrancy()) {
                var eventArgs = new DictAddEventArgs<TKey, TValue>(key, value);
                DictionaryChanging?.Invoke(this, eventArgs);
                AddingKvp?.Invoke(this, eventArgs);

                _dictionary.Add(key, value);

                OnPropertyChangedCountAndIndex();
                DictionaryChanged?.Invoke(this, eventArgs);
                AddedDKvp?.Invoke(this, eventArgs);
                CollectionView.OnCollectionChanged_Add(key, value);
            }
        }

        public override void Clear() {
            using (BlockReentrancy()) {
                var eventArgs = new DictResetEventArgs<TKey, TValue>();
                DictionaryChanging?.Invoke(this, eventArgs);
                ResettingKvp?.Invoke(this, eventArgs);

                _internalDictionaryKv.Clear();

                OnPropertyChangedCountAndIndex();
                DictionaryChanged?.Invoke(this, eventArgs);
                ResetKvp?.Invoke(this, eventArgs);
                CollectionView.OnCollectionChanged_Clear();
            }
        }

        public void RefreshAll() {
            using (BlockReentrancy()) {
                var eventArgs = new DictResetEventArgs<TKey, TValue>();
                DictionaryChanging?.Invoke(this, eventArgs);
                ResettingKvp?.Invoke(this, eventArgs);

                OnPropertyChangedCountAndIndex();
                DictionaryChanged?.Invoke(this, eventArgs);
                ResetKvp?.Invoke(this, eventArgs);
                CollectionView.OnCollectionChanged_Clear();
            }
        }

        public void RefreshKey(TKey key) {
            using (BlockReentrancy()) {
                var oldValue = _dictionary[key];
                var eventArgs = new DictReplaceEventArgs<TKey, TValue>(key, oldValue, oldValue);
                DictionaryChanging?.Invoke(this, eventArgs);
                ReplacingKvp?.Invoke(this, eventArgs);

                OnPropertyChangedIndex();
                DictionaryChanged?.Invoke(this, eventArgs);
                ReplacedKvp?.Invoke(this, eventArgs);
                CollectionView.OnCollectionChanged_Replace(key, oldValue, oldValue);
            }
        }

        public override bool Remove(TKey key) {
            using (BlockReentrancy()) {
                var oldValue = _dictionary[key];

                var eventArgs = new DictRemoveEventArgs<TKey, TValue>(key, oldValue);
                DictionaryChanging?.Invoke(this, eventArgs);
                RemovingKvp?.Invoke(this, eventArgs);
                CollectionView.OnCollectionChanged_Removing(key, oldValue);

                if (!_dictionary.Remove(key)) return false;

                OnPropertyChangedCountAndIndex();
                DictionaryChanged?.Invoke(this, eventArgs);
                RemovedKvp?.Invoke(this, eventArgs);
                CollectionView.OnCollectionChanged_Remove(key, oldValue);
                return true;
            }
        }
        #endregion

        #region Property Notify Methods
        protected const string CountString = "Count";
        protected const string IndexerName = "Item[]";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnPropertyChangedIndex() => OnPropertyChanged(IndexerName);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnPropertyChangedCountAndIndex() {
            OnPropertyChanged(CountString);
            OnPropertyChanged(IndexerName);
        }
        #endregion

        #region Reentrancy Monitor

        /// <summary>
        /// Allows onChange events reentrancy when set to true. Be careful when allowing reentrancy, as it can cause stack overflow
        /// from infinite calls due to conflicting callbacks.
        /// </summary>
        private SimpleMonitor ReentrancyMonitor => _monitor ??= new SimpleMonitor();
        private SimpleMonitor _monitor;

        public bool AllowReentrancy {
            get => ReentrancyMonitor.AllowReentrancy;
            set => ReentrancyMonitor.AllowReentrancy = value;
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal SimpleMonitor BlockReentrancy() => ReentrancyMonitor.BlockReentrancy();

        #endregion
    }
    */

}

