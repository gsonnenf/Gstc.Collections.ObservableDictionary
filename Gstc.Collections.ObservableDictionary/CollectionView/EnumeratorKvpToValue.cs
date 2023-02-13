using System.Collections;
using System.Collections.Generic;

namespace Gstc.Collections.ObservableDictionary.CollectionView {
    public class EnumeratorKvpToValue<TKey, TValue> : IEnumerator<TValue> {
        public IEnumerator<KeyValuePair<TKey, TValue>> EnumeratorKvp;
        public EnumeratorKvpToValue(IEnumerator<KeyValuePair<TKey, TValue>> enumeratorKvp) => EnumeratorKvp = enumeratorKvp;
        public TValue Current => EnumeratorKvp.Current.Value;
        object IEnumerator.Current => EnumeratorKvp.Current.Value;
        public void Dispose() => EnumeratorKvp.Dispose();
        public bool MoveNext() => EnumeratorKvp.MoveNext();
        public void Reset() => EnumeratorKvp.MoveNext();
    }
}
