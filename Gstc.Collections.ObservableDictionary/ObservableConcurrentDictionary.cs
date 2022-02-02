using Gstc.Collections.ObservableDictionary.Base;
using System.Collections.Concurrent;

namespace Gstc.Collections.ObservableDictionary {
    public class ObservableConcurrentDictionary<TKey, TValue> :
        AbstractObservableIDictionary<ConcurrentDictionary<TKey, TValue>, TKey, TValue> { }
}
