using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Gstc.Collections.ObservableDictionary.CollectionView;
public class ObservableListViewEnumerable<TKey, TValue, TOutput> : IObservableListView<TOutput>, IDisposable {

    public event NotifyCollectionChangedEventHandler Adding;
    public event NotifyCollectionChangedEventHandler Moving;
    public event NotifyCollectionChangedEventHandler Removing;
    public event NotifyCollectionChangedEventHandler Replacing;
    public event NotifyCollectionChangedEventHandler Resetting;
    public event NotifyCollectionChangedEventHandler CollectionChanging;
    public event NotifyCollectionChangedEventHandler Added;
    public event NotifyCollectionChangedEventHandler Moved;
    public event NotifyCollectionChangedEventHandler Removed;
    public event NotifyCollectionChangedEventHandler Replaced;
    public event NotifyCollectionChangedEventHandler Reset;
    public event NotifyCollectionChangedEventHandler CollectionChanged;

    internal List<KeyValuePair<TKey, TValue>> _orderedCollection { get; }

    #region Ctor
    public ObservableListViewEnumerable(List<KeyValuePair<TKey, TValue>> orderedCollection) {
        _orderedCollection = orderedCollection;
    }
    #endregion

    public TKey this[int index] => _orderedCollection[index].Key;

    public IEnumerator<TOutput> GetEnumerator() {
        throw new NotImplementedException();
    }

    void IDisposable.Dispose() {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        throw new NotImplementedException();
    }


    #region define Event Handlers

    private void OnAdding(TKey key, int index) {
        var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, key, index);
        CollectionChanging?.Invoke(this, eventArgs);
        Adding?.Invoke(this, eventArgs);
    }

    private void OnAdded(TKey key, int index) {
        var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, key, index);
        CollectionChanged?.Invoke(this, eventArgs);
        Added?.Invoke(this, eventArgs);
    }



    #endregion
}
