namespace Gstc.Collections.ObservableDictionary.CollectionView;
public class ObservableListViewKey<TKey, TValue> : AbstractObservableListView<TKey, TValue, TKey> {
    public ObservableListViewKey(IObservableDictionary<TKey, TValue> obvDict) : base(obvDict) { }
    public override TKey this[int index] => _orderedCollection[index].Key;
}
