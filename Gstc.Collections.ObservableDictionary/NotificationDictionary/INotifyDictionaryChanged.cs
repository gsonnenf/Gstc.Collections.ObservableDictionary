using System.ComponentModel;

namespace Gstc.Collections.ObservableDictionary.NotificationDictionary {

    public interface INotifyDictionaryChanged : INotifyPropertyChanged {
        event NotifyDictionaryChangedEventHandler DictionaryChanged;
    }

    public delegate void NotifyDictionaryChangedEventHandler(object sender, NotifyDictionaryChangedEventArgs e);
}
