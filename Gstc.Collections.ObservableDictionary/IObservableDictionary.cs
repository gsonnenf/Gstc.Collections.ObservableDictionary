using Gstc.Collections.ObservableDictionary.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel;

namespace Gstc.Collections.ObservableDictionary {
    public interface IObservableDictionary<TKey, TValue> :
        IDictionary<TKey, TValue>,
        INotifyDictionaryChanging<TKey, TValue>,
        INotifyDictionaryChanged<TKey, TValue>,
        INotifyPropertyChanged {

        public void RefreshKey(TKey key);
        public void RefreshAll();
    }
}
