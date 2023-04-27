using System;

namespace Gstc.Collections.ObservableDictionary.ObservableList {
    public class OneWayBindingException : NotSupportedException {
        public static OneWayBindingException Create() => new OneWayBindingException("The target collection was modified but bidirectional is not set to true.");
        public OneWayBindingException(string message) : base(message) { }
    }
}
