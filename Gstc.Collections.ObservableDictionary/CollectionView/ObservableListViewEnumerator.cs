using System;
using System.Collections;
using System.Collections.Generic;

namespace Gstc.Collections.ObservableDictionary.CollectionView;
internal class ObservableListViewEnumerator<TKey, TValue, TOutput> : IEnumerator<TOutput> {

    private ObservableListViewAbstract<TKey, TValue, TOutput> _listView;
    private readonly int _initialVersion;

    private int _index;
    private TOutput? _current;

    internal ObservableListViewEnumerator(ObservableListViewAbstract<TKey, TValue, TOutput> listView) {
        _index = 0;
        _listView = listView;
        _initialVersion = _listView._version;
        _current = default;
    }

    public void Dispose() {
        _listView = default;
        _current = default;
    }

    public bool MoveNext() {
        if (_initialVersion == _listView._version && _index < _listView.Count) {
            _current = _listView[_index++];
            return true;
        }
        if (_initialVersion != _listView._version) ThrowInvalidOperationException_InvalidOperation_EnumFailedVersion();
        _index = _listView.Count + 1;
        _current = default;
        return false;

    }
    public TOutput? Current => _current;

    object IEnumerator.Current {
        get {
            if (_index == 0 || _index > _listView.Count) ThrowInvalidOperationException_InvalidOperation_EnumOpCantHappen();
            return Current;
        }
    }

    void IEnumerator.Reset() {
        if (_initialVersion != _listView._version) ThrowInvalidOperationException_InvalidOperation_EnumFailedVersion();
        _index = 0;
        _current = default;
    }

    private void ThrowInvalidOperationException_InvalidOperation_EnumOpCantHappen() {
        throw new InvalidOperationException("Enumerator index exceeds list index."); //Todo: find out text for this error.
    }
    private void ThrowInvalidOperationException_InvalidOperation_EnumFailedVersion() {
        throw new InvalidOperationException("Collection was modified; enumeration operation may not execute.");
    }
}
