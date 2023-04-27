using Gstc.Collections.ObservableLists.ComponentModel;
using System.Collections.Specialized;

namespace Gstc.Collections.ObservableDictionary.ObservableList;
public interface IObservableListEnumerable<TOutput> :
    IObservableEnumerable<TOutput>,
    INotifyCollectionChanging,
    INotifyCollectionChanged,
    INotifyListChangingEvents,
    INotifyListChangedEvents { }
