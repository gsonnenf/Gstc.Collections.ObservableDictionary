using Gstc.Collections.ObservableDictionary.Base;
using System.Collections.Concurrent;

namespace Gstc.Collections.ObservableDictionary {
    internal class ObservableConcurrentDictionary<TKey, TValue> :
        AbstractObservableIDictionary<ConcurrentDictionary<TKey, TValue>, TKey, TValue> {
    }
}
