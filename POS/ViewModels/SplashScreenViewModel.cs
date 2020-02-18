using System;
using System.Linq.Expressions;
using Pos.Properties;
using Util.MicroMvvm;

namespace Pos.ViewModels
{
    public class SplashScreenViewModel : ObservableObject
    {
        private string splashScreenText = Resources.SplashScreenLoadingDb;

        public string SplashScreenText
        {
            get
            {
                return this.splashScreenText;
            }
            set
            {
                this.splashScreenText = value;
                this.OnPropertyChanged<string>((Expression<Func<string>>)(() => this.SplashScreenText));
            }
        }
    }
}