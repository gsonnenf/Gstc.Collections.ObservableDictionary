using Gstc.Collections.ObservableLists.ComponentModel;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Gstc.Collections.ObservableDictionary.CollectionView;
public interface IObservableListView<TKey, TValue, TOutput> :
    IEnumerable<TOutput>,
    INotifyCollectionChanging,
    INotifyCollectionChanged,
    INotifyListChangingEvents,
    INotifyListChangedEvents { }
