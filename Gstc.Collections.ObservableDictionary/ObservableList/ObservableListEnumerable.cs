using Gstc.Collections.ObservableLists;
using Gstc.Collections.ObservableLists.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;

namespace Gstc.Collections.ObservableDictionary.ObservableList {
    /// <summary>
    /// Creates a projection of ...
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public abstract class ObservableListEnumerable<TInput, TOutput> :
    IObservableEnumerable<TOutput>,
    INotifyCollectionChanging,
    INotifyCollectionChanged,
    INotifyListChangingEvents,
    INotifyListChangedEvents,
    IDisposable {

        #region Events
        public event NotifyCollectionChangedEventHandler CollectionChanging;
        public event NotifyCollectionChangedEventHandler Adding;
        public event NotifyCollectionChangedEventHandler Moving;
        public event NotifyCollectionChangedEventHandler Removing;
        public event NotifyCollectionChangedEventHandler Replacing;
        public event NotifyCollectionChangedEventHandler Resetting;
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event NotifyCollectionChangedEventHandler Added;
        public event NotifyCollectionChangedEventHandler Moved;
        public event NotifyCollectionChangedEventHandler Removed;
        public event NotifyCollectionChangedEventHandler Replaced;
        public event NotifyCollectionChangedEventHandler Reset;
        #endregion

        #region Properties
        public IObservableList<TInput> _obvList { get; set; }
        #endregion

        #region Constructors
        public ObservableListEnumerable(IObservableList<TInput> obvList) {
            _obvList = obvList;
            Bind();
        }

        public void Bind() {
            _obvList.Adding += OnAddingEvent;
            _obvList.Removing += OnRemovingEvent;
            _obvList.Replacing += OnReplacingEvent;
            _obvList.Resetting += OnResetingEvent;
            _obvList.Added += OnAddedEvent;
            _obvList.Moved += OnMovedEvent;
            _obvList.Removed += OnRemovedEvent;
            _obvList.Replaced += OnReplacedEvent;
            _obvList.Reset += OnResetEvent;
        }

        public void Dispose() {
            _obvList.Adding -= OnAddingEvent;
            _obvList.Moving -= OnMovingEvent;
            _obvList.Removing -= OnRemovingEvent;
            _obvList.Replacing -= OnReplacingEvent;
            _obvList.Resetting -= OnResetingEvent;
            _obvList.Added -= OnAddedEvent;
            _obvList.Moved -= OnMovedEvent;
            _obvList.Removed -= OnRemovedEvent;
            _obvList.Replaced -= OnReplacedEvent;
            _obvList.Reset -= OnResetEvent;
            _obvList = default;
        }

        ~ObservableListEnumerable() => Dispose();
        #endregion

        #region Abstract Methods
        public abstract TOutput ConvertItem(TInput item);
        #endregion

        #region Enumerators Methods
        // This can also be used with linq, but we are currently avoiding linq dependencies. _obvList.Select(ConverItem).GetEnumerator();
        public IEnumerator<TOutput> GetEnumerator() => new ObvEnumerableViewEnumerator(this);
        IEnumerator IEnumerable.GetEnumerator() => new ObvEnumerableViewEnumerator(this);
        #endregion

        #region EventHandler Methods - Changing
        private void OnAddingEvent(object sender, NotifyCollectionChangedEventArgs args) {
            var eventArgs = ConvertAddArgs(args);
            CollectionChanging?.Invoke(this, eventArgs);
            Adding?.Invoke(this, eventArgs);
        }
        private void OnMovingEvent(object sender, NotifyCollectionChangedEventArgs args) {
            var eventArgs = ConvertMoveArgs(args);
            CollectionChanging?.Invoke(this, eventArgs);
            Moving?.Invoke(this, eventArgs);
        }
        private void OnRemovingEvent(object sender, NotifyCollectionChangedEventArgs args) {
            var eventArgs = ConvertRemoveArgs(args);
            CollectionChanging?.Invoke(this, eventArgs);
            Removing?.Invoke(this, eventArgs);
        }
        private void OnReplacingEvent(object sender, NotifyCollectionChangedEventArgs args) {
            var eventArgs = ConvertReplaceArgs(args);
            CollectionChanging?.Invoke(this, eventArgs);
            Replacing?.Invoke(this, eventArgs);
        }
        private void OnResetingEvent(object sender, NotifyCollectionChangedEventArgs args) {
            var eventArgs = ConvertResetArgs(args);
            CollectionChanging?.Invoke(this, eventArgs);
            Resetting?.Invoke(this, eventArgs);
        }
        #endregion

        #region EventHandler Methods - Changed
        private void OnAddedEvent(object sender, NotifyCollectionChangedEventArgs args) {
            var eventArgs = ConvertAddArgs(args);
            CollectionChanged?.Invoke(this, eventArgs);
            Added?.Invoke(this, eventArgs);
        }
        private void OnMovedEvent(object sender, NotifyCollectionChangedEventArgs args) {
            var eventArgs = ConvertMoveArgs(args);
            CollectionChanged?.Invoke(this, eventArgs);
            Moved?.Invoke(this, eventArgs);
        }
        private void OnRemovedEvent(object sender, NotifyCollectionChangedEventArgs args) {
            var eventArgs = ConvertRemoveArgs(args);
            CollectionChanged?.Invoke(this, eventArgs);
            Removed?.Invoke(this, eventArgs);
        }
        private void OnReplacedEvent(object sender, NotifyCollectionChangedEventArgs args) {
            var eventArgs = ConvertReplaceArgs(args);
            CollectionChanged?.Invoke(this, eventArgs);
            Replaced?.Invoke(this, eventArgs);
        }
        private void OnResetEvent(object sender, NotifyCollectionChangedEventArgs args) {
            var eventArgs = ConvertResetArgs(args);
            CollectionChanged?.Invoke(this, eventArgs);
            Reset?.Invoke(this, eventArgs);
        }
        #endregion

        #region Conversion Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private List<TOutput> ConvertItems(IList inputItems) {
            //Note: In benchmarking creating a new list was nearly the fastest option available, with only a custome IList wrapper being about 20% faster.
            var outputItems = new List<TOutput>();
            foreach (TInput item in inputItems) outputItems.Add(ConvertItem(item));
            return outputItems;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private NotifyCollectionChangedEventArgs ConvertAddArgs(NotifyCollectionChangedEventArgs args) =>
            args.NewItems.Count == 1 ?
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, ConvertItem((TInput)args.NewItems), args.NewStartingIndex) :
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, ConvertItems(args.NewItems), args.NewStartingIndex);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private NotifyCollectionChangedEventArgs ConvertMoveArgs(NotifyCollectionChangedEventArgs args) =>
            args.NewItems.Count == 1 ?
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, ConvertItem((TInput)args.OldItems), args.NewStartingIndex, args.OldStartingIndex) :
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, ConvertItems(args.OldItems), args.NewStartingIndex, args.OldStartingIndex);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private NotifyCollectionChangedEventArgs ConvertRemoveArgs(NotifyCollectionChangedEventArgs args) =>
            args.NewItems.Count == 1 ?
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, ConvertItem((TInput)args.OldItems), args.OldStartingIndex) :
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, ConvertItems(args.OldItems), args.OldStartingIndex);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private NotifyCollectionChangedEventArgs ConvertReplaceArgs(NotifyCollectionChangedEventArgs args) =>
           args.NewItems.Count == 1 ?
               new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, ConvertItem((TInput)args.NewItems), ConvertItem((TInput)args.OldItems), args.OldStartingIndex) :
               new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, ConvertItems(args.NewItems), ConvertItems(args.OldItems), args.OldStartingIndex);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private NotifyCollectionChangedEventArgs ConvertResetArgs(NotifyCollectionChangedEventArgs args) =>
                   new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
        #endregion

        #region Enumerator Class
        private class ObvEnumerableViewEnumerator : IEnumerator<TOutput> {
            private readonly IEnumerator<TInput> _inputEnumerator;
            private readonly ObservableListEnumerable<TInput, TOutput> _obvListView;
            public ObvEnumerableViewEnumerator(ObservableListEnumerable<TInput, TOutput> obvListView) {
                _obvListView = obvListView;
                _inputEnumerator = obvListView._obvList.GetEnumerator();
            }
            public TOutput Current => _obvListView.ConvertItem(_inputEnumerator.Current);
            object IEnumerator.Current => _obvListView.ConvertItem(_inputEnumerator.Current);
            public void Dispose() => _inputEnumerator.Dispose();
            public bool MoveNext() => _inputEnumerator.MoveNext();
            public void Reset() => _inputEnumerator.Reset();
        }
        #endregion
    }
}
