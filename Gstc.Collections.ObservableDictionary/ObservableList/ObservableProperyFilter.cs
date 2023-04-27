using System;
using System.ComponentModel;

namespace Gstc.Collections.ObservableDictionary.ObservableList;
//Todo: Figure out if this is good, and where it should go
public class ObservablePropertyFilter<TInput, TOutput> : INotifyPropertyChanged {
    public event PropertyChangedEventHandler PropertyChanged;
    private Func<TInput, TOutput> _convert;
    private TInput _input;

    public TInput Input {
        get => _input;
        set {
            _input = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Output)));
        }
    }
    public TOutput? Output => _convert.Invoke(_input);

    public ObservablePropertyFilter(Func<TInput, TOutput> convert) => _convert = convert;
}
