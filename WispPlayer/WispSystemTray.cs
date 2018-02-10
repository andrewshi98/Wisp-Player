using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WispPlayer
{
    class WispSystemTray
    {
        public ICommand Menu_Show
        {
            get
            {
                return new DelegateCommand
                {
                    CommandAction = () => System.Windows.Application.Current.Shutdown()
                };
            }
        }

        public ICommand Menu_Exit
        {
            get
            {
                return new DelegateCommand
                {
                    CommandAction = () => System.Windows.Application.Current.Shutdown()
                };
            }
        }
    }

    public class DelegateCommand : ICommand
    {
        public Action CommandAction { get; set; }
        public Func<bool> CanExecuteFunc { get; set; }

        public void Execute(object parameter)
        {
            CommandAction();
        }

        public bool CanExecute(object parameter)
        {
            return CanExecuteFunc == null || CanExecuteFunc();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}