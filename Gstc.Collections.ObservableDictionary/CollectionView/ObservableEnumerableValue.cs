using System.Collections.Generic;

namespace Gstc.Collections.ObservableDictionary.CollectionView;

/// <summary>
/// <inheritdoc cref="AbstractObservableEnumerable{TKey, TValue, TOuput}"/>
/// <br/><br/>
/// The enumerable is of type TValue.
/// </summary>
/// <typeparam name="TKey">They TKey of the dictionary.</typeparam>
/// <typeparam name="TValue">The TValue of the dictionary and the TItem of the enumerator.</typeparam>
public class ObservableEnumerableValue<TKey, TValue> : AbstractObservableEnumerable<TKey, TValue, TValue> {
    public ObservableEnumerableValue(IObservableDictionary<TKey, TValue> obvDictionary) : base(obvDictionary) { }
    public override IEnumerator<TValue> GetEnumerator() => _obvDictionary.Values.GetEnumerator();
}
