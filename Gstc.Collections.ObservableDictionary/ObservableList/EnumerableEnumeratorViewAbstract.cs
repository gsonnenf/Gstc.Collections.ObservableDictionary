using System.Collections;
using System.Collections.Generic;

namespace Gstc.Collections.ObservableDictionary.ObservableList {
    public abstract class EnumerableEnumeratorViewAbstract<TInput, TOutput> : EnumeratorViewAbstract<TInput, TOutput>, IEnumerable<TOutput> {
        private readonly IEnumerator<TInput> _inputEnumerator;
        public EnumerableEnumeratorViewAbstract(IEnumerator<TInput> inputEnumerator) => _inputEnumerator = inputEnumerator;
        public EnumerableEnumeratorViewAbstract(IEnumerable<TInput> inputEnumerable) => _inputEnumerator = inputEnumerable.GetEnumerator();
        public IEnumerator<TOutput> GetEnumerator() => this;
        IEnumerator IEnumerable.GetEnumerator() => this;
    }
}
