using Gstc.Collections.ObservableDictionary.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Gstc.Collections.ObservableDictionary.CollectionView {

    /// <summary>
    /// <inheritdoc cref="ObservableEnumerableKvp{TKey, TValue}"/>
    /// <br/><br/>
    /// The enumerable is of type TValue.
    /// </summary>
    /// <typeparam name="TKey">They TKey of the dictionary.</typeparam>
    /// <typeparam name="TValue">The TValue of the dictionary and the TItem of the enumerator.</typeparam>
    public class ObservableEnumerableValue<TKey, TValue> : IObservableEnumerable<TValue>, IDisposable {

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private IObservableDictionary<TKey, TValue> _obvDictionary;
        public ObservableEnumerableValue(IObservableDictionary<TKey, TValue> obvDictionary) {
            _obvDictionary = obvDictionary;
            _obvDictionary.DictionaryChanged += DictionaryChanged;
        }
        ~ObservableEnumerableValue() => Dispose();

        /// <summary>
        /// When dictionary changes, calls a collection change event. A reset event is always used because index information is not available.
        /// </summary>
        private void DictionaryChanged(object sender, INotifyDictionaryChangedEventArgs<TKey, TValue> e)
            => CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        public void Dispose() {
            _obvDictionary.DictionaryChanged -= DictionaryChanged;
            _obvDictionary = null;
        }
        public IEnumerator<TValue> GetEnumerator() => _obvDictionary.Values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _obvDictionary.Values.GetEnumerator();


    }
}
