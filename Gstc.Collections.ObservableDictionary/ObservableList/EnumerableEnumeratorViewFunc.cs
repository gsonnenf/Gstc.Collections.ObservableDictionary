namespace Gstc.Collections.ObservableDictionary.ObservableList {
    public class EnumerableEnumeratorViewFunc<TInput, TOutput> : EnumerableEnumeratorViewAbstract<TInput, TOutput> {
        private Func<TInput, TOutput> _convertItem;
        public EnumerableEnumeratorViewFunc(IEnumerator<TInput> inputEnumerator, Func<TInput, TOutput> convertItem) {
            _inputEnumerator = inputEnumerator;
            _convertItem = convertItem;
        }

        public EnumerableEnumeratorViewFunc(IEnumerable<TInput> inputEnumerable, Func<TInput, TOutput> convertItem) {
            _inputEnumerator = inputEnumerable.GetEnumerator();
            _convertItem = convertItem;
        }
        protected override TOutput ConvertItem(TInput item) => _convertItem(item);

    }
}
