using System.Collections;
using System.Collections.Generic;

namespace Gstc.Collections.ObservableDictionary.CollectionView {
    public class EnumeratorKvpToKey<TKey, TValue> : IEnumerator<TKey>, IEnumerable<TKey> {
        public IEnumerator<KeyValuePair<TKey, TValue>> EnumeratorKvp;
        public EnumeratorKvpToKey(IEnumerator<KeyValuePair<TKey, TValue>> enumeratorKvp) => EnumeratorKvp = enumeratorKvp;
        public EnumeratorKvpToKey(IEnumerable<KeyValuePair<TKey, TValue>> enumerableKvp) => EnumeratorKvp = enumerableKvp.GetEnumerator();
        public TKey Current => EnumeratorKvp.Current.Key;
        object IEnumerator.Current => EnumeratorKvp.Current.Key;
        public void Dispose() => EnumeratorKvp.Dispose();

        public bool MoveNext() => EnumeratorKvp.MoveNext();
        public void Reset() => EnumeratorKvp.MoveNext();

        public IEnumerator<TKey> GetEnumerator() => this;
        IEnumerator IEnumerable.GetEnumerator() => this;
    }
}
