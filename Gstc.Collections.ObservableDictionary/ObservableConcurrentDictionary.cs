using Gstc.Collections.ObservableDictionary.Abstract;
using Gstc.Collections.ObservableDictionary.Notify;
using System.Collections.Concurrent;

namespace Gstc.Collections.ObservableDictionary {
    public class ObservableConcurrentDictionary<TKey, TValue> :
        AbstractObservableIDictionary<TKey, TValue, ConcurrentDictionary<TKey, TValue>, NotifyDictionary> { }
}
