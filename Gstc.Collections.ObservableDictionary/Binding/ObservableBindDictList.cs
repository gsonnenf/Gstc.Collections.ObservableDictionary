﻿using Gstc.Collections.ObservableDictionary.ComponentModel;
using Gstc.Collections.ObservableDictionary.ObservableList;
using Gstc.Collections.ObservableLists;
using Gstc.Collections.ObservableLists.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Gstc.Collections.ObservableDictionary.Binding;
public class ObservableBindDictList<TKey, TValue> : IDisposable {

    #region Fields and Properties
    private readonly SyncingFlagScope _syncing = new();
    private IObservableDictionary<TKey, TValue> _obvDict;
    private ObservableList<KeyValuePair<TKey, TValue>> _obvListKvp;
    public CollectionIdentifier SourceCollection { get; set; }
    public bool IsBidirectional { get; set; }
    #endregion

    #region Constructor
    public ObservableBindDictList(
          bool isBidirectional = true,
          CollectionIdentifier sourceCollection = CollectionIdentifier.Dictionary) {
        IsBidirectional = isBidirectional;
        SourceCollection = sourceCollection;
    }

    public ObservableBindDictList(
            IObservableDictionary<TKey, TValue> obvDict,
            ObservableList<KeyValuePair<TKey, TValue>> obvListKvp,
            bool isBidirectional = true,
            CollectionIdentifier sourceCollection = CollectionIdentifier.Dictionary) {
        IsBidirectional = isBidirectional;
        SourceCollection = sourceCollection;
        ReplaceDictionary(obvDict);
        ReplaceList(obvListKvp);
    }

    ~ObservableBindDictList() => Dispose();

    public void Dispose() {
        UnbindList();
        UnbindDict();
    }
    #endregion

    #region List Projections
    //public ObvEnumerableViewKey GetKeyView() => new ObvEnumerableViewKey(_obvDict);
    public class ObvEnumerableViewKey : ObservableEnumerableIList<KeyValuePair<TKey, TValue>, TKey> {
        public ObvEnumerableViewKey(IObservableList<KeyValuePair<TKey, TValue>> obvList) : base(obvList) { }
        public override TKey ConvertItem(KeyValuePair<TKey, TValue> item) => item.Key;
    }

    public class ObvEnumerableViewValue : ObservableEnumerableIList<KeyValuePair<TKey, TValue>, TValue> {
        public ObvEnumerableViewValue(IObservableList<KeyValuePair<TKey, TValue>> obvList) : base(obvList) { }
        public override TValue ConvertItem(KeyValuePair<TKey, TValue> item) => item.Value;
    }
    #endregion

    #region Bindings
    private void BindList() {
        _obvListKvp.Added += ObvListKvp_Added;
        _obvListKvp.Removed += ObvListKvp_Removed;
        _obvListKvp.Replaced += ObvListKvp_Replaced;
        _obvListKvp.Reset += ObvListKvp_Reset;
    }

    private void UnbindList() {
        _obvListKvp.Added -= ObvListKvp_Added;
        _obvListKvp.Removed -= ObvListKvp_Removed;
        _obvListKvp.Replaced -= ObvListKvp_Replaced;
        _obvListKvp.Reset -= ObvListKvp_Reset;
    }

    private void BindDict() {
        _obvDict.AddedKvp += ObvDict_AddingKvp;
        _obvDict.RemovedKvp += ObvDict_RemovedKvp;
        _obvDict.ReplacedKvp += ObvDict_ReplacedKvp;
        _obvDict.ResetKvp += ObvDict_ResetKvp;
    }

    private void UnbindDict() {
        _obvDict.AddedKvp -= ObvDict_AddingKvp;
        _obvDict.RemovedKvp -= ObvDict_RemovedKvp;
        _obvDict.ReplacedKvp -= ObvDict_ReplacedKvp;
        _obvDict.ResetKvp -= ObvDict_ResetKvp;
    }

    private void ReplaceDictionary(IObservableDictionary<TKey, TValue> obvDict) {
        if (obvDict != null) UnbindDict();
        _obvDict = obvDict;
        ResetSynchronization();
        if (obvDict != null) BindDict();
    }

    private void ReplaceList(ObservableList<KeyValuePair<TKey, TValue>> obvListKvp) {
        if (obvListKvp != null) UnbindList();
        _obvListKvp = obvListKvp;
        ResetSynchronization();
        if (obvListKvp != null) BindList();
    }

