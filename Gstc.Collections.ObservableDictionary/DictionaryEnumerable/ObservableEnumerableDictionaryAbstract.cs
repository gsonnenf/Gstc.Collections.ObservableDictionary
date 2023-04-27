using Gstc.Collections.ObservableDictionary.ComponentModel;
using Gstc.Collections.ObservableDictionary.ObservableList;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Gstc.Collections.ObservableDictionary.DictionaryEnumerable;
/// <summary>
/// This class serves an enumerable wrapper with <see cref="INotifyCollectionChanged"/> events for a <see cref="IObservableDictionary{TKey, TValue}"/>
/// using the <see cref="KeyValuePair{TKey,TValue}"/> as its internal enumerator. A primary use case for this class is XAML databinding.
/// <br/><br/>
/// The collection changed event is always called with NotifyCollectionChangedAction.Reset, because a dictionary does not have index information.
/// Order is not guarenteed between iterations. Dispose may be called to unbind the enumerable from the ObservableDictionary when the enumberable is no longer needed.
/// </summary>
/// <typeparam name="TKey">The TKey of the dictionary and of the enumerable KeyValuePair{TKey,TValue} TItem type.</typeparam>
/// <typeparam name="TValue">The TValue of the dictionary and of the enumerable KeyValuePair{TKey,TValue} TItem type.</typeparam>
public abstract class ObservableEnumerableDictionaryAbstract<TKey, TValue, TOutput> : IObservableEnumerable<TOutput>, IDisposable {

    #region Members
    public event NotifyCollectionChangedEventHandler CollectionChanged;
    protected IObservableDictionary<TKey, TValue> _obvDictionary;
    #endregion

    #region Ctor
    public ObservableEnumerableDictionaryAbstract(IObservableDictionary<TKey, TValue> obvDictionary) {
        _obvDictionary = obvDictionary;
        _obvDictionary.DictionaryChanged += DictionaryChanged;
    }

    ~ObservableEnumerableDictionaryAbstract() => Dispose();

    public void Dispose() {
        _obvDictionary.DictionaryChanged -= DictionaryChanged;
        _obvDictionary = default;
    }
    #endregion

    ///Calls reset event on colllection change. A reset event is always used because index information is not available.
    private void DictionaryChanged(object sender, IDictionaryChangedEventArgs<TKey, TValue> e)
        => CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

    public abstract IEnumerator<TOutput> GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
