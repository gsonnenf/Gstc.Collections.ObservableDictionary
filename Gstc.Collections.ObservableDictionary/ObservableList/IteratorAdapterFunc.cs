using System;
using System.Collections;
using System.Collections.Generic;

namespace Gstc.Collections.ObservableDictionary.ObservableList {
    /// <summary>
    ///  Consider moving to snippets collection.
    /// This functionality is likely provided in Linq by the Select.
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public class IteratorAdapterFunc<TInput, TOutput> : IEnumerator<TOutput>, IEnumerable<TOutput> {
        private readonly IEnumerator<TInput> _inputEnumerator;
        private Func<TInput, TOutput> _convertItem;
        public IteratorAdapterFunc(IEnumerator<TInput> inputEnumerator, Func<TInput, TOutput> convertItem) {
            _inputEnumerator = inputEnumerator;
            _convertItem = convertItem;
        }

        public IteratorAdapterFunc(IEnumerable<TInput> inputEnumerable, Func<TInput, TOutput> convertItem) {
            _inputEnumerator = inputEnumerable.GetEnumerator();
            _convertItem = convertItem;
        }

        public TOutput Current => _convertItem(_inputEnumerator.Current);
        object IEnumerator.Current => _convertItem(_inputEnumerator.Current);
        public void Dispose() => _inputEnumerator.Dispose();
        public bool MoveNext() => _inputEnumerator.MoveNext();
        public void Reset() => _inputEnumerator.Reset();
        public IEnumerator<TOutput> GetEnumerator() => this;
        IEnumerator IEnumerable.GetEnumerator() => this;
    }
}