    private void ResetSynchronization() {
        using (_syncing.Begin()) switch (SourceCollection) {
                case CollectionIdentifier.Dictionary:
                    if (_obvListKvp == null) return;
                    _obvListKvp.Clear();
                    if (_obvDict == null) return;
                    foreach (var kvp in _obvDict) _obvListKvp.Add(kvp);
                    _obvListKvp.RefreshAll();
                    break;
                case CollectionIdentifier.List:
                    if (_obvDict == null) return;
                    _obvDict.Clear();
                    if (_obvListKvp == null) return;
                    foreach (var kvp in _obvListKvp) _obvDict.Add(kvp);
                    _obvDict.RefreshAll();
                    break;
                default:
                    throw new InvalidEnumArgumentException(nameof(SourceCollection));
            }
    }
    #endregion

    #region List Events
    private void ObvListKvp_Added(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs args) {
        if (_syncing.InProgress || _obvDict == null) return;
        if (IsBidirectional == false && !(SourceCollection == CollectionIdentifier.List)) throw OneWayBindingException.Create();
        using (_syncing.Begin())
            foreach (KeyValuePair<TKey, TValue> kvp in args.NewItems)
                _obvDict.Add(kvp);
    }

    private void ObvListKvp_Removed(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs args) {
        if (_syncing.InProgress || _obvDict == null) return;
        if (IsBidirectional == false && !(SourceCollection == CollectionIdentifier.List)) throw OneWayBindingException.Create();
        using (_syncing.Begin())
            foreach (KeyValuePair<TKey, TValue> kvp in args.OldItems)
                _obvDict.Remove(kvp.Key);
    }

    private void ObvListKvp_Replaced(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs args) {
        if (_syncing.InProgress || _obvDict == null) return;
        if (IsBidirectional == false && !(SourceCollection == CollectionIdentifier.List)) throw OneWayBindingException.Create();

        using (_syncing.Begin()) {
            for (int index = 0; index < args.OldItems.Count; index++) {
                var oldKvp = (KeyValuePair<TKey, TValue>)args.OldItems[index];
                var newKvp = (KeyValuePair<TKey, TValue>)args.NewItems[index];

                if (oldKvp.Key.Equals(newKvp.Key)) _obvDict[oldKvp.Key] = newKvp.Value;
                else {
                    if (!_obvDict.Remove(oldKvp)) throw new KeyNotFoundException();
                    _obvDict.Add(newKvp);
                }
            }
        }
    }

    private void ObvListKvp_Reset(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs args) {
        if (_syncing.InProgress || _obvDict == null) return;
        if (IsBidirectional == false && !(SourceCollection == CollectionIdentifier.List)) throw OneWayBindingException.Create();
        using (_syncing.Begin()) {
            _obvDict.Clear();
            foreach (var kvp in _obvListKvp) _obvDict.Add(kvp);
        }
    }
    #endregion

    #region Dictionary Events
    private void ObvDict_AddingKvp(object sender, DictAddEventArgs<TKey, TValue> args) {
        if (_syncing.InProgress || _obvListKvp == null) return;
        if (IsBidirectional == false && !(SourceCollection == CollectionIdentifier.Dictionary)) throw OneWayBindingException.Create();
        using (_syncing.Begin()) _obvDict.Add(new KeyValuePair<TKey, TValue>(args.Key, args.NewValue));
    }

    private void ObvDict_RemovedKvp(object sender, DictRemoveEventArgs<TKey, TValue> args) {
        if (_syncing.InProgress || _obvListKvp == null) return;
        if (IsBidirectional == false && !(SourceCollection == CollectionIdentifier.Dictionary)) throw OneWayBindingException.Create();
        using (_syncing.Begin()) _obvListKvp.RemoveAt(_obvListKvp.List.FindIndex(item => item.Key.Equals(args.Key))); //Notes: Benchmark shows find index is fast.
    }

    private void ObvDict_ReplacedKvp(object sender, DictReplaceEventArgs<TKey, TValue> args) {
        if (_syncing.InProgress || _obvListKvp == null) return;
        if (IsBidirectional == false && !(SourceCollection == CollectionIdentifier.Dictionary)) throw OneWayBindingException.Create();
        using (_syncing.Begin()) _obvListKvp[_obvListKvp.List.FindIndex(item => item.Key.Equals(args.Key))] = new KeyValuePair<TKey, TValue>(args.Key, args.NewValue);
    }

    private void ObvDict_ResetKvp(object sender, DictResetEventArgs<TKey, TValue> args) {
        if (_syncing.InProgress || _obvListKvp == null) return;
        if (IsBidirectional == false && !(SourceCollection == CollectionIdentifier.Dictionary)) throw OneWayBindingException.Create();
        using (_syncing.Begin()) _obvListKvp.List = _obvDict.ToList();
    }
    #endregion
}