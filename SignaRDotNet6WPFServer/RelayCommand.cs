using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace SignaRDotNet6WPFServer
{
    public class RelayCommand : ICommand
    {
        readonly Func<Boolean> canExecute;
        readonly Action execute;

        public RelayCommand(Action execute) : this(execute, null)
        {

        }
        public RelayCommand(Action execute, Func<Boolean> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentException("execute");
            }
            this.execute = execute;
            this.canExecute = canExecute;

        }



        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (canExecute != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }
            remove
            {
                if (canExecute != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return canExecute == null ? true : canExecute();
        }
        public void Execute(object parameter)
        {
            if (execute != null)
            {
                execute();
            }
        }
    }
    public class RelayWithParasCommand<T> : ICommand
    {
        readonly Func<object, Boolean> canExecute;
        readonly Action<T> execute;

        public RelayWithParasCommand(Action<T> execute) : this(execute, null)
        {

        }
        public RelayWithParasCommand(Action<T> execute, Func<object, Boolean> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentException("execute");
            }
            this.execute = execute;
            this.canExecute = canExecute;

        }



        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (canExecute != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }
            remove
            {
                if (canExecute != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return canExecute == null ? true : canExecute(parameter);
        }



        public void Execute(object parameter)
        {
            if (execute != null)
            {
                if (parameter != null)
                {
                    execute((T)parameter);
                }
                else
                {
                    execute(default(T));
                }
            }
        }
    }
}
