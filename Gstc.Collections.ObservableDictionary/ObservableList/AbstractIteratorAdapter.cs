using System.Collections;
using System.Collections.Generic;

namespace Gstc.Collections.ObservableDictionary.ObservableList {
    /// <summary>
    /// Consider moving to snippets collection.
    /// Allows an enumerator to be enumerated as a transformed type. 
    /// This appears to be done effectively by linq Select for enumerators and not needed.
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public abstract class AbstractIteratorAdapter<TInput, TOutput> : IEnumerator<TOutput>, IEnumerable<TOutput> {
        private readonly IEnumerator<TInput> _inputEnumerator;
        protected abstract TOutput ConvertItem(TInput input);
        public AbstractIteratorAdapter(IEnumerator<TInput> inputEnumerator) => _inputEnumerator = inputEnumerator;
        public AbstractIteratorAdapter(IEnumerable<TInput> inputEnumerable) => _inputEnumerator = inputEnumerable.GetEnumerator();
        public TOutput Current => ConvertItem(_inputEnumerator.Current);
        object IEnumerator.Current => ConvertItem(_inputEnumerator.Current);
        public void Dispose() => _inputEnumerator.Dispose();
        public bool MoveNext() => _inputEnumerator.MoveNext();
        public void Reset() => _inputEnumerator.Reset();
        public IEnumerator<TOutput> GetEnumerator() => this;
        IEnumerator IEnumerable.GetEnumerator() => this;
    }
}
