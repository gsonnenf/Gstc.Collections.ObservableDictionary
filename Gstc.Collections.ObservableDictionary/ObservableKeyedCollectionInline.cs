using System;


namespace Gstc.Collections.ObservableDictionary {
    public class ObservableKeyedCollectionInline<TKey, TValue> : ObservableKeyedCollection<TKey, TValue> {

        public Func<TValue, TKey> GetKeyFunc;
        protected override TKey GetKeyForItem(TValue item) => GetKeyFunc(item);
        public ObservableKeyedCollectionInline(Func<TValue, TKey> getKeyFunc) : base() {
            GetKeyFunc = getKeyFunc;
        }
    }
}

