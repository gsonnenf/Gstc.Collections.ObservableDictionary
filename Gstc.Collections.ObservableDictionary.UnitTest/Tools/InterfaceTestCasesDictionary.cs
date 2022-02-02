using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Gstc.Collections.ObservableDictionary.NotificationDictionary;
using Gstc.Collections.ObservableLists.Test.MockObjects;
using NUnit.Framework;

namespace Gstc.Collections.ObservableDictionary.UnitTest.Tools {
    public class InterfaceTestCaseDictionary : DictionaryTestBase<string,TestItem> {

        public void DictionaryTest(IDictionary dictionary) {

            Assert.IsNotNull(dictionary as INotifyDictionaryChanged);

            MockEvent.AddNotifiersDictionary(dictionary as INotifyDictionaryChanged);

            //Special case: ObservableListKeyed must have key match mapped property in object.
            // if (dictionary is ObservableListKeyed<string, TestItem>) {
            //     Key1 = Item1.Id;
            //     Key2 = Item2.Id;
            //     Key3 = Item3.Id;
            //     DefaultKey = DefaultValue.Id;
            //     UpdateKey = UpdateValue.Id;
            // }

            //Add Test
            dictionary.Add(Key1, Item1);
            Assert.AreEqual(1, dictionary.Count);
            MockEvent.AssertMockNotifiersDictionary(2,1);

            //Contains test
            Assert.IsTrue(dictionary.Contains(Key1));
            Assert.IsFalse(dictionary.Contains(Key2));

            //Remove Test
            dictionary.Remove(Key1);
            Assert.AreEqual(0, dictionary.Count);
            MockEvent.AssertMockNotifiersDictionary(2,1);

            //index[] test
            dictionary[DefaultKey] = DefaultValue;
            Assert.AreEqual(1, dictionary.Count);
            MockEvent.AssertMockNotifiersDictionary(2,1);

            //Special case: ObservableListKeyed must have key match mapped property in object.
            //if (dictionary is ObservableListKeyed<string, TestItem>) UpdateValue.Id = DefaultKey;
            dictionary[DefaultKey] = UpdateValue;
            Assert.AreEqual(1, dictionary.Count);
            MockEvent.AssertMockNotifiersDictionary(1,1);

            //Clear Test
            dictionary.Clear();
            Assert.AreEqual(0, dictionary.Count);
            MockEvent.AssertMockNotifiersDictionary(2,1);

            //Keys/Value Test

            //Special Case: ObservableCollection Sorted List will sort these, they must be in order or it will fail general test.
            //if (dictionary is ObservableSortedList<string, TestItem>) {
            //    Key1 = "1";
            //    Key2 = "2";
            //}

            dictionary.Add(Key1, Item1);
            dictionary.Add(Key2, Item2);

            var keys = dictionary.Keys.GetEnumerator();
            keys.MoveNext();
            Assert.IsTrue(dictionary.Contains(keys.Current));
            keys.MoveNext();
            Assert.IsTrue(dictionary.Contains(keys.Current));
            keys.MoveNext();
            Assert.IsFalse(dictionary.Contains("not a key"));


            var values = dictionary.Values.GetEnumerator();
            values.MoveNext();
            Assert.IsTrue(Item1 == values.Current || Item2 == values.Current); //Concurrent dictionary is out of order.
            values.MoveNext();
            Assert.IsTrue(Item1 == values.Current || Item2 == values.Current); //Concurrent dictionary is out of order.

            //Readonlytest
            Assert.IsNotNull(dictionary.IsReadOnly);

            //IsFixedSize
            Assert.IsNotNull(dictionary.IsFixedSize);

            //ICollection
            var enumerator = dictionary.GetEnumerator();
            enumerator.MoveNext();
            Assert.IsTrue(dictionary.Contains(enumerator.Key));
            Assert.IsTrue(dictionary[enumerator.Key] == enumerator.Value);
 
            enumerator.MoveNext();
            Assert.IsTrue(dictionary.Contains(enumerator.Key));
            Assert.IsTrue(dictionary[enumerator.Key] == enumerator.Value);
        }

