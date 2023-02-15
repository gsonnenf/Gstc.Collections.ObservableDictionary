using System;
using System.Windows.Input;

namespace Gstc.Collections.ObservableDictionary.Demo.Model {
    public class SimpleActionCommand : ICommand {

        private event Action? _execute;
        private event Func<bool> _canExecute;
        public event EventHandler? CanExecuteChanged;
        public bool _defaultCanExecute = true;

        public SimpleActionCommand(Action execute, bool defaultCanExecute = true) => _execute = execute;
        public SimpleActionCommand(Action execute, Func<bool> canExecute, bool defaultCanExecute = true) {
            _execute = execute;
            _canExecute = canExecute;
            _defaultCanExecute = defaultCanExecute;
        }

        public bool CanExecute(object? parameter) => (_canExecute != null) ? _canExecute() : _defaultCanExecute;
        public void Execute(object? parameter) => _execute?.Invoke();
        public void CallCanExecuteChanged() => CanExecuteChanged?.Invoke(this, null);

    }
}
