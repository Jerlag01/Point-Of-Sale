using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Util.MicroMvvm
{
    [Serializable]
    public abstract class ObservableObject : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if (propertyChanged == null)
                return;
            propertyChanged((object)this, e);
        }

        protected void OnPropertyChanged<T>(Expression<Func<T>> propertyExpresssion)
        {
            this.OnPropertyChanged(PropertySupport.ExtractPropertyName<T>(propertyExpresssion));
        }

        protected void OnPropertyChanged(string propertyName)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            PropertyDescriptor property = TypeDescriptor.GetProperties((object)this)[propertyName];
        }

        public void Dispose()
        {
            this.OnDispose();
        }

        protected virtual void OnDispose()
        {
        }
    }
}