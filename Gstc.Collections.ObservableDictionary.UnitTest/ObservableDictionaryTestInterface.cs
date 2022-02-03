using AutoFixture;
using Gstc.Collections.ObservableDictionary.UnitTest.Tools;
using Gstc.Collections.ObservableLists.Test.MockObjects;
using Gstc.Collections.ObservableLists.Test.Tools;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Gstc.Collections.ObservableDictionary.UnitTest {
    [TestFixture]
    public class ObservableDictionaryTestInterface {

        #region Fields and Setup
        public static object[] StaticDataSource => new object[] {
            new ObservableDictionary<string, TestItem>(),
            new ObservableConcurrentDictionary<string,TestItem>(),
            new ObservableKeyedCollectionInline<string,TestItem>( (item) => item.Id )
        };

        private readonly Fixture _fixture = new();

        private readonly InterfaceTestCaseDictionary _testCasesDictionary = new();

        private readonly InterfaceTestCases _testCasesCollection = new();

        [SetUp]
        public void TestInit() {
            _testCasesDictionary.TestInit();
            _testCasesCollection.TestInit();
        }
        #endregion

        #region Tests
        [Description("Down casts to IDictionary and tests for observable triggers with that interface.")]
        [TestCaseSource(nameof(StaticDataSource))]
        [Test]
        public void Test_DictionaryInterface(object obj) {
            if (obj is IDictionary collection) _testCasesDictionary.DictionaryTest(collection);
            else Console.WriteLine("Collection is not a " + nameof(IDictionary));
        }


        [Description("Down casts to IDictionary<> and tests for observable triggers with that interface.")]
        [TestCaseSource(nameof(StaticDataSource))]
        [Test]
        public void DictionaryGeneric(object obj) {
            if (obj is IDictionary<string, TestItem> collection) _testCasesDictionary.DictionaryGenericTest(collection);
            else Console.WriteLine("Collection is not a " + nameof(IDictionary<string, TestItem>));
        }


        [Description("Down casts to ICollection<KeyValuePair<,>> and tests for observable triggers with that interface.")]
        [TestCaseSource(nameof(StaticDataSource))]
        [Test]
        public void CollectionKvp(object obj) {
            if (obj is ICollection<KeyValuePair<string, TestItem>> collection) _testCasesDictionary.CollectionKeyValuePairTest(collection);
            else Console.WriteLine("Collection is not a " + nameof(KeyValuePair<string, TestItem>));
        }


        [Description("Down casts to ICollection<TestItem> and tests for observable triggers with that interface.")]
        [TestCaseSource(nameof(StaticDataSource))]
        [Test]
        public void CollectionGenericInterface(object obj) {
            if (obj is ICollection<TestItem> collection) _testCasesCollection.CollectionGenericTest(collection);
            else Console.WriteLine("Collection is not a " + nameof(ICollection<TestItem>));
        }


        [Description("Down casts to ICollection and tests for observable triggers with that interface.")]
        [TestCaseSource(nameof(StaticDataSource))]
        [Test]
        public void Collection(object obj) {

            if (obj is IDictionary dictionary) { //Adds initial elements. ICollection does not have add method.
                dictionary.Add(_fixture.Create<string>(), _fixture.Create<TestItem>());
                dictionary.Add(_fixture.Create<string>(), _fixture.Create<TestItem>());
                dictionary.Add(_fixture.Create<string>(), _fixture.Create<TestItem>());
            } else if (obj is ICollection<TestItem> collectionGeneric) {
                collectionGeneric.Add(_fixture.Create<TestItem>());
                collectionGeneric.Add(_fixture.Create<TestItem>());
                collectionGeneric.Add(_fixture.Create<TestItem>());
            } else {
                Console.WriteLine("Could not add test elements collection for ICollection test");
                return;
            }

            if (obj is ICollection collection) _testCasesCollection.CollectionTest(collection);
            else Console.WriteLine("Collection is not a " + nameof(ICollection));
        }

        [Description("Down casts to IList<> and tests for observable triggers with that interface.")]
        [TestCaseSource(nameof(StaticDataSource))]
        [Test]
        public void ListInterfaceGeneric(object obj) {
            if (obj is IList<TestItem> collection) _testCasesCollection.ListGenericTest(collection);
            else Console.WriteLine("Collection is not a " + nameof(IList<TestItem>));
        }

        [Description("Down casts to IList and tests for observable triggers with that interface.")]
        [TestCaseSource(nameof(StaticDataSource))]
        [Test]
        public void ListInterface(object obj) {
            if (obj is IList collection) _testCasesCollection.ListTest(collection);
            else Console.WriteLine("Collection is not a " + nameof(IList));
        }
        #endregion
    }
}
