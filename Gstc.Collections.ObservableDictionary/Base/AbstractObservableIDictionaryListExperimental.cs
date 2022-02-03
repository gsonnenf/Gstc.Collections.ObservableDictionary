using Gstc.Collections.ObservableDictionary.NotificationCollectionDictionary.Gstc.Collections.ObservableDictionary.Notification;
using Gstc.Collections.ObservableDictionary.NotificationDictionary;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

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
    public abstract class AbstractObservableIDictionaryListExperimental<TDictionary, TKey, TValue> :
        BaseObservableDictionary<TKey, TValue>,
        INotifyCollectionChanged
        where TDictionary : IDictionary<TKey, TValue>, new() {

        #region Events
        public override event PropertyChangedEventHandler PropertyChanged {
            add => Notify.PropertyChanged += value;
            remove => Notify.PropertyChanged -= value;
        }

        public override event NotifyDictionaryChangedEventHandler DictionaryChanged {
            add => Notify.DictionaryChanged += value;
            remove => Notify.DictionaryChanged -= value;
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged {
            add => Notify.CollectionChanged += value;
            remove => Notify.CollectionChanged -= value;
        }
        #endregion

        #region Fields and Properties


        public NotifyDictionaryCollectionComposition<AbstractObservableIDictionaryListExperimental<TDictionary, TKey, TValue>> Notify { get; protected set; }
        public TDictionary Dictionary {
            get => _dictionary;
            set {
                _dictionary = value;

                Notify.OnPropertyChangedCountAndIndex();
                Notify.OnDictionaryReset();
                Notify.OnCollectionChangedReset();
            }
        }
        private TDictionary _dictionary;
        #endregion

        #region Constructors
        protected AbstractObservableIDictionaryListExperimental() {
            Notify = new(this);
            _dictionary = new TDictionary();
        }
        protected AbstractObservableIDictionaryListExperimental(TDictionary dictionary) {
            Notify = new(this);
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
                var index = _dictionary.Values.ToList().IndexOf(oldValue);

                Notify.OnPropertyChangedIndex();
                Notify.OnDictionaryReplace(key, oldValue, newValue);
                Notify.OnCollectionChangedReplace(oldValue, newValue, index);
            }
        }

        public override void Add(TKey key, TValue value) {
            //CheckReentrancy();
            _dictionary.Add(key, value);
            Notify.OnPropertyChangedCountAndIndex();
            Notify.OnDictionaryAdd(key, value);
            var index = _dictionary.Values.ToList().IndexOf(value);
            Notify.OnCollectionChangedAdd(value, index);
        }

        public override void Clear() {
            //CheckReentrancy();
            _dictionary.Clear();
            Notify.OnPropertyChangedCountAndIndex();
            Notify.OnDictionaryReset();
            Notify.OnCollectionChangedReset();
        }

        public override bool Remove(TKey key) {
            //CheckReentrancy();
            var removedItem = _dictionary[key];
            var index = _dictionary.Values.ToList().IndexOf(removedItem);
            if (!_dictionary.Remove(key)) return false;
            Notify.OnPropertyChangedCountAndIndex();
            Notify.OnDictionaryRemove(key, removedItem);
            Notify.OnCollectionChangedRemove(removedItem, index);
            return true;
        }
        #endregion
    }
}
