using Gstc.Collections.ObservableDictionary.Test.Fakes;
using NUnit.Framework;
using System;

namespace Gstc.Collections.ObservableDictionary.UnitTest {
    [TestFixture]
    internal class ObservableDictionaryArgsTest {

        [Test]
        internal void ArgTest() {
            ObservableDictionary<TestKey, TestValue> obvDict = new();

            TestKey key = new TestKey();
            TestValue value = new TestValue();

            obvDict.AddedKvp += (_, _) => Console.WriteLine("test");

            obvDict.Add(key, value);


        }
    }
}
