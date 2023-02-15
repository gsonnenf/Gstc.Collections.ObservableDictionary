using Gstc.Collections.ObservableDictionary.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Gstc.Collections.ObservableDictionary.CollectionView {
    /// <summary>
    /// <inheritdoc cref="ObservableEnumerableKvp{TKey, TValue}"/>
    /// <br/><br/>
    /// The enumerable is of type TKey.
    /// </summary>
    /// <typeparam name="TKey">They TKey of the dictionary and the TItem of the enumerator.</typeparam>
    /// <typeparam name="TValue">The TValue of the dictionary.</typeparam>
    public class ObservableEnumerableKey<TKey, TValue> : IObservableEnumerable<TKey>, IDisposable {

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private IObservableDictionary<TKey, TValue> _obvDictionary;
        public ObservableEnumerableKey(IObservableDictionary<TKey, TValue> obvDictionary) {
            _obvDictionary = obvDictionary;
            _obvDictionary.DictionaryChanged += DictionaryChanged;
        }
        ~ObservableEnumerableKey() => Dispose();

        /// <summary>
        /// When dictionary changes, calls a collection change event. A reset event is always used because index information is not available.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DictionaryChanged(object sender, INotifyDictionaryChangedEventArgs<TKey, TValue> e)
            => CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        public void Dispose() {
            _obvDictionary.DictionaryChanged -= DictionaryChanged;
            _obvDictionary = null;
        }
        public IEnumerator<TKey> GetEnumerator() => _obvDictionary.Keys.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _obvDictionary.Keys.GetEnumerator();
    }
}