        public void DictionaryGenericTest(IDictionary<string, TestItem> dictionary) {

            Assert.IsNotNull(dictionary as INotifyDictionaryChanged);
            MockEvent.AddNotifiersDictionary(dictionary as INotifyDictionaryChanged);

            //Special case: ObservableListKeyed must have key match mapped property in object.
            //if (dictionary is ObservableListKeyed<string, TestItem>) {
            //    Key1 = Item1.Id;
            //    Key2 = Item2.Id;
            //    Key3 = Item3.Id;
            //    DefaultKey = DefaultValue.Id;
            //    UpdateKey = UpdateValue.Id;
            //}

            //Add Test
            dictionary.Add(Key1, Item1);
            Assert.AreEqual(1, dictionary.Count);
            MockEvent.AssertMockNotifiersDictionary(2,1);

            //Contains test
            Assert.IsTrue(dictionary.ContainsKey(Key1));
            Assert.IsFalse(dictionary.ContainsKey(Key2));

            //Remove Test
            dictionary.Remove(Key1);
            Assert.AreEqual(0, dictionary.Count);
            MockEvent.AssertMockNotifiersDictionary(2,1);

            //index[] test
            dictionary[DefaultKey] = DefaultValue;
            Assert.AreEqual(1, dictionary.Count);
            MockEvent.AssertMockNotifiersDictionary(2,1);

            //Special case: ObservableListKeyed must have key match mapped property in object.
            //if (dictionary is ObservableListKeyed<string, TestItem>) UpdateValue.Id = DefaultKey;

            dictionary[DefaultKey] = UpdateValue;
            Assert.AreEqual(1, dictionary.Count);
            MockEvent.AssertMockNotifiersDictionary(1,1);

            //Clear Test
            dictionary.Clear();
            Assert.AreEqual(0, dictionary.Count);
            MockEvent.AssertMockNotifiersDictionary(2,1);

            //Keys/Value Test
            //if (dictionary is ObservableSortedList<string, TestItem>) { //Special case for a sorted list
            //    Key1 = "1";
            //    Key2 = "2";
            //}

            dictionary.Add(Key1, Item1);
            dictionary.Add(Key2, Item2);

            //Enumerator Key
            using (var keys = dictionary.Keys.GetEnumerator()) {
                keys.MoveNext();
                Assert.IsTrue(Key1 == keys.Current || Key2 == keys.Current); //NOTE: Concurrent Dictionary does not provide keys in order added.
                keys.MoveNext();
                Assert.IsTrue(Key1 == keys.Current || Key2 == keys.Current);
                keys.MoveNext();
                Assert.IsNull(keys.Current);
            }

            //Enumerator Value
            using (var values = dictionary.Values.GetEnumerator()) {
                values.MoveNext();
                Assert.IsTrue(Item1 == values.Current || Item2 == values.Current); //NOTE: Concurrent Dictionary does not provide items in order added.
                values.MoveNext();
                Assert.IsTrue(Item1 == values.Current || Item2 == values.Current);
                values.MoveNext();
                Assert.IsNull(values.Current);
            }

            //TryGetValue test
            Assert.IsTrue(dictionary.TryGetValue(Key1, out var itemA));
            Assert.AreSame(Item1,itemA);
            Assert.IsFalse(dictionary.TryGetValue(Key3, out var itemB));
            Assert.AreSame(null, itemB);

            //Readonlytest
            Assert.IsNotNull(dictionary.IsReadOnly);

            //KVP Enumerator Test
            var enumerator = dictionary.GetEnumerator();
            enumerator.MoveNext();
            Assert.IsTrue( Key1 == enumerator.Current.Key || Key2 == enumerator.Current.Key);
            Assert.IsTrue( Item1 == enumerator.Current.Value || Item2 == enumerator.Current.Value);
            
            enumerator.MoveNext();
            Assert.IsTrue(Key1 == enumerator.Current.Key || Key2 == enumerator.Current.Key);
            Assert.IsTrue(Item1 == enumerator.Current.Value || Item2 == enumerator.Current.Value);
            var a2 = enumerator.Current.Key;

            //BUG: There is a bug in concurrent dictionary where the IEnumerator keeps yielding the last item instead of null.
            if (dictionary is not ObservableConcurrentDictionary<string, TestItem>) {
                enumerator.MoveNext();
                Assert.IsNull(enumerator.Current.Key);
                Assert.IsNull(enumerator.Current.Value);
            }
            //IEnumerable
        }

