using System.Collections;
using System.Collections.Generic;

namespace Gstc.Collections.ObservableDictionary.ObservableList {

    /// <summary>
    /// Creates a projection of an enumerator of type {TInput} in the form of {TOutput} as specified by
    /// the abstract ConvertItem() method.
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public abstract class EnumeratorViewAbstract<TInput, TOutput> : IEnumerator<TOutput> {
        protected readonly IEnumerator<TInput> _inputEnumerator;
        protected abstract TOutput ConvertItem(TInput item);
        public EnumeratorViewAbstract(IEnumerable<TInput> list) => _inputEnumerator = list.GetEnumerator();
        protected EnumeratorViewAbstract() { }
        public TOutput Current => ConvertItem(_inputEnumerator.Current);
        object IEnumerator.Current => ConvertItem(_inputEnumerator.Current);
        public void Dispose() => _inputEnumerator.Dispose();
        public bool MoveNext() => _inputEnumerator.MoveNext();
        public void Reset() => _inputEnumerator.Reset();

    }
}
