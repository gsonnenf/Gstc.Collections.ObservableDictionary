
using Gstc.Collections.ObservableDictionary.CollectionView;
using NUnit.Framework;
using System;
using System.Diagnostics;

namespace Gstc.Collections.ObservableDictionary.Test {
    [TestFixture]
    internal class EnumeratorTest {
        [Test]
        public void Benchmark() {
            Stopwatch stopWatch;
            ObservableDictionary<string, TestClass> dict = new ObservableDictionary<string, TestClass>();
            int counter = 0;

            for (int i = 0; i < 100; i++) dict.Add(i.ToString(), new());

            int iterations = 100000;
            //Test 0
            Console.WriteLine("Test: Warmup");
            stopWatch = new Stopwatch();
            counter = 0;

            stopWatch.Start();
            for (int i = 0; i < iterations; i++)
                foreach (var item in dict) counter += item.Value.Num;
            stopWatch.Stop();

            Console.WriteLine(counter);
            Console.WriteLine("Time Elapsed: " + stopWatch.Elapsed + "\n");

            //Test 1
            Console.WriteLine("Test: Reference Dictionary Kvp");
            stopWatch = new Stopwatch();
            counter = 0;

            stopWatch.Start();
            for (int i = 0; i < iterations; i++)
                foreach (var item in dict) counter += item.Value.Num;
            stopWatch.Stop();

            Console.WriteLine(counter);
            Console.WriteLine("Time Elapsed: " + stopWatch.Elapsed + "\n");

            //Test 2
            Console.WriteLine("Test: Dictionary Values");
            stopWatch = new Stopwatch();
            counter = 0;
            var values = dict.Values;

            stopWatch.Start();
            for (int i = 0; i < iterations; i++)
                foreach (var item in values) counter += item.Num;
            stopWatch.Stop();

            Console.WriteLine(counter);
            Console.WriteLine("Time Elapsed: " + stopWatch.Elapsed + "\n");

            //Test 3
            Console.WriteLine("Test: ObservableEnumerable");
            stopWatch = new Stopwatch();
            counter = 0;
            var enumerator = new ObservableEnumerableValue<string, TestClass>(dict);


            stopWatch.Start();
            for (int i = 0; i < iterations; i++) {
                foreach (var item in enumerator) counter += item.Num;
            }
            stopWatch.Stop();

            Console.WriteLine(counter);
            Console.WriteLine("Time Elapsed: " + stopWatch.Elapsed + "\n");

            //Test 4
            Console.WriteLine("Test: ObservableCollectionView");
            stopWatch = new Stopwatch();
            var collectionView = new ObservableListViewValue<string, TestClass>(dict);
            counter = 0;

            stopWatch.Start();
            for (int i = 0; i < iterations; i++)
                foreach (var item in collectionView) counter += item.Num;
            stopWatch.Stop();

            Console.WriteLine(counter);
            Console.WriteLine("Time Elapsed: " + stopWatch.Elapsed + "\n");


        }

        private class TestClass {
            public int Num = 1;
        }
    }
}