        public void CollectionKeyValuePairTest(ICollection<KeyValuePair<string, TestItem>> collection) {

            Assert.IsNotNull(collection as INotifyDictionaryChanged);
            MockEvent.AddNotifiersDictionary(collection as INotifyDictionaryChanged);

            //Special case, ObservableListKeyed must have Keys match Item ID
            //if (collection is ObservableListKeyed<string, TestItem>) {
            //    Key1 = Item1.Id;
            //    Key2 = Item2.Id;
            //    Key3 = Item3.Id;
            //}

            //Special case, ObservableSortedList puts keys in order, so it must be added in alphabetical order to pass this general dictionary test.
            // if (collection is ObservableSortedList<string, TestItem>) {
            //     Key1 = "1";
            //     Key2 = "2";
            //     Key3 = "3";
            // }

            //Add and count
            var kvp1 = new KeyValuePair<string, TestItem>(Key1, Item1);
            var kvp2 = new KeyValuePair<string, TestItem>(Key2, Item2);
            var kvp3 = new KeyValuePair<string, TestItem>(Key3, Item3);

            collection.Add(kvp1);
            Assert.AreEqual(1, collection.Count);
            MockEvent.AssertMockNotifiersDictionary(2,1);

            //Remove Test
            collection.Remove(kvp1);
            Assert.AreEqual(0, collection.Count);
            MockEvent.AssertMockNotifiersDictionary(2,1);

            //ClearTest
            collection.Add(kvp1);
            collection.Add(kvp2);
            Assert.AreEqual(2, collection.Count);
            MockEvent.AssertMockNotifiersDictionary(4,2);

            collection.Clear();
            Assert.AreEqual(0, collection.Count);
            MockEvent.AssertMockNotifiersDictionary(2,1);

            //Contains Test
            collection.Add(kvp1);
            Assert.IsTrue(collection.Contains(kvp1));
            Assert.IsFalse(collection.Contains(kvp2));

            //Syncroot Test
            Assert.IsFalse(collection.IsReadOnly);

            //IEnumerator/ EnumeratorGeneric test
            collection.Clear();
            collection.Add(kvp1);
            collection.Add(kvp2);
            collection.Add(kvp3);
            Assert.AreEqual(3, collection.Count);

            IEnumerator enumerator = collection.GetEnumerator();
            Assert.IsNotNull(enumerator);
            
            enumerator.MoveNext();

            var dictionary = collection as IDictionary<string, TestItem>;
            var kvp = ((KeyValuePair<string, TestItem>) enumerator.Current);
            Assert.IsTrue( dictionary.ContainsKey( kvp.Key));
            Assert.IsTrue( dictionary[kvp.Key] == kvp.Value);


            IEnumerator<KeyValuePair<string, TestItem>> enumeratorGeneric = collection.GetEnumerator();
            Assert.IsNotNull(enumeratorGeneric);
            enumeratorGeneric.MoveNext();
            Assert.IsTrue(dictionary.ContainsKey(enumeratorGeneric.Current.Key));
            Assert.IsTrue(dictionary[enumeratorGeneric.Current.Key] == enumeratorGeneric.Current.Value);

            //CopyTo test
            var array = new KeyValuePair<string, TestItem>[3];
            collection.CopyTo(array, 0);
            Assert.IsTrue(array.Contains(kvp1));
            Assert.IsTrue(array.Contains(kvp2));
            Assert.IsTrue(array.Contains(kvp3));
            Assert.IsFalse(array.Contains(new KeyValuePair<string, TestItem>()));
        }
    }

}
