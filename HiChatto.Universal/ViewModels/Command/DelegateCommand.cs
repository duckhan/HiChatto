using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HiChatto.Universal.ViewModels.Command
{
    public class DelegateCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private SimpleCommandDelegate _handler;
        private bool _isEnable;
        public bool IsEnable
        {
            get { return _isEnable; }
        }
        public delegate void SimpleCommandDelegate();
        public DelegateCommand (SimpleCommandDelegate handler)
        {
            _handler = handler;
            if (_handler != null)
            {
                _isEnable = true;
            }
        }
        public bool CanExecute(object parameter)
        {
          //  CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            return _isEnable;
        }

        public void Execute(object parameter)
        {
            _handler?.Invoke();
        }
    }
}
