using Gstc.Collections.ObservableLists;

namespace Gstc.Collections.ObservableDictionary {
    public interface IObservableDictionaryCollection<TKey, TValue> :
        IObservableDictionary<TKey, TValue>,
        IObservableCollection<TValue> { }
}
