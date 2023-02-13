using Gstc.Collections.ObservableDictionary.Abstract;
using Gstc.Collections.ObservableDictionary.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Gstc.Collections.ObservableDictionary {
    /// <summary>
    /// Observable dictionary can serve as a stand alone dictionary, or serve as an observable wrapper for a pre-existing dictionary.
    /// ObservableDictionary implements INotifyPropertyChanged and INotifyDictionaryChanged. It does NOT implement ICollectionChanged.
    /// Use an ObservableKeyedCollection or SortedList if you need ICollectionChanged.
    /// 
    /// </summary>
    /// <typeparam name="TKey">Key field of Dictionary</typeparam>
    /// <typeparam name="TValue">Value field of Dictionary</typeparam>
    /// <typeparam name="TDictionary"></typeparam>
    /// <typeparam name="TNotify"></typeparam>
    public class ObservableIDictionary<TKey, TValue, TDictionary> :
        AbstractDictionaryUpcast<TKey, TValue>,
        IObservableDictionary<TKey, TValue>
        where TDictionary : IDictionary<TKey, TValue>, IDictionary, new() {

        #region Events Dictionary Changing
        public event NotifyDictionaryChangingEventHandler<TKey, TValue> DictionaryChanging;
        public event NotifyDictAddEventHander<TKey, TValue> AddingDict;
        public event NotifyDictRemoveEventHander<TKey, TValue> RemovingDict;
        public event NotifyDictReplaceEventHander<TKey, TValue> ReplacingDict;
        public event NotifyDictResetEventHander<TKey, TValue> ResetingDict;
        #endregion

        #region Events Dictionary Changed
        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyDictionaryChangedEventHandler<TKey, TValue> DictionaryChanged;
        public event NotifyDictAddEventHander<TKey, TValue> AddedDict;
        public event NotifyDictRemoveEventHander<TKey, TValue> RemovedDict;
        public event NotifyDictReplaceEventHander<TKey, TValue> ReplacedDict;
        public event NotifyDictResetEventHander<TKey, TValue> ResetDict;
        #endregion

        #region Fields and Properties

        protected TDictionary _dictionary;
        protected override IDictionary<TKey, TValue> _internalDictionaryKv => _dictionary;
        protected override IDictionary _InternalDictionary => _dictionary;
        /// <summary>
        /// Notification handler for INotifyPropertyChanged and INotifyDictionaryChanged events and callbacks.
        /// </summary>
        public TDictionary Dictionary {
            get => _dictionary;
            set {
                using (BlockReentrancy()) {
                    var eventArgs = new DictResetEventArgs<TKey, TValue>();
                    DictionaryChanging?.Invoke(this, eventArgs);
                    ResetingDict?.Invoke(this, eventArgs);

                    _dictionary = value;

                    OnPropertyChangedCountAndIndex();
                    DictionaryChanged?.Invoke(this, eventArgs);
                    ResetDict?.Invoke(this, eventArgs);
                }
            }
        }
        #endregion

        #region Constructors
        protected ObservableIDictionary() => _dictionary = new TDictionary();
        protected ObservableIDictionary(TDictionary dictionary) => _dictionary = dictionary;
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
                    ReplacingDict?.Invoke(this, eventArgs);

                    _dictionary[key] = newValue;

                    OnPropertyChangedIndex();
                    DictionaryChanged?.Invoke(this, eventArgs);
                    ReplacedDict?.Invoke(this, eventArgs);
                }
            }
        }

        public override void Add(TKey key, TValue value) {
            using (BlockReentrancy()) {
                var eventArgs = new DictAddEventArgs<TKey, TValue>(key, value);
                DictionaryChanging?.Invoke(this, eventArgs);
                AddingDict?.Invoke(this, eventArgs);

                _dictionary.Add(key, value);

                OnPropertyChangedCountAndIndex();
                DictionaryChanged?.Invoke(this, eventArgs);
                AddedDict?.Invoke(this, eventArgs);
            }
        }

        public override void Clear() {
            using (BlockReentrancy()) {
                var eventArgs = new DictResetEventArgs<TKey, TValue>();
                DictionaryChanging?.Invoke(this, eventArgs);
                ResetingDict?.Invoke(this, eventArgs);

                _internalDictionaryKv.Clear(); //Fixes ambiguity between IDictionary<> and IDictionary

                OnPropertyChangedCountAndIndex();
                DictionaryChanged?.Invoke(this, eventArgs);
                ResetDict?.Invoke(this, eventArgs);
            }
        }

        public void RefreshAll() {
            using (BlockReentrancy()) {
                var eventArgs = new DictResetEventArgs<TKey, TValue>();
                DictionaryChanging?.Invoke(this, eventArgs);
                ResetingDict?.Invoke(this, eventArgs);

                OnPropertyChangedCountAndIndex();
                DictionaryChanged?.Invoke(this, eventArgs);
                ResetDict?.Invoke(this, eventArgs);
            }
        }

        public void RefreshKey(TKey key) {
            using (BlockReentrancy()) {
                var oldValue = _dictionary[key];
                var eventArgs = new DictReplaceEventArgs<TKey, TValue>(key, oldValue, oldValue);
                DictionaryChanging?.Invoke(this, eventArgs);
                ReplacingDict?.Invoke(this, eventArgs);

                OnPropertyChangedIndex();
                DictionaryChanged?.Invoke(this, eventArgs);
                ReplacedDict?.Invoke(this, eventArgs);
            }
        }

        public override bool Remove(TKey key) {
            using (BlockReentrancy()) {
                var oldValue = _dictionary[key];
                var eventArgs = new DictRemoveEventArgs<TKey, TValue>(key, oldValue);
                DictionaryChanging?.Invoke(this, eventArgs);
                RemovingDict?.Invoke(this, eventArgs);

                if (!_dictionary.Remove(key)) return false;

                OnPropertyChangedCountAndIndex();
                DictionaryChanged?.Invoke(this, eventArgs);
                RemovedDict?.Invoke(this, eventArgs);
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private SimpleMonitor BlockReentrancy() => ReentrancyMonitor.BlockReentrancy();



        #endregion
    }
}
