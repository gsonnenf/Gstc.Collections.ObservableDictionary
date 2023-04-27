using Gstc.Collections.ObservableDictionary.CollectionView;
using Gstc.Collections.ObservableDictionary.DictionaryEnumerable;
using Gstc.Collections.ObservableDictionary.ObservableList;
using Gstc.Collections.ObservableDictionary.Test.Fakes;
using Gstc.Collections.ObservableDictionary.Test.Tools;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Gstc.Collections.ObservableDictionary.Test;
[TestFixture]
internal class BenchmarkTest {


    [Test, Sequential]
    public void FindIndexLargeList(
        [Values(1000000, 100000, 10000, 10)] int iterations,
        [Values(10, 100, 1000, 1000000)] int numOfItems) {

        int result = -1;
        int desiredNum = numOfItems / 2;
        List<int> list = Enumerable.Range(0, numOfItems).ToList();
        IList<int> iList = list;

        //Warmup
        for (int i = 0; i < iterations; i++)
            result = list.Aggregate(0, (last, next) => last + next);
        Console.WriteLine("Ready:");

        using (ScopedStopwatch.Start("\nIndexOf"))
            for (int i = 0; i < iterations; i++)
                result = list.FindIndex(item => item == desiredNum);

        using (ScopedStopwatch.Start("\nIndexOf (check type"))
            for (int i = 0; i < iterations; i++) {
                if (iList is List<int>) result = list.FindIndex(item => item == desiredNum);
                else throw new InvalidCastException();
            }

        using (ScopedStopwatch.Start("\nToList().IndexOf"))
            for (int i = 0; i < iterations; i++)
                result = iList.ToList().FindIndex(item => item == desiredNum);

        using (ScopedStopwatch.Start("\n For loop"))
            for (int i = 0; i < iterations; i++)
                for (int index = 0; index < iList.Count; index++) {
                    if (iList[index] == desiredNum) {
                        result = index;
                        break;
                    }
                }

        using (ScopedStopwatch.Start("\nEnumerable.Range"))
            for (int i = 0; i < iterations; i++)
                result = Enumerable.Range(0, list.Count).FirstOrDefault(i => iList[i] == desiredNum);

        using (ScopedStopwatch.Start("\n Foreach"))
            for (int i = 0; i < iterations; i++)
                foreach (var item in iList) {
                    if (item == desiredNum) {
                        result = item;
                        break;
                    }
                }
    }

    [Test]
    public void EnumeratorVsLinqTest() {
        int numOfItems = 100000;
        int iterations = 1000;
        double temp = 0;

        Func<int, double> convertNum = (item => (double)item * -1);

        //setup
        var list = new List<int>();
        foreach (var index in Enumerable.Range(0, numOfItems)) list.Add(index);

        //test
        using (ScopedStopwatch.Start("\nWarmup")) {
            for (int i = 0; i < iterations; i++)
                foreach (double num in list.Select(convertNum)) temp = num;
            for (int i = 0; i < iterations; i++)
                foreach (double num in new EnumeratorAdapterIntDouble(list)) temp = num;
        }

        //These tests are for taking memory snapshots
        using (ScopedStopwatch.Start("\nLinq: int to double")) {
            for (int i = 0; i < iterations; i++) {
                var counter = 0;
                foreach (var num in list.Select(convertNum)) {
                    temp = num;
                    if (counter++ > numOfItems - 10)
                        break;
                }
            }
        }

        //Test 
        using (ScopedStopwatch.Start("\nCreating new list")) {
            for (int i = 0; i < iterations; i++) {
                var newList = list.Select(item => (double)item * -1).ToList();
                var counter = 0;
                foreach (var num in newList) {
                    temp = num;
                    if (counter++ > numOfItems - 10)
                        break;
                }
            }
        }
        //test
        using (ScopedStopwatch.Start("\nEnumerableAdapter: int to double")) {
            for (int i = 0; i < iterations; i++) {
                var counter = 0;
                foreach (var num in new EnumeratorAdapterIntDouble(list)) {
                    temp = num;
                    if (counter++ > numOfItems - 10)
                        break;
                }
            }
        }

        //test
        using (ScopedStopwatch.Start("\nEnumerableAdapter<>: int to double")) {
            for (int i = 0; i < iterations; i++)
                foreach (var num in new IteratorAdapterFunc<int, double>(list, convertNum)) temp = num;
        }

        //TEST B
        Func<TestClass1, TestClass2> ConvertTestClass = (item => new TestClass2(item.Num));
        var listTestClass = new List<TestClass1>();

        foreach (var index in Enumerable.Range(0, numOfItems)) listTestClass.Add(new TestClass1());
        //test
        using (ScopedStopwatch.Start("\nLinq: TestClass1 to TestClass2")) {
            for (int i = 0; i < iterations; i++)
                foreach (var item in listTestClass.Select(ConvertTestClass)) temp = item.Num;
        }
        //test
        using (ScopedStopwatch.Start("\nEnumerableAdapter: TestClass1 to TestClass2")) {
            for (int i = 0; i < iterations; i++)
                foreach (var item in new EnumeratorAdapterTestClass(listTestClass)) temp = item.Num;
        }
        //test
        using (ScopedStopwatch.Start("\nEnumerableAdapter<>: TestClass1 to TestClass2")) {
            for (int i = 0; i < iterations; i++)
                foreach (var item in new IteratorAdapterFunc<TestClass1, TestClass2>(listTestClass, ConvertTestClass)) temp = item.Num;
        }

    }

