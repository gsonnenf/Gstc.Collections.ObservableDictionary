using Gstc.Collections.ObservableDictionary.Base.Notify;
using Gstc.Collections.ObservableLists.Base.Notify;
using System.Collections.Generic;
using System.ComponentModel;

namespace Gstc.Collections.ObservableDictionary.Base {
    /// <summary>
    /// Observable dictionary can serve as a stand alone dictionary, or serve as an observable wrapper for a pre-existing dictionary.
    /// ObservableDictionary implements INotifyPropertyChanged and INotifyDictionaryChanged. It does NOT implement ICollectionChanged.
    /// Use an ObservableKeyedCollection or SortedList if you need ICollectionChanged.
    /// 
    /// </summary>
    /// <typeparam name="TKey">Key field of Dictionary</typeparam>
    /// <typeparam name="TValue">Value field of Dictionary</typeparam>
    /// <typeparam name="TDictionary"></typeparam>
    public abstract class AbstractObservableIDictionary<TDictionary, TKey, TValue> :
        BaseObservableDictionary<TKey, TValue>
        where TDictionary : IDictionary<TKey, TValue>, new() {

        #region Events
        public override event PropertyChangedEventHandler PropertyChanged {
            add => NotifyProperty.PropertyChanged += value;
            remove => NotifyProperty.PropertyChanged -= value;
        }

        public override event NotifyDictionaryChangedEventHandler DictionaryChanged {
            add => NotifyDictionary.DictionaryChanged += value;
            remove => NotifyDictionary.DictionaryChanged -= value;
        }
        #endregion

        #region Fields and Properties
        public NotifyPropertyComposition NotifyProperty { get; protected set; }
        public NotifyDictionaryComposition<AbstractObservableIDictionary<TDictionary, TKey, TValue>> NotifyDictionary { get; protected set; }
        public TDictionary Dictionary {
            get => _dictionary;
            set {
                _dictionary = value;
                NotifyProperty.OnPropertyChangedCountAndIndex();
                NotifyDictionary.OnDictionaryReset();
            }
        }
        private TDictionary _dictionary;
        #endregion

        #region Constructors
        protected AbstractObservableIDictionary() {
            _dictionary = new TDictionary();
        }
        protected AbstractObservableIDictionary(TDictionary dictionary) {
            _dictionary = dictionary;
        }
        #endregion

        #region Methods
        protected override IDictionary<TKey, TValue> InternalDictionary => Dictionary;

        public override TValue this[TKey key] {
            get => _dictionary[key];
            set {
                //CheckReentrancy();
                if (!ContainsKey(key)) {
                    Add(key, value);
                    return;
                }
                var oldValue = _dictionary[key];
                var newValue = value;
                _dictionary[key] = newValue;
                NotifyProperty.OnPropertyChangedIndex();
                NotifyDictionary.OnDictionaryReplace(key, oldValue, newValue);
            }
        }

        public override void Add(TKey key, TValue value) {
            //CheckReentrancy();
            _dictionary.Add(key, value);
            NotifyProperty.OnPropertyChangedCountAndIndex();
            NotifyDictionary.OnDictionaryAdd(key, value);
        }

        public override void Clear() {
            //CheckReentrancy();
            _dictionary.Clear();
            NotifyProperty.OnPropertyChangedCountAndIndex();
            NotifyDictionary.OnDictionaryReset();
        }

        public override bool Remove(TKey key) {
            //CheckReentrancy();
            var removedItem = _dictionary[key];
            if (!_dictionary.Remove(key)) return false;
            NotifyProperty.OnPropertyChangedCountAndIndex();
            NotifyDictionary.OnDictionaryRemove(key, removedItem);
            return true;
        }
        #endregion
    }
}
