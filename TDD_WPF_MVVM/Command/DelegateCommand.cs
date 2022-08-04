using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TDD_WPF_MVVM.Command
{
    public class DelegateCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Predicate<object?>? _canexecute;

        public DelegateCommand(Action<object?> execute, Predicate<object?>? canexecute = null)
        {
            _execute = execute;
            _canexecute = canexecute;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return _canexecute == null || _canexecute(parameter);
        }

        public void Execute(object? parameter)
        {
            _execute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
