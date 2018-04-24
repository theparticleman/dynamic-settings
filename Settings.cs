using System;
using System.Collections.Generic;
using System.Dynamic;
using Microsoft.Extensions.Configuration;

namespace DynamicSettingsTest
{
    public class Settings : DynamicObject
    {
        readonly IConfigurationRoot config;

        public Settings()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");
            config = builder.Build();
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (binder.Name.EndsWith("Bool"))
            {
                var settingName = binder.Name.Substring(0, binder.Name.Length - 4);
                result = config.GetValue<bool>(settingName);
            }
            else if (binder.Name.EndsWith("Int"))
            {
                var settingName = binder.Name.Substring(0, binder.Name.Length - 3);
                result = config.GetValue<int>(settingName);
            }
            else
            {
                result = config.GetValue<string>(binder.Name);
            }
            if (result == null) throw new SettingMissingException(binder.Name);
            return true;
        }
    }

    public class OverridableSettings : Settings
    {
        readonly Dictionary<string, object> propertyOverrideValues = new Dictionary<string, object>();
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            propertyOverrideValues[binder.Name] = value;
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (propertyOverrideValues.ContainsKey(binder.Name))
            {
                result = propertyOverrideValues[binder.Name];
                return true;
            }

            return base.TryGetMember(binder, out result);
        }
    }

    public class SettingsWithProperties : Settings
    {
        public string ConstProperty => "const value";
    }

    public class SettingMissingException : Exception
    {
        public SettingMissingException(string settingName) : base($"Setting '{settingName}' missing")
        {
        }
    }
}