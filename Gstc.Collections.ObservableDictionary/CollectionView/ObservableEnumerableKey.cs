using System.Collections.Generic;

namespace Gstc.Collections.ObservableDictionary.CollectionView;
/// <summary>
/// <inheritdoc cref="AbstractObservableEnumerable{TKey, TValue, TOuput}"/>
/// <br/><br/>
/// The enumerable is of type TKey.
/// </summary>
/// <typeparam name="TKey">They TKey of the dictionary and the TItem of the enumerator.</typeparam>
/// <typeparam name="TValue">The TValue of the dictionary.</typeparam>
public class ObservableEnumerableKey<TKey, TValue> : AbstractObservableEnumerable<TKey, TValue, TKey> {
    public ObservableEnumerableKey(IObservableDictionary<TKey, TValue> obvDictionary) : base(obvDictionary) { }
    public override IEnumerator<TKey> GetEnumerator() => _obvDictionary.Keys.GetEnumerator();
}
