using Gstc.Collections.ObservableLists;
using System;
using System.Collections.Generic;

namespace Gstc.Collections.ObservableDictionary.Binding;

/// <summary>
/// An <see cref="ObservableList{KeyValuePair{TKey, TValue}}"/> with an initialization binding to a 
/// <see cref="ObservableIDictionary{TKey, TValue, TDictionary}"/> using a <see cref="ObservableBindDictList{TKey, TValue}"/>.
/// 
/// The list maintains order based on order of additions. The move command can be used on elements to change order without
/// affecting the underlying dictionary.
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public class ObservableListDictSync<TKey, TValue> : ObservableList<KeyValuePair<TKey, TValue>>, IDisposable {

    private ObservableBindDictList<TKey, TValue> _obvDictListBind;

    public ObservableListDictSync(IObservableDictionary<TKey, TValue> obvDictionary) {
        _obvDictListBind = new ObservableBindDictList<TKey, TValue>(obvDictionary, this);
    }

    public void Dispose() {
        _obvDictListBind.Dispose();
        Clear();
    }
}
