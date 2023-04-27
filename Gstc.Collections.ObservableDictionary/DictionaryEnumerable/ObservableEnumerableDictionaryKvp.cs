using System.Collections.Generic;

namespace Gstc.Collections.ObservableDictionary.DictionaryEnumerable;

/// <summary>
/// <inheritdoc cref="ObservableEnumerableDictionaryAbstract{TKey, TValue, TOuput}"/>
/// <br/><br/>
/// he enumerable is of type KeyValuePair{TKey,TValue}.
/// </summary>
/// <typeparam name="TKey">The TKey of the dictionary and TKey of enumerable KeyValuePair{TKey,TValue}.</typeparam>
/// <typeparam name="TValue">The TValue of the dictionary and TValue of enumerable KeyValuePair{TKey,TValue}.</typeparam>
public class ObservableEnumerableDictionaryKvp<TKey, TValue> : ObservableEnumerableDictionaryAbstract<TKey, TValue, KeyValuePair<TKey, TValue>> {
    public ObservableEnumerableDictionaryKvp(IObservableDictionary<TKey, TValue> obvDictionary) : base(obvDictionary) { }
    public override IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _obvDictionary.GetEnumerator();
}
