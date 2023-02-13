using System;
using System.Collections;
using System.Collections.Generic;

namespace Gstc.Collections.ObservableDictionary.CollectionView {
    internal class EnumeratorCollectionView<TKey, TValue>
        : IEnumerator<TValue> {
        private readonly ObservableCollectionView<TKey, TValue> _collectionView;
        private readonly int _initialVersion;

        private int _index;
        private TValue? _current;

        internal EnumeratorCollectionView(ObservableCollectionView<TKey, TValue> collectionView) {
            _index = 0;
            _collectionView = collectionView;
            _initialVersion = _collectionView._version;
            _current = default;
        }

        public void Dispose() { }

        public bool MoveNext() {
            if (_initialVersion == _collectionView._version && _index < _collectionView.Count) {
                _current = _collectionView[_index];
                _index++;
                return true;
            }
            if (_initialVersion != _collectionView._version) ThrowInvalidOperationException_InvalidOperation_EnumFailedVersion();

            _index = _collectionView.Count + 1;
            _current = default;
            return false;

        }
        public TValue Current => _current!;

        object IEnumerator.Current {
            get {
                if (_index == 0 || _index == _collectionView.Count + 1) ThrowInvalidOperationException_InvalidOperation_EnumOpCantHappen();
                return Current;
            }
        }

        void IEnumerator.Reset() {
            if (_initialVersion != _collectionView._version) {
                ThrowInvalidOperationException_InvalidOperation_EnumFailedVersion();
            }
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
}
