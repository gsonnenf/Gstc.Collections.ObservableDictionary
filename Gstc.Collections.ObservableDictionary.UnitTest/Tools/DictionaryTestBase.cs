using AutoFixture;
using Gstc.Collections.ObservableDictionary.NotificationDictionary;
using Gstc.Collections.ObservableLists.Test.Tools;
using NUnit.Framework;

namespace Gstc.Collections.ObservableDictionary.UnitTest.Tools {
    public class DictionaryTestBase<TKey,TItem> : CollectionTestBase<TItem> {

        protected TKey Key1 { get; set; }
        protected TKey Key2 { get; set; }
        protected TKey Key3 { get; set; }

        protected TKey DefaultKey { get; set; }
        protected TKey UpdateKey { get; set; }


        public new void TestInit() {
            base.TestInit();
            Key1 = Fixture.Create<TKey>();
            Key2 = Fixture.Create<TKey>();
            Key3 = Fixture.Create<TKey>();

            DefaultKey = Fixture.Create<TKey>();
            UpdateKey = Fixture.Create<TKey>();
        }


        #region Test_DictionaryInterface Event Arg Tests
        protected void AssertDictionaryEventReset(object sender, NotifyDictionaryChangedEventArgs args) {
            Assert.That(args.Action, Is.EqualTo(NotifyDictionaryChangedAction.Reset));
            Assert.That(args.OldItems, Is.Null);
            Assert.That(args.NewItems, Is.Null);
            Assert.That(args.OldKeys, Is.Null);
            Assert.That(args.NewKeys, Is.Null);
        }

        protected NotifyDictionaryChangedEventHandler GenerateAssertDictionaryEventAddOne(TKey key, TItem item) {
            return (sender, args) => {
                Assert.That(args.Action, Is.EqualTo(NotifyDictionaryChangedAction.Add));
                Assert.That(args.OldItems, Is.Null);
                Assert.That(args.OldKeys, Is.Null);
                Assert.That(args.NewKeys[0], Is.EqualTo(key));
                Assert.That(args.NewItems[0], Is.EqualTo(item));
            };
        }

        protected NotifyDictionaryChangedEventHandler GenerateAssertDictionaryEventRemoveOne(TKey key, TItem item) {
            return (sender, args) => {
                Assert.That(args.Action, Is.EqualTo(NotifyDictionaryChangedAction.Remove));
                Assert.That(args.NewKeys, Is.Null);
                Assert.That(args.NewItems, Is.Null);
                Assert.That(args.OldKeys[0], Is.EqualTo(key));
                Assert.That(args.OldItems[0], Is.EqualTo(item));
            };
        }
        #endregion
    }
}
