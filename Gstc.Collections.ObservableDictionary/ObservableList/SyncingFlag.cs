using System;

namespace Gstc.Collections.ObservableDictionary.ObservableList {

    /// <summary>
    /// Todo: Move this To ObservableList
    /// </summary>
    public class SyncingFlag : IDisposable {

        public bool InProgress = false;
        public SyncingFlag Begin() {
            InProgress = true;
            return this;
        }
        public void Dispose() => InProgress = false;
    }
}
