using Gstc.Collections.ObservableDictionary.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Gstc.Collections.ObservableDictionary.CollectionView {

    /// <summary>
    /// This class serves an enumerable wrapper with <see cref="INotifyCollectionChanged"/> events for a <see cref="IObservableDictionary{TKey, TValue}"/>
    /// using the <see cref="KeyValuePair{TKey,TValue}"/> as its internal enumerator. A primary use case for this class is databinding in XAML.
    /// <br/><br/>
    /// The collection changed event is always called with NotifyCollectionChangedAction.Reset, because a dictionary does not have index information.
    /// Dispose may be called to unbind the enumerable from the ObservableDictionary when the enumberable is no longer needed.
    /// </summary>
    /// <typeparam name="TKey">The TKey of the dictionary and of the enumerable KeyValuePair{TKey,TValue} TItem type.</typeparam>
    /// <typeparam name="TValue">The TValue of the dictionary and of the enumerable KeyValuePair{TKey,TValue} TItem type.</typeparam>
    public class ObservableEnumerableKvp<TKey, TValue> : IObservableEnumerable<KeyValuePair<TKey, TValue>>, IDisposable {

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private IObservableDictionary<TKey, TValue> _obvDictionary;
        public ObservableEnumerableKvp(IObservableDictionary<TKey, TValue> obvDictionary) {
            _obvDictionary = obvDictionary;
            _obvDictionary.DictionaryChanged += DictionaryChanged;
        }
        ~ObservableEnumerableKvp() => Dispose();

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
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _obvDictionary.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _obvDictionary.GetEnumerator();
    }
}
