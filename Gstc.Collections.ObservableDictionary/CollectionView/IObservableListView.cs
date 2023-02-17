using Gstc.Collections.ObservableLists.ComponentModel;
using System.Collections.Specialized;

namespace Gstc.Collections.ObservableDictionary.CollectionView;
public interface IObservableListView<TOutput> :
    IObservableEnumerable<TOutput>,
    INotifyCollectionChanging,
    INotifyCollectionChanged,
    INotifyListChangingEvents,
    INotifyListChangedEvents { }
