using Gstc.Collections.ObservableDictionary.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;

namespace Gstc.Collections.ObservableDictionary.CollectionView;
/// <summary>
/// 
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public abstract class AbstractObservableListView<TKey, TValue, TOutput> : IObservableListView<TOutput>, IDisposable {

    #region Events
    public event NotifyCollectionChangedEventHandler CollectionChanged;
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
    #endregion

    private IObservableDictionary<TKey, TValue> _obvDict;
    internal Dictionary<TKey, int> _keyIndexDictionary = new();
    internal List<KeyValuePair<TKey, TValue>> _orderedCollection = new(); //Todo: remake wth a SortedList, and with just holding a key.
    internal int _version;
    private int _insertIndex = -1;

    public int Count => _orderedCollection.Count;

    #region Constructor
    public AbstractObservableListView(IObservableDictionary<TKey, TValue> obvDict) {
        _obvDict = obvDict;
        RebuildIndices();
        _obvDict.AddedKvp += OnAddedValue;
        _obvDict.RemovedKvp += OnRemovedValue;
        _obvDict.ReplacedKvp += OnReplacedValue;
        _obvDict.ResetKvp += OnResetValue;
        //todo: setup Changing events as well
    }

    ~AbstractObservableListView() => Dispose();


    public void Dispose() {
        _obvDict.AddedKvp -= OnAddedValue;
        _obvDict.RemovedKvp -= OnRemovedValue;
        _obvDict.ReplacedKvp -= OnReplacedValue;
        _obvDict.ResetKvp -= OnResetValue;
        _obvDict = default;
        _keyIndexDictionary = default;
        _orderedCollection = default;
    }
    #endregion

    #region Enumerators
    public IEnumerator<TOutput> GetEnumerator() => new ObservableListViewEnumerator<TKey, TValue, TOutput>(this);
    IEnumerator IEnumerable.GetEnumerator() => new ObservableListViewEnumerator<TKey, TValue, TOutput>(this);
    #endregion

    #region Methods
    /// <summary>
    /// Need benchmarking to compare kvp with c(d())
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
     //notes: _orderedCollection[index].Value; benchmarks @ 310ms. The extra memory for a kvp is probably worth the faster access.
    // notes: _obvDict[_orderedCollection[index]]; benchmarks @ 530ms, with _orderedCollection only holding keys. It uses less memory, but is much slower.
    public abstract TOutput this[int index] { get; }

    /// <summary>
    /// O(n) for _keyIndexDictionary
    /// O(1) for no _keyIndexDictionary
    /// </summary>
    /// <param name="oldIndex"></param>
    /// <param name="newIndex"></param>
    public void Move(int oldIndex, int newIndex) {
        _version++;
        var removedItem = _orderedCollection[oldIndex];
        _orderedCollection.RemoveAt(oldIndex);
        _orderedCollection.Insert(newIndex, removedItem);
        ResetKeyIndexDictionary();
        var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, removedItem, newIndex, oldIndex);
        CollectionChanged?.Invoke(this, eventArgs);
        Moved?.Invoke(this, eventArgs);
    }

    /// <summary>
    /// Insert is O(n), even if Add is O(1)
    /// </summary>
    /// <param name="key"></param>
    /// <param name="Value"></param>
    /// <param name="index"></param>
    public void Insert(TKey key, TValue Value, int index) {
        if (_insertIndex != -1) throw new ReentrancyException(nameof(AbstractObservableListView<TKey, TValue, TOutput>) + ": Insert method is not allowed to be used with async or reentrancy.");
        _insertIndex = index;
        _obvDict.Add(key, Value);
        _insertIndex = -1;
    }
    #endregion

    #region Event Handlers Value
    /// <summary>
    /// O(1)
    /// </summary>
    /// <param name="key"></param>
    /// <param name="newItem"></param>
    private void OnAddedValue(object sender, INotifyDictionaryChangedEventArgs<TKey, TValue> args) {
        var key = args.Key;
        var newItem = args.NewValue;
        _version++;
        if (_insertIndex == -1) {
            _keyIndexDictionary.Add(key, _orderedCollection.Count);
            _orderedCollection.Add(new KeyValuePair<TKey, TValue>(key, newItem));
            var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItem, _obvDict.Count);
            CollectionChanged?.Invoke(this, eventArgs);
            Added?.Invoke(this, eventArgs);
        } else {
            _orderedCollection.Insert(_insertIndex, new KeyValuePair<TKey, TValue>(key, newItem));
            ResetKeyIndexDictionary();
            var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItem, _insertIndex);
            CollectionChanged?.Invoke(this, eventArgs);
            Added?.Invoke(this, eventArgs);
        }
    }

    /// <summary>
    /// O(n) for no _keyIndexDictionary
    /// O(1) for _keyIndexDictionary
    /// </summary>
    /// <param name="key"></param>
    /// <param name="oldItem"></param>
    private void OnRemovedValue(object sender, INotifyDictionaryChangedEventArgs<TKey, TValue> args) {
        _version++;
        var key = args.Key;
        var oldItem = args.OldValue;
        var index = _keyIndexDictionary[key];
        _orderedCollection.RemoveAt(index);
        ResetKeyIndexDictionary();
        var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItem, index);
        CollectionChanged?.Invoke(this, eventArgs);
        Removed?.Invoke(this, eventArgs);
    }

    /// <summary>
    ///  O(1) for _keyIndexDictionary
    ///  O(N) for no _keyIndexDictionary
    /// </summary>
    /// <param name="key"></param>
    /// <param name="newItem"></param>
    /// <param name="oldItem"></param>
    /// 
    void OnReplacedValue(object sender, INotifyDictionaryChangedEventArgs<TKey, TValue> args) {
        _version++;
        var key = args.Key;
        var newItem = args.NewValue;
        var oldItem = args.OldValue;
        var index = _keyIndexDictionary[key];
        _orderedCollection[index] = new KeyValuePair<TKey, TValue>(key, newItem);
        var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItem, oldItem, index);
        CollectionChanged?.Invoke(this, eventArgs);
        Replaced?.Invoke(this, eventArgs);
    }

    /// <summary>
    /// O(n)
    /// </summary>
    void OnResetValue(object sender, INotifyDictionaryChangedEventArgs<TKey, TValue> args) {
        _version++;
        RebuildIndices();
        var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
        CollectionChanged?.Invoke(this, eventArgs);
        Reset?.Invoke(this, eventArgs);
    }
    #endregion

    #region Event Handles Key


    #endregion

    #region helper methods
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ResetKeyIndexDictionary() {
        _keyIndexDictionary.Clear();
        for (int i = 0; i > _orderedCollection.Count; i++) _keyIndexDictionary.Add(_orderedCollection[i].Key, i);
    }
    private void RebuildIndices() {
        _orderedCollection.Clear();
        _keyIndexDictionary.Clear();
        foreach (var kvp in _obvDict) {
            _keyIndexDictionary.Add(kvp.Key, _orderedCollection.Count);
            _orderedCollection.Add(kvp);
        }
    }
    #endregion


}
