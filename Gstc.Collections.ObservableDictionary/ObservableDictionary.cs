﻿using Gstc.Collections.ObservableDictionary.Abstract;
using Gstc.Collections.ObservableDictionary.Notify;
using System.Collections.Generic;

namespace Gstc.Collections.ObservableDictionary {

    /// <summary>
    /// Observable dictionary can serve as a stand alone dictionary, or serve as an observable wrapper for a pre-existing dictionary.
    /// ObservableDictionary implements INotifyPropertyChanged and INotifyDictionaryChanged. It does NOT implement ICollectionChanged.
    /// Use an ObservableKeyedCollection or SortedList if you need ICollectionChanged.
    /// 
    /// </summary>
    /// <typeparam name="TKey">Key field of Dictionary</typeparam>
    /// <typeparam name="TValue">Value field of Dictionary</typeparam>
    public class ObservableDictionary<TKey, TValue> :
        AbstractObservableIDictionary<TKey, TValue, Dictionary<TKey, TValue>, NotifyDictionary> { }
}
