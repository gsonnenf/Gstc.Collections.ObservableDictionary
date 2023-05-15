using System;
using System.Collections.Generic;

namespace Gstc.Collections.ObservableDictionary.ObservableList {
    public class EnumeratorViewFunc<TInput, TOutput> : EnumeratorViewAbstract<TInput, TOutput> {
        private Func<TInput, TOutput> _convertItem;
        public EnumeratorViewFunc(IEnumerator<TInput> inputEnumerator, Func<TInput, TOutput> convertItem) {
            _inputEnumerator = inputEnumerator;
            _convertItem = convertItem;
        }

        public EnumeratorViewFunc(IEnumerable<TInput> inputEnumerable, Func<TInput, TOutput> convertItem) {
            _inputEnumerator = inputEnumerable.GetEnumerator();
            _convertItem = convertItem;
        }
        protected override TOutput ConvertItem(TInput item) => _convertItem(item);

    }
}
