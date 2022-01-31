using Gstc.Collections.ObservableLists.Interface;

namespace Gstc.Collections.ObservableDictionary.Interface {
    public interface IObservableDictionaryCollection<TKey, TValue> :
        IObservableCollection,
        IObservableDictionary<TKey, TValue> {

    }
}