    public class TestClass1 {
        public int Num = 1;
    }

    public class TestClass2 {
        public int Num = 0;

        public TestClass2(int num) => Num = num;
    }

    public class EnumeratorAdapterTestClass : AbstractIteratorAdapter<TestClass1, TestClass2> {
        public EnumeratorAdapterTestClass(IEnumerator<TestClass1> inputEnumerator) : base(inputEnumerator) { }

        public EnumeratorAdapterTestClass(IEnumerable<TestClass1> inputEnumerator) : base(inputEnumerator) { }

        protected override TestClass2 ConvertItem(TestClass1 input) => new TestClass2(input.Num);
    }

    public class EnumeratorAdapterIntDouble : AbstractIteratorAdapter<int, double> {
        public EnumeratorAdapterIntDouble(IEnumerator<int> inputEnumerator) : base(inputEnumerator) { }

        public EnumeratorAdapterIntDouble(IEnumerable<int> inputEnumerator) : base(inputEnumerator) { }

        protected override double ConvertItem(int input) => -1 * (double)input;
    }

    [Test, Sequential]
    public void NotifyCollectionChangedEventArgsCastBenchmark(
            [Values(500000, 300000, 200000, 100000, 1000)] int iterations,
            [Values(2, 3, 5, 10, 1000)] int numOfItems) {

        int temp = 0;
        Console.WriteLine("Iterations: " + iterations);
        Console.WriteLine("numberOfElements: " + numOfItems);

        //Setup
        var collection = new Collection<KeyValuePair<int, int>>();
        foreach (var index in Enumerable.Range(0, numOfItems)) collection.Add(new KeyValuePair<int, int>(index, index));
        var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, collection, 0);
        NotifyCollectionChangedEventArgs newArgs = default;

