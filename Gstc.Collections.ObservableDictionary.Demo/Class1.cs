using System.Collections;
using System.Collections.Generic;

namespace Gstc.Collections.ObservableDictionary.Demo {
    public class Class1 : IEnumerable {

        public List<Class2a> listInt = new() { new(1), new(2), new(3) };
        public List<Class2b> listString = new() { new("a"), new("b"), new("c") };
        //public IEnumerator<Class2a> GetEnumerator() => listInt.GetEnumerator();

        public IEnumerator GetEnumerator() => listString.GetEnumerator();
    }

    public class Class2a {
        public Class2a(int num) {
            Num = num;
        }

        public int Num { get; set; }
    }

    public class Class2b {
        public Class2b(string numString) {
            NumString = numString;
        }

        public string NumString { get; set; }
    }
}
