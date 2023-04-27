using System.Collections.Generic;

namespace Gstc.Collections.ObservableDictionary.DictionaryEnumerable;
/// <summary>
/// <inheritdoc cref="ObservableEnumerableDictionaryAbstract{TKey, TValue, TOuput}"/>
/// <br/><br/>
/// The enumerable is of type TKey.
/// </summary>
/// <typeparam name="TKey">They TKey of the dictionary and the TItem of the enumerator.</typeparam>
/// <typeparam name="TValue">The TValue of the dictionary.</typeparam>
public class ObservableEnumerableDictionaryKey<TKey, TValue> : ObservableEnumerableDictionaryAbstract<TKey, TValue, TKey> {
    public ObservableEnumerableDictionaryKey(IObservableDictionary<TKey, TValue> obvDictionary) : base(obvDictionary) { }
    public override IEnumerator<TKey> GetEnumerator() => _obvDictionary.Keys.GetEnumerator();
}
