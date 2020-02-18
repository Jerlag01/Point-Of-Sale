using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Pos.Services.Web.Properties
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
        [DefaultSettingValue("WebApiUser")]
        public string RemoteLogin
        {
            get
            {
                return (string)this[nameof(RemoteLogin)];
            }
            set
            {
                this[nameof(RemoteLogin)] = (object)value;
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("P4$$w0rd")]
        public string RemotePassword
        {
            get
            {
                return (string)this[nameof(RemotePassword)];
            }
            set
            {
                this[nameof(RemotePassword)] = (object)value;
            }
        }
    }
}
