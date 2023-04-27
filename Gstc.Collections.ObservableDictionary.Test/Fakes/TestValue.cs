using System;

namespace Gstc.Collections.ObservableDictionary.Test.Fakes {
    public class TestValue {
        public string Content { get; set; } = Guid.NewGuid().ToString();
    }
}
