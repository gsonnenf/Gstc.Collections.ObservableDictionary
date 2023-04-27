using System.Collections.Generic;

namespace Gstc.Collections.ObservableDictionary.CollectionView;
public class ObservableListViewKvp<TKey, TValue> : ObservableListViewAbstract<TKey, TValue, KeyValuePair<TKey, TValue>> {
    public ObservableListViewKvp(IObservableDictionary<TKey, TValue> obvDict) : base(obvDict) { }
    public override KeyValuePair<TKey, TValue> this[int index] => _orderedCollection[index];
}
