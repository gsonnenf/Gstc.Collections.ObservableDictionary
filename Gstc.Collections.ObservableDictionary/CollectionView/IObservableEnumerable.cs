using System.Collections.Generic;
using System.Collections.Specialized;

namespace Gstc.Collections.ObservableDictionary.CollectionView;

//todo: move to ObservableList
public interface IObservableEnumerable<TItem> : IEnumerable<TItem>, INotifyCollectionChanged { }
