using Gstc.Collections.ObservableDictionary.ComponentModel;
using Gstc.Collections.ObservableLists;
using System.Collections.Generic;
using System.Linq;

namespace Gstc.Collections.ObservableDictionary.Binding;
public class ObservableDictListBind<TKey, TValue> {
    IObservableDictionary<TKey, TValue> _obvDict;
    ObservableList<KeyValuePair<TKey, TValue>> _obvListKvp;

    public void OneWayBind() {
        _obvListKvp.List = _obvDict.ToList();
        _obvDict.AddedKvp += _obvDict_AddingKvp;
        _obvDict.RemovedKvp += _obvDict_RemovedKvp;
        _obvDict.ReplacedKvp += _obvDict_ReplacedKvp;
        _obvDict.ResetKvp += _obvDict_ResetKvp;
    }

    #region Binding Event Handlers
    private void _obvDict_AddingKvp(object sender, DictAddEventArgs<TKey, TValue> args)
        => _obvDict.Add(new KeyValuePair<TKey, TValue>(args.Key, args.NewValue));
    private void _obvDict_RemovedKvp(object sender, DictRemoveEventArgs<TKey, TValue> args)
        => _obvListKvp.RemoveAt(_obvListKvp.List.FindIndex(item => item.Key.Equals(args.Key))); //Notes: Benchmark shows find index is fast.
    private void _obvDict_ReplacedKvp(object sender, DictReplaceEventArgs<TKey, TValue> args)
        => _obvListKvp[_obvListKvp.List.FindIndex(item => item.Key.Equals(args.Key))]
            = new KeyValuePair<TKey, TValue>(args.Key, args.NewValue);
    private void _obvDict_ResetKvp(object sender, DictResetEventArgs<TKey, TValue> args)
        => _obvListKvp.List = _obvDict.ToList();
    #endregion




}