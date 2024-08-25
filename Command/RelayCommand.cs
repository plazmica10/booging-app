using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.Command
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) { return _canExecute == null || _canExecute(parameter); }

        public void Execute(object? parameter) { _execute(parameter); }

        public void RaiseCanExecuteChanged() { CanExecuteChanged?.Invoke(this, EventArgs.Empty); }
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool>? _canExecute;

        public RelayCommand(Action<T> execute, Func<T, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) { return _canExecute == null || _canExecute.Invoke((T)(parameter ?? default(T)!)); }

        public void Execute(object? parameter) { _execute.Invoke((T)(parameter ?? default(T)!)); }

        public void RaiseCanExecuteChanged() { CanExecuteChanged?.Invoke(this, EventArgs.Empty); }
    }
}
