using Gstc.Collections.ObservableDictionary.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Gstc.Collections.ObservableDictionary.CollectionView {
    public class AbstractObservableEnumerable<TKey, TValue, TOutput> : IObservableEnumerable<TOutput>, IDisposable {

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private IObservableDictionary<TKey, TValue> _obvDictionary;
        public AbstractObservableEnumerable(IObservableDictionary<TKey, TValue> obvDictionary) {
            _obvDictionary = obvDictionary;
            _obvDictionary.DictionaryChanged += DictionaryChanged;
        }
        ~AbstractObservableEnumerable() => Dispose();

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

        public virtual IEnumerator<TOutput> GetEnumerator() {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }
}
