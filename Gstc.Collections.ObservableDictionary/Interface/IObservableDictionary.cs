﻿using Gstc.Collections.ObservableDictionary.NotificationDictionary;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Gstc.Collections.ObservableDictionary.Interface {
    public interface IObservableDictionary<TKey, TValue> :
        IDictionary,
        IDictionary<TKey, TValue>,
        INotifyDictionaryChanged,
        INotifyPropertyChanged {
    }
}
