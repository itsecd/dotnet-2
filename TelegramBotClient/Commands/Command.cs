using System;
using System.Windows.Input;

namespace TelegramBotClient.Commands
{
    public class Command : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public Command(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute != null && _canExecute(parameter);

        public void Execute(object parameter)
        {
            _execute.Invoke(parameter);
        }
    }
}
