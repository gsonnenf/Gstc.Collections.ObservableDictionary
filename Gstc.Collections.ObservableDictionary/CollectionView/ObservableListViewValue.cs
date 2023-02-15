namespace Gstc.Collections.ObservableDictionary.CollectionView {
    public class ObservableListViewValue<TKey, TValue> : AbstractObservableListView<TKey, TValue, TValue> {
        public ObservableListViewValue(IObservableDictionary<TKey, TValue> obvDict) : base(obvDict) { }
        public override TValue this[int index] => _orderedCollection[index].Value;
    }
}
