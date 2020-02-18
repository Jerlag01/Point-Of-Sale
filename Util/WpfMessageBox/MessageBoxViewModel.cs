using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Util.WpfMessageBox
{
    internal class MessageBoxViewModel : INotifyPropertyChanged
    {
        private bool _isOkDefault;
        private bool _isYesDefault;
        private bool _isNoDefault;
        private bool _isCancelDefault;
        private string _title;
        private string _message;
        private MessageBoxButton _buttonOption;
        private MessageBoxOptions _options;
        private Visibility _yesNoVisibility;
        private Visibility _cancelVisibility;
        private Visibility _okVisibility;
        private HorizontalAlignment _contentTextAlignment;
        private FlowDirection _contentFlowDirection;
        private FlowDirection _titleFlowDirection;
        private ICommand _yesCommand;
        private ICommand _noCommand;
        private ICommand _cancelCommand;
        private ICommand _okCommand;
        private ICommand _escapeCommand;
        private ICommand _closeCommand;
        private WPFMessageBoxWindow _view;
        private ImageSource _messageImageSource;

        public MessageBoxResult Result { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MessageBoxViewModel(
          WPFMessageBoxWindow view,
          string title,
          string message,
          MessageBoxButton buttonOption,
          MessageBoxImage image,
          MessageBoxResult defaultResult,
          MessageBoxOptions options)
        {
            this.Title = title;
            this.Message = message;
            this.ButtonOption = buttonOption;
            this.Options = options;
            this.SetDirections(options);
            this.SetButtonVisibility(buttonOption);
            this.SetImageSource(image);
            this.SetButtonDefault(defaultResult);
            this._view = view;
        }

        public MessageBoxButton ButtonOption
        {
            get
            {
                return this._buttonOption;
            }
            set
            {
                if (this._buttonOption == value)
                    return;
                this._buttonOption = value;
                this.NotifyPropertyChange(nameof(ButtonOption));
            }
        }

        public MessageBoxOptions Options
        {
            get
            {
                return this._options;
            }
            set
            {
                if (this._options == value)
                    return;
                this._options = value;
                this.NotifyPropertyChange(nameof(Options));
            }
        }

        public string Title
        {
            get
            {
                return this._title;
            }
            set
            {
                if (!(this._title != value))
                    return;
                this._title = value;
                this.NotifyPropertyChange(nameof(Title));
            }
        }

        public string Message
        {
            get
            {
                return this._message;
            }
            set
            {
                if (!(this._message != value))
                    return;
                this._message = value;
                this.NotifyPropertyChange(nameof(Message));
            }
        }

        public ImageSource MessageImageSource
        {
            get
            {
                return this._messageImageSource;
            }
            set
            {
                this._messageImageSource = value;
                this.NotifyPropertyChange(nameof(MessageImageSource));
            }
        }

        public Visibility YesNoVisibility
        {
            get
            {
                return this._yesNoVisibility;
            }
            set
            {
                if (this._yesNoVisibility == value)
                    return;
                this._yesNoVisibility = value;
                this.NotifyPropertyChange(nameof(YesNoVisibility));
            }
        }

        public Visibility CancelVisibility
        {
            get
            {
                return this._cancelVisibility;
            }
            set
            {
                if (this._cancelVisibility == value)
                    return;
                this._cancelVisibility = value;
                this.NotifyPropertyChange(nameof(CancelVisibility));
            }
        }

        public Visibility OkVisibility
        {
            get
            {
                return this._okVisibility;
            }
            set
            {
                if (this._okVisibility == value)
                    return;
                this._okVisibility = value;
                this.NotifyPropertyChange(nameof(OkVisibility));
            }
        }

        public HorizontalAlignment ContentTextAlignment
        {
            get
            {
                return this._contentTextAlignment;
            }
            set
            {
                if (this._contentTextAlignment == value)
                    return;
                this._contentTextAlignment = value;
                this.NotifyPropertyChange(nameof(ContentTextAlignment));
            }
        }

        public FlowDirection ContentFlowDirection
        {
            get
            {
                return this._contentFlowDirection;
            }
            set
            {
                if (this._contentFlowDirection == value)
                    return;
                this._contentFlowDirection = value;
                this.NotifyPropertyChange(nameof(ContentFlowDirection));
            }
        }

        public FlowDirection TitleFlowDirection
        {
            get
            {
                return this._titleFlowDirection;
            }
            set
            {
                if (this._titleFlowDirection == value)
                    return;
                this._titleFlowDirection = value;
                this.NotifyPropertyChange(nameof(TitleFlowDirection));
            }
        }

        public bool IsOkDefault
        {
            get
            {
                return this._isOkDefault;
            }
            set
            {
                if (this._isOkDefault == value)
                    return;
                this._isOkDefault = value;
                this.NotifyPropertyChange(nameof(IsOkDefault));
            }
        }

        public bool IsYesDefault
        {
            get
            {
                return this._isYesDefault;
            }
            set
            {
                if (this._isYesDefault == value)
                    return;
                this._isYesDefault = value;
                this.NotifyPropertyChange(nameof(IsYesDefault));
            }
        }

        public bool IsNoDefault
        {
            get
            {
                return this._isNoDefault;
            }
            set
            {
                if (this._isNoDefault == value)
                    return;
                this._isNoDefault = value;
                this.NotifyPropertyChange(nameof(IsNoDefault));
            }
        }

        public bool IsCancelDefault
        {
            get
            {
                return this._isCancelDefault;
            }
            set
            {
                if (this._isCancelDefault == value)
                    return;
                this._isCancelDefault = value;
                this.NotifyPropertyChange(nameof(IsCancelDefault));
            }
        }

        public ICommand YesCommand
        {
            get
            {
                if (this._yesCommand == null)
                    this._yesCommand = (ICommand)new DelegateCommand((Action)(() =>
                    {
                        this.Result = MessageBoxResult.Yes;
                        this._view.Close();
                    }));
                return this._yesCommand;
            }
        }

        public ICommand NoCommand
        {
            get
            {
                if (this._noCommand == null)
                    this._noCommand = (ICommand)new DelegateCommand((Action)(() =>
                    {
                        this.Result = MessageBoxResult.No;
                        this._view.Close();
                    }));
                return this._noCommand;
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                if (this._cancelCommand == null)
                    this._cancelCommand = (ICommand)new DelegateCommand((Action)(() =>
                    {
                        this.Result = MessageBoxResult.Cancel;
                        this._view.Close();
                    }));
                return this._cancelCommand;
            }
        }

        public ICommand OkCommand
        {
            get
            {
                if (this._okCommand == null)
                    this._okCommand = (ICommand)new DelegateCommand((Action)(() =>
                    {
                        this.Result = MessageBoxResult.OK;
                        this._view.Close();
                    }));
                return this._okCommand;
            }
        }

        public ICommand EscapeCommand
        {
            get
            {
                if (this._escapeCommand == null)
                    this._escapeCommand = (ICommand)new DelegateCommand((Action)(() =>
                    {
                        switch (this.ButtonOption)
                        {
                            case MessageBoxButton.OK:
                                this.Result = MessageBoxResult.OK;
                                this._view.Close();
                                break;
                            case MessageBoxButton.OKCancel:
                            case MessageBoxButton.YesNoCancel:
                                this.Result = MessageBoxResult.Cancel;
                                this._view.Close();
                                break;
                        }
                    }));
                return this._escapeCommand;
            }
        }

        public ICommand CloseCommand
        {
            get
            {
                if (this._closeCommand == null)
                    this._closeCommand = (ICommand)new DelegateCommand((Action)(() =>
                    {
                        if (this.Result != MessageBoxResult.None)
                            return;
                        switch (this.ButtonOption)
                        {
                            case MessageBoxButton.OK:
                                this.Result = MessageBoxResult.OK;
                                break;
                            case MessageBoxButton.OKCancel:
                            case MessageBoxButton.YesNoCancel:
                                this.Result = MessageBoxResult.Cancel;
                                break;
                        }
                    }));
                return this._closeCommand;
            }
        }

        private void SetDirections(MessageBoxOptions options)
        {
            switch (options)
            {
                case MessageBoxOptions.None:
                    this.ContentTextAlignment = HorizontalAlignment.Left;
                    this.ContentFlowDirection = FlowDirection.LeftToRight;
                    this.TitleFlowDirection = FlowDirection.LeftToRight;
                    break;
                case MessageBoxOptions.RightAlign:
                    this.ContentTextAlignment = HorizontalAlignment.Right;
                    this.ContentFlowDirection = FlowDirection.LeftToRight;
                    this.TitleFlowDirection = FlowDirection.LeftToRight;
                    break;
                case MessageBoxOptions.RtlReading:
                    this.ContentTextAlignment = HorizontalAlignment.Right;
                    this.ContentFlowDirection = FlowDirection.RightToLeft;
                    this.TitleFlowDirection = FlowDirection.RightToLeft;
                    break;
                case MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading:
                    this.ContentTextAlignment = HorizontalAlignment.Left;
                    this.ContentFlowDirection = FlowDirection.RightToLeft;
                    this.TitleFlowDirection = FlowDirection.RightToLeft;
                    break;
            }
        }

        private void NotifyPropertyChange(string property)
        {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if (propertyChanged == null)
                return;
            propertyChanged((object)this, new PropertyChangedEventArgs(property));
        }

        private void SetButtonDefault(MessageBoxResult defaultResult)
        {
            switch (defaultResult)
            {
                case MessageBoxResult.OK:
                    this.IsOkDefault = true;
                    break;
                case MessageBoxResult.Cancel:
                    this.IsCancelDefault = true;
                    break;
                case MessageBoxResult.Yes:
                    this.IsYesDefault = true;
                    break;
                case MessageBoxResult.No:
                    this.IsNoDefault = true;
                    break;
            }
        }

        private void SetButtonVisibility(MessageBoxButton buttonOption)
        {
            switch (buttonOption)
            {
                case MessageBoxButton.OK:
                    this.YesNoVisibility = this.CancelVisibility = Visibility.Collapsed;
                    break;
                case MessageBoxButton.OKCancel:
                    this.YesNoVisibility = Visibility.Collapsed;
                    break;
                case MessageBoxButton.YesNoCancel:
                    this.OkVisibility = Visibility.Collapsed;
                    break;
                case MessageBoxButton.YesNo:
                    this.OkVisibility = this.CancelVisibility = Visibility.Collapsed;
                    break;
                default:
                    this.OkVisibility = this.CancelVisibility = this.YesNoVisibility = Visibility.Collapsed;
                    break;
            }
        }

        private void SetImageSource(MessageBoxImage image)
        {
            switch (image)
            {
                case MessageBoxImage.Hand:
                    this.MessageImageSource = SystemIcons.Error.ToImageSource();
                    break;
                case MessageBoxImage.Question:
                    this.MessageImageSource = SystemIcons.Question.ToImageSource();
                    break;
                case MessageBoxImage.Exclamation:
                    this.MessageImageSource = SystemIcons.Warning.ToImageSource();
                    break;
                case MessageBoxImage.Asterisk:
                    this.MessageImageSource = SystemIcons.Information.ToImageSource();
                    break;
                default:
                    this.MessageImageSource = (ImageSource)null;
                    break;
            }
        }
    }
}
