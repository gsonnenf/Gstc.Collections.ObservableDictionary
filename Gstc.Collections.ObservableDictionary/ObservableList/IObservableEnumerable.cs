using System.Collections.Generic;
using System.Collections.Specialized;

namespace Gstc.Collections.ObservableDictionary.ObservableList;

//todo: move to ObservableList
public interface IObservableEnumerable<TItem> : IEnumerable<TItem>, INotifyCollectionChanged { }
