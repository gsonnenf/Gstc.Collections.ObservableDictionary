namespace Gstc.Collections.ObservableDictionary.Test.Fakes {
    internal class TestKey {
        public static int _idCount = 0;
        public static int NewId() => _idCount++;
        public int Id { get; set; } = NewId();
    }
}
