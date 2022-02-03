using System.ComponentModel;

namespace Gstc.Collections.ObservableDictionary.ComponentModel {

    public interface INotifyDictionaryChanged : INotifyPropertyChanged {
        event NotifyDictionaryChangedEventHandler DictionaryChanged;
    }

}
