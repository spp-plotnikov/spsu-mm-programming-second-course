using System;
using System.Windows.Input;

namespace InstClient.Command
{
    public class SimpleCommand : ICommand
    {
        private Action _command;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _command.Invoke();
        }

        public SimpleCommand(Action command)
        {
            _command = command;
        }

        public event EventHandler CanExecuteChanged;
    }
}