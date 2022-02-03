using Gstc.Collections.ObservableLists.ComponentModel;

namespace Gstc.Collections.ObservableDictionary.ComponentModel {
    public interface INotifyDictionary : INotifyProperty, INotifyDictionaryChanged {
        void OnDictionaryReset();
        void OnDictionaryAdd(object key, object item);
        void OnDictionaryRemove(object key, object item);
        void OnDictionaryReplace(object key, object oldItem, object newItem);
    }
}
