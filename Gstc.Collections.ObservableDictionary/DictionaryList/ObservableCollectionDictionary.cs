using Gstc.Collections.ObservableLists;

namespace Gstc.Collections.ObservableDictionary.DictionaryList {
    public class ObservableCollectionDictionary<TKey, TValue> : ObservableList<TKey> {

        private IObservableDictionary<Tkey, TValue>
        private ObservableDictListBind _obvBind;
        public ObservableCollectionDictionary(IObservableDictionary) {
            _obvBind = new ObservableDictListBind(
                ObvDict:


                );
        }



    }
}