        //Test
        using (ScopedStopwatch.Start("\nLinq"))
            for (int i = 0; i < iterations; i++) {
                var newIList = args.NewItems.Cast<KeyValuePair<int, int>>().Select(kvp => kvp.Key).ToList();
                newArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newIList, 0);
                for (int j = 0; j < numOfItems; j++) temp = (int)newArgs.NewItems[j];
            }
        Console.WriteLine(newArgs.NewItems);

        //Test
        using (ScopedStopwatch.Start("\nInt[]"))
            for (int i = 0; i < iterations; i++) {
                var newIList = new int[args.NewItems.Count];
                var index = 0;
                foreach (KeyValuePair<int, int> kvp in args.NewItems) newIList[index] = kvp.Key;
                newArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newIList, 0);
                for (int j = 0; j < numOfItems; j++) temp = (int)newArgs.NewItems[j];
            }
        Console.WriteLine(newArgs.NewItems);

        //Test
        using (ScopedStopwatch.Start("\nArrayList"))
            for (int i = 0; i < iterations; i++) {
                var newIList = new ArrayList(args.NewItems.Count);
                foreach (KeyValuePair<int, int> kvp in args.NewItems) newIList.Add(kvp.Key);
                newArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newIList, 0);
                for (int j = 0; j < numOfItems; j++) temp = (int)newArgs.NewItems[j];
            }
        Console.WriteLine(newArgs.NewItems);

        //Test
        using (ScopedStopwatch.Start("\nList<T>"))
            for (int i = 0; i < iterations; i++) {
                var newIList = new List<int>();
                foreach (KeyValuePair<int, int> kvp in args.NewItems) newIList.Add(kvp.Key);
                newArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newIList, 0);
                for (int j = 0; j < numOfItems; j++) temp = (int)newArgs.NewItems[j];
            }
        Console.WriteLine(newArgs.NewItems);

        //Test
        using (ScopedStopwatch.Start("\nCollection<T>"))
            for (int i = 0; i < iterations; i++) {
                var newIList = new Collection<int>();

                foreach (KeyValuePair<int, int> kvp in args.NewItems) newIList.Add(kvp.Key);
                newArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newIList, 0);
                for (int j = 0; j < numOfItems; j++) temp = (int)newArgs.NewItems[j];
            }
        Console.WriteLine(newArgs.NewItems);

        //Test
        using (ScopedStopwatch.Start("\nKvpListWrapper"))
            for (int i = 0; i < iterations; i++) {
                var iListWrapper = new KvpListWrapper(args.NewItems);
                newArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, iListWrapper, 0);
                for (int j = 0; j < numOfItems; j++) temp = (int)newArgs.NewItems[j];
            }
        Console.WriteLine(newArgs.NewItems);

        //Test
        if (numOfItems == 1) {
            using (ScopedStopwatch.Start("\nSingleItemKvpListWrapper")) //Todo: Implement this instead of List<T> for about a 30% improvement gain.
                for (int i = 0; i < iterations; i++) {
                    var iListWrapper = new SingleItemKvpListWrapper(args.NewItems);
                    newArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, iListWrapper, 0);
                    for (int j = 0; j < numOfItems; j++) temp = (int)newArgs.NewItems[j];
                }
            Console.WriteLine(newArgs.NewItems);
        } else Console.WriteLine("\nSingleItemKvpListWrapper not performed.\n");
    }

    public class SingleItemKvpListWrapper : SingleItemReadOnlyListWrapper<KeyValuePair<int, int>, int> {
        public SingleItemKvpListWrapper(IList list) : base(list) { }
        protected override int Convert(KeyValuePair<int, int> input) => input.Key;
    }

    public class KvpListWrapper : ReadOnlyListWrapper<KeyValuePair<int, int>, int> {
        public KvpListWrapper(IList list) : base(list) { }
        protected override int Convert(KeyValuePair<int, int> input) => input.Key;
    }



    [Test]
    public void EnumerableBenchmarks() {
        ObservableDictionary<string, TestClass> dict = new ObservableDictionary<string, TestClass>();

        //Setup
        int counter = 0;
        int iterations = 100000;
        for (int i = 0; i < 100; i++) dict.Add(i.ToString(), new());

        //Test 0
        counter = 0;
        using (ScopedStopwatch.Start("\nWarmup"))
            for (int i = 0; i < iterations; i++)
                foreach (var item in dict) counter += item.Value.Num;
        Console.WriteLine(counter);

        //Test 1
        using (ScopedStopwatch.Start("\nReference: Dictionary Kvp.Value"))
            for (int i = 0; i < iterations; i++)
                foreach (var item in dict) counter += item.Value.Num;
        Console.WriteLine(counter);

        //Test 2
        counter = 0;
        var values = dict.Values;
        using (ScopedStopwatch.Start("Test: Dictionary.Values"))
            for (int i = 0; i < iterations; i++)
                foreach (var item in values) counter += item.Num;
        Console.WriteLine(counter);

        //Test 3
        counter = 0;
        var enumerator = new ObservableEnumerableDictionaryValue<string, TestClass>(dict);
        using (ScopedStopwatch.Start("Test: ObservableEnumerableValue"))
            for (int i = 0; i < iterations; i++)
                foreach (var item in enumerator) counter += item.Num;
        Console.WriteLine(counter);

        //Test 4
        var listView = new ObservableListViewValue<string, TestClass>(dict);
        counter = 0;
        using (ScopedStopwatch.Start("Test: ObservableListViewValue"))
            for (int i = 0; i < iterations; i++)
                foreach (var item in listView) counter += item.Num;
        Console.WriteLine(counter);
    }



    private class TestClass {
        public int Num = 1;
    }
}
