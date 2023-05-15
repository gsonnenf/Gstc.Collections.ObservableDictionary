using System.Collections.Generic;

namespace Gstc.Collections.ObservableDictionary.DictionaryEnumerable;

/// <summary>
/// A <see cref="ObservableEnumerableDictionaryAbstract{TKey, TValue, TOuput}"></see> where the enumerable is of 
/// type KeyValuePair{TKey,TValue}.
/// <br/><br/> 
/// <inheritdoc cref="ObservableEnumerableDictionaryAbstract{TKey, TValue, TOuput}"/>
/// </summary>
/// <typeparam name="TKey">The TKey of the dictionary and TKey of enumerable KeyValuePair{TKey,TValue}.</typeparam>
/// <typeparam name="TValue">The TValue of the dictionary and TValue of enumerable KeyValuePair{TKey,TValue}.</typeparam>
public class ObservableEnumerableDictionaryKvp<TKey, TValue> : ObservableEnumerableDictionaryAbstract<TKey, TValue, KeyValuePair<TKey, TValue>> {
    public ObservableEnumerableDictionaryKvp(IObservableDictionary<TKey, TValue> obvDictionary) : base(obvDictionary) { }
    public override IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _obvDictionary.GetEnumerator();
}
