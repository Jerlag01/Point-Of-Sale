using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Util.WpfMessageBox
{
    public class DelegateCommand : ICommand
    {
        private readonly Action m_ExecuteMethod;
        private readonly Func<bool> m_CanExecuteMethod;
        private bool m_IsAutomaticRequeryDisabled;
        private List<WeakReference> m_CanExecuteChangedHandlers;

        public DelegateCommand(Action executeMethod)
          : this(executeMethod, (Func<bool>)null, false)
        {
        }

        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod)
          : this(executeMethod, canExecuteMethod, false)
        {
        }

        public DelegateCommand(
          Action executeMethod,
          Func<bool> canExecuteMethod,
          bool isAutomaticRequeryDisabled)
        {
            if (executeMethod == null)
                throw new ArgumentNullException(nameof(executeMethod));
            this.m_ExecuteMethod = executeMethod;
            this.m_CanExecuteMethod = canExecuteMethod;
            this.m_IsAutomaticRequeryDisabled = isAutomaticRequeryDisabled;
        }

        public bool CanExecute()
        {
            return this.m_CanExecuteMethod == null || this.m_CanExecuteMethod();
        }

        public void Execute()
        {
            Action executeMethod = this.m_ExecuteMethod;
            if (executeMethod == null)
                return;
            executeMethod();
        }

        public bool IsAutomaticRequeryDisabled
        {
            get
            {
                return this.m_IsAutomaticRequeryDisabled;
            }
            set
            {
                if (this.m_IsAutomaticRequeryDisabled == value)
                    return;
                if (value)
                    CommandManagerHelper.RemoveHandlersFromRequerySuggested(this.m_CanExecuteChangedHandlers);
                else
                    CommandManagerHelper.AddHandlersToRequerySuggested(this.m_CanExecuteChangedHandlers);
                this.m_IsAutomaticRequeryDisabled = value;
            }
        }

        public void RaiseCanExecuteChanged()
        {
            this.OnCanExecuteChanged();
        }

        protected virtual void OnCanExecuteChanged()
        {
            CommandManagerHelper.CallWeakReferenceHandlers(this.m_CanExecuteChangedHandlers);
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (!this.m_IsAutomaticRequeryDisabled)
                    CommandManager.RequerySuggested += value;
                CommandManagerHelper.AddWeakReferenceHandler(ref this.m_CanExecuteChangedHandlers, value, 2);
            }
            remove
            {
                if (!this.m_IsAutomaticRequeryDisabled)
                    CommandManager.RequerySuggested -= value;
                CommandManagerHelper.RemoveWeakReferenceHandler(this.m_CanExecuteChangedHandlers, value);
            }
        }

        bool ICommand.CanExecute(object parameter)
        {
            return this.CanExecute();
        }

        void ICommand.Execute(object parameter)
        {
            this.Execute();
        }
    }
}
