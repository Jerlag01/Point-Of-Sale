using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Util.WpfMessageBox
{
    public class DelegateCommand<T> : ICommand
    {
        private readonly Action<T> m_ExecuteMethod;
        private readonly Func<T, bool> m_CanExecuteMethod;
        private bool m_IsAutomaticRequeryDisabled;
        private List<WeakReference> m_CanExecuteChangedHandlers;

        public DelegateCommand(Action<T> executeMethod)
          : this(executeMethod, (Func<T, bool>)null, false)
        {
        }

        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
          : this(executeMethod, canExecuteMethod, false)
        {
        }

        public DelegateCommand(
          Action<T> executeMethod,
          Func<T, bool> canExecuteMethod,
          bool isAutomaticRequeryDisabled)
        {
            if (executeMethod == null)
                throw new ArgumentNullException(nameof(executeMethod));
            this.m_ExecuteMethod = executeMethod;
            this.m_CanExecuteMethod = canExecuteMethod;
            this.m_IsAutomaticRequeryDisabled = isAutomaticRequeryDisabled;
        }

        public bool CanExecute(T parameter)
        {
            return this.m_CanExecuteMethod == null || this.m_CanExecuteMethod(parameter);
        }

        public void Execute(T parameter)
        {
            Action<T> executeMethod = this.m_ExecuteMethod;
            if (executeMethod == null)
                return;
            executeMethod(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            this.OnCanExecuteChanged();
        }

        protected virtual void OnCanExecuteChanged()
        {
            CommandManagerHelper.CallWeakReferenceHandlers(this.m_CanExecuteChangedHandlers);
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
            return parameter == null && typeof(T).IsValueType ? this.m_CanExecuteMethod == null : this.CanExecute((T)parameter);
        }

        void ICommand.Execute(object parameter)
        {
            this.Execute((T)parameter);
        }
    }
}
