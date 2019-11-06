using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ICMS_Server
{
    public class RelayCommand : ICommand
    {
        private Action<object> _action { get; set; }
        private Func<bool> _func { get; set; }

        public RelayCommand(Action<object> action, Func<bool> func = null)
        {
            _action = action;
            _func = func;
        }

        public event EventHandler CanExecuteChanged = (sender, e) => { };

        public bool CanExecute(object parameter)
        {
            return _func == null ? true : _func();
        }

        public void Execute(object parameter)
        {
            _action(parameter);
        }
    }
}
