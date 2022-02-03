using Gstc.Collections.ObservableDictionary.ComponentModel;
using Gstc.Collections.ObservableDictionary.Interface;
using Gstc.Collections.ObservableLists.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel;

namespace Gstc.Collections.ObservableDictionary.Abstract {
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
    public abstract class AbstractObservableIDictionary<TKey, TValue, TDictionary, TNotify> :
        AbstractDictionaryAdapter<TKey, TValue>,
        IObservableDictionary<TKey, TValue>
        where TDictionary : IDictionary<TKey, TValue>, new()
        where TNotify : INotifyDictionary, INotifyProperty, new() {

        #region Events
        public event PropertyChangedEventHandler PropertyChanged {
            add => Notify.PropertyChanged += value;
            remove => Notify.PropertyChanged -= value;
        }

        public event NotifyDictionaryChangedEventHandler DictionaryChanged {
            add => Notify.DictionaryChanged += value;
            remove => Notify.DictionaryChanged -= value;
        }
        #endregion

        #region Fields and Properties
        /// <summary>
        /// Notification handler for INotifyPropertyChanged and INotifyDictionaryChanged events and callbacks.
        /// </summary>
        protected readonly TNotify Notify = new();
        public TDictionary Dictionary {
            get => _dictionary;
            set {
                _dictionary = value;
                Notify.OnPropertyChangedCountAndIndex();
                Notify.OnDictionaryReset();
            }
        }
        private TDictionary _dictionary;
        #endregion

        #region Constructors
        protected AbstractObservableIDictionary() {
            Notify.Sender = this;
            _dictionary = new TDictionary();
        }
        protected AbstractObservableIDictionary(TDictionary dictionary) {
            Notify.Sender = this;
            _dictionary = dictionary;
        }
        #endregion

        #region Methods
        protected override IDictionary<TKey, TValue> InternalDictionary => Dictionary;

        public override TValue this[TKey key] {
            get => _dictionary[key];
            set {
                Notify.CheckReentrancy();
                if (!ContainsKey(key)) {
                    Add(key, value);
                    return;
                }
                var oldValue = _dictionary[key];
                var newValue = value;
                _dictionary[key] = newValue;
                Notify.OnPropertyChangedIndex();
                Notify.OnDictionaryReplace(key, oldValue, newValue);
            }
        }

        public override void Add(TKey key, TValue value) {
            //CheckReentrancy();
            _dictionary.Add(key, value);
            Notify.OnPropertyChangedCountAndIndex();
            Notify.OnDictionaryAdd(key, value);
        }

        public override void Clear() {
            //CheckReentrancy();
            _dictionary.Clear();
            Notify.OnPropertyChangedCountAndIndex();
            Notify.OnDictionaryReset();
        }

        public override bool Remove(TKey key) {
            //CheckReentrancy();
            var removedItem = _dictionary[key];
            if (!_dictionary.Remove(key)) return false;
            Notify.OnPropertyChangedCountAndIndex();
            Notify.OnDictionaryRemove(key, removedItem);
            return true;
        }
        #endregion
    }
}
