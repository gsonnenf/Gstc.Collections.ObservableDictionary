using System;
using System.Runtime.CompilerServices;

namespace Gstc.Collections.ObservableDictionary.ComponentModel {
    public class DictionaryEventArgsException : NotSupportedException {

        public static DictionaryEventArgsException Create(string className, [CallerMemberName] string propertyName = "undefined") {
            var message = className + " does not support " + propertyName + ".";
            return new DictionaryEventArgsException(message);
        }

        public DictionaryEventArgsException(string message) : base(message) { }
    }
}
