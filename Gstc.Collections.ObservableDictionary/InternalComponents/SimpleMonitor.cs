using System;

namespace Gstc.Collections.ObservableDictionary.Abstract {
    internal class SimpleMonitor : IDisposable {
        internal bool AllowReentrancy { get; set; }
        private int _blockReentrancyCount = 0;

        public SimpleMonitor BlockReentrancy() {
            if (_blockReentrancyCount > 0 && !AllowReentrancy) throw new ReentrancyException("ReentrancyNotAllowed");
            _blockReentrancyCount++;
            return this;
        }

        public void Dispose() => _blockReentrancyCount--;
    }
}
