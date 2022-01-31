namespace Gstc.Collections.ObservableDictionary.Base.Notify {

    public interface INotifyDictionaryChanged {
        event NotifyDictionaryChangedEventHandler DictionaryChanged;
    }

    public delegate void NotifyDictionaryChangedEventHandler(object sender, NotifyDictionaryChangedEventArgs e);
}
