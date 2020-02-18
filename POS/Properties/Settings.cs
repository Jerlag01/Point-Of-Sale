using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Pos.Properties
{
    [CompilerGenerated]
    [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed class Settings : ApplicationSettingsBase
    {
        private static Settings defaultInstance = (Settings)SettingsBase.Synchronized((SettingsBase)new Settings());

        public static Settings Default
        {
            get
            {
                return Settings.defaultInstance;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("localhost\\SQLEXPRESS")]
        public string DbInstance
        {
            get
            {
                return (string)this[nameof(DbInstance)];
            }
            set
            {
                this[nameof(DbInstance)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("TjokERP")]
        public string DbCatalog
        {
            get
            {
                return (string)this[nameof(DbCatalog)];
            }
            set
            {
                this[nameof(DbCatalog)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("tjokerp")]
        public string DbUser
        {
            get
            {
                return (string)this[nameof(DbUser)];
            }
            set
            {
                this[nameof(DbUser)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("tjokerp")]
        public string DbPassword
        {
            get
            {
                return (string)this[nameof(DbPassword)];
            }
            set
            {
                this[nameof(DbPassword)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("True")]
        public bool UpdateSettings
        {
            get
            {
                return (bool)this[nameof(UpdateSettings)];
            }
            set
            {
                this[nameof(UpdateSettings)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("EN")]
        public string Language
        {
            get
            {
                return (string)this[nameof(Language)];
            }
            set
            {
                this[nameof(Language)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("")]
        public string SerialPortCashDrawer
        {
            get
            {
                return (string)this[nameof(SerialPortCashDrawer)];
            }
            set
            {
                this[nameof(SerialPortCashDrawer)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("2016-03-05")]
        public DateTime MemberCardStartDate
        {
            get
            {
                return (DateTime)this[nameof(MemberCardStartDate)];
            }
            set
            {
                this[nameof(MemberCardStartDate)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("395.00:00:00")]
        public TimeSpan MemberCardValidityPeriod
        {
            get
            {
                return (TimeSpan)this[nameof(MemberCardValidityPeriod)];
            }
            set
            {
                this[nameof(MemberCardValidityPeriod)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("")]
        public string EidReaderName
        {
            get
            {
                return (string)this[nameof(EidReaderName)];
            }
            set
            {
                this[nameof(EidReaderName)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("")]
        public string CardPrinterName
        {
            get
            {
                return (string)this[nameof(CardPrinterName)];
            }
            set
            {
                this[nameof(CardPrinterName)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("")]
        public string NfcReaderName
        {
            get
            {
                return (string)this[nameof(NfcReaderName)];
            }
            set
            {
                this[nameof(NfcReaderName)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("5000")]
        public int AuthenticationTimeout
        {
            get
            {
                return (int)this[nameof(AuthenticationTimeout)];
            }
            set
            {
                this[nameof(AuthenticationTimeout)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("")]
        public string EmailSmtpServer
        {
            get
            {
                return (string)this[nameof(EmailSmtpServer)];
            }
            set
            {
                this[nameof(EmailSmtpServer)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("")]
        public string EmailFromAddress
        {
            get
            {
                return (string)this[nameof(EmailFromAddress)];
            }
            set
            {
                this[nameof(EmailFromAddress)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("")]
        public string EmailFromName
        {
            get
            {
                return (string)this[nameof(EmailFromName)];
            }
            set
            {
                this[nameof(EmailFromName)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("")]
        public string EmailToAddress
        {
            get
            {
                return (string)this[nameof(EmailToAddress)];
            }
            set
            {
                this[nameof(EmailToAddress)] = (object)value;
            }
        }

        private void SettingChangingEventHandler(object sender, SettingChangingEventArgs e)
        {
        }

        private void SettingsSavingEventHandler(object sender, CancelEventArgs e)
        {
        }
    }
}
