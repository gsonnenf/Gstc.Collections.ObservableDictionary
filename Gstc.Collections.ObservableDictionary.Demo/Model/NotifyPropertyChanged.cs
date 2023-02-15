using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Gstc.Collections.ObservableDictionary.Demo.Model;
public class NotifyPropertyChanged : INotifyPropertyChanged {
    #region Notify
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(object sender, PropertyChangedEventArgs args) => PropertyChanged?.Invoke(this, args);
    protected void OnPropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    #endregion
}
