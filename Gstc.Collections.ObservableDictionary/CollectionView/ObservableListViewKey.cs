namespace Gstc.Collections.ObservableDictionary.CollectionView;
public class ObservableListViewKey<TKey, TValue> : ObservableListViewAbstract<TKey, TValue, TKey> {
    public ObservableListViewKey(IObservableDictionary<TKey, TValue> obvDict) : base(obvDict) { }
    public override TKey this[int index] => _orderedCollection[index].Key;
}
