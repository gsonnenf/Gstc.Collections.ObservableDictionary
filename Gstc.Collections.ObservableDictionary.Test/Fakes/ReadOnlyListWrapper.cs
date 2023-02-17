using System;
using System.Collections;
using System.Diagnostics;

namespace Gstc.Collections.ObservableDictionary.Test.Fakes {
    internal abstract class ReadOnlyListWrapper<TInput, TOutput> : IList {
        private readonly IList _list;
        protected abstract TOutput Convert(TInput input);
        internal ReadOnlyListWrapper(IList list) {
            Debug.Assert(list != null);
            _list = list;
        }
        public int Count => _list.Count;
        public bool IsReadOnly => true;
        public bool IsFixedSize => true;
        public bool IsSynchronized => _list.IsSynchronized;

        private static string SR = "SR.NotSupported_ReadOnlyCollection";
        public object? this[int index] {
            get => Convert((TInput)_list[index]); //I think i don't need null check

            set => throw new NotSupportedException(SR);
        }
        public object SyncRoot => _list.SyncRoot;
        public bool Contains(object? value) => _list.Contains(value);
        public void CopyTo(Array array, int index) => _list.CopyTo(array, index);
        public IEnumerator GetEnumerator() => _list.GetEnumerator(); //This would have to be a wrapper too.
        public int IndexOf(object? value) => _list.IndexOf(value);

        #region Not Supported
        public int Add(object? value) => throw new NotSupportedException(SR);
        public void Clear() => throw new NotSupportedException(SR);
        public void Insert(int index, object? value) => throw new NotSupportedException(SR);
        public void Remove(object? value) => throw new NotSupportedException(SR);
        public void RemoveAt(int index) => throw new NotSupportedException(SR);
        #endregion
    }

    internal abstract class SingleItemReadOnlyListWrapper<TInput, TOutput> : IList {
        private static string SR = "SR.NotSupported_ReadOnlyCollection";
        //private readonly object? _item;

        private readonly TOutput _item;
        protected abstract TOutput Convert(TInput input);
        public SingleItemReadOnlyListWrapper(IList singleItemList) => _item = Convert((TInput)singleItemList[0]);

        public object? this[int index] {
            get {
                if (index != 0) throw new ArgumentOutOfRangeException(nameof(index));
                return _item;
            }
            set => throw new NotSupportedException(SR);
        }

        public bool IsFixedSize => true;
        public bool IsReadOnly => true;
        public int Count => 1;
        public bool IsSynchronized => false;
        public object SyncRoot => this;
        public IEnumerator GetEnumerator() { yield return _item; }
        public bool Contains(object? value) => _item is null ? value is null : _item.Equals(value);
        public int IndexOf(object? value) => Contains(value) ? 0 : -1;
        public void CopyTo(Array array, int index) {
            //CollectionHelpers.ValidateCopyToArguments(1, array, index);
            array.SetValue(_item, index);
        }
        #region Not Supported
        public int Add(object? value) => throw new NotSupportedException(SR);
        public void Clear() => throw new NotSupportedException(SR);
        public void Insert(int index, object? value) => throw new NotSupportedException(SR);
        public void Remove(object? value) => throw new NotSupportedException(SR);
        public void RemoveAt(int index) => throw new NotSupportedException(SR);
        #endregion
    }
}
