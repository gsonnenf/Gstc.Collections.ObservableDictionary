using System;
using System.Collections;
using System.Collections.Generic;

namespace Gstc.Collections.ObservableDictionary.Abstract {
    public abstract class AbstractDictionaryUpcast<TKey, TValue> :
        IDictionary<TKey, TValue>,
        IDictionary {

        protected abstract IDictionary<TKey, TValue> _internalDictionaryKv { get; }
        protected abstract IDictionary _InternalDictionary { get; }

        #region IDictionary<TKey,TValue>
        //abstract
        public abstract TValue this[TKey key] { get; set; }
        public abstract void Add(TKey key, TValue value);
        public abstract void Clear();
        public abstract bool Remove(TKey key);

        //non abstract
        public bool ContainsKey(TKey key) => _internalDictionaryKv.ContainsKey(key);
        public bool TryGetValue(TKey key, out TValue value) => _internalDictionaryKv.TryGetValue(key, out value);
        public void Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);
        public bool Contains(KeyValuePair<TKey, TValue> item) => ContainsKey(item.Key);
        public bool Remove(KeyValuePair<TKey, TValue> item) => Remove(item.Key);

        public ICollection<TKey> Keys => _internalDictionaryKv.Keys;
        public ICollection<TValue> Values => _internalDictionaryKv.Values;
        public int Count => _internalDictionaryKv.Count;
        public bool IsReadOnly => _internalDictionaryKv.IsReadOnly;
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => _internalDictionaryKv.CopyTo(array, arrayIndex);
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _internalDictionaryKv.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _internalDictionaryKv.GetEnumerator();
        #endregion

        #region IDictionary
        object IDictionary.this[object key] {
            get => this[(TKey)key];
            set => this[(TKey)key] = (TValue)value;
        }
        public bool IsFixedSize => _InternalDictionary.IsFixedSize;
        ICollection IDictionary.Keys => _InternalDictionary.Keys;
        ICollection IDictionary.Values => _InternalDictionary.Values;
        public bool IsSynchronized => _InternalDictionary.IsSynchronized;
        public object SyncRoot => _InternalDictionary.SyncRoot;

        void IDictionary.Add(object key, object value) => Add((TKey)key, (TValue)value);
        bool IDictionary.Contains(object key) => ContainsKey((TKey)key);
        void ICollection.CopyTo(Array array, int index) => _InternalDictionary.CopyTo(array, index);
        IDictionaryEnumerator IDictionary.GetEnumerator() => _InternalDictionary.GetEnumerator();
        void IDictionary.Remove(object key) => Remove((TKey)key);
        #endregion

        #region ICollection<KeyValuePair<TKey, TValue>>
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item) => Remove(item.Key);
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item) => _internalDictionaryKv.Contains(item);
        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => _internalDictionaryKv.CopyTo(array, arrayIndex);
        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => _internalDictionaryKv.IsReadOnly;
        #endregion
    }
}
