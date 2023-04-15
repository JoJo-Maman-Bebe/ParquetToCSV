using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace FileReaderAPI.Helpers
{
    public class ConfigurationHelper
    {
        public string GetCustomConfigValue(string configSectionName, string keyName)
        {
            string environmentValue = GetEnvironmentValue();
            if (environmentValue == "Live")
            {
                NameValueCollection customSection = (NameValueCollection)System.Configuration.ConfigurationManager.GetSection(configSectionName);
                return customSection[keyName];
            }
            else
            {
                NameValueCollection customSection = (NameValueCollection)System.Configuration.ConfigurationManager.GetSection(configSectionName + "Test");
                return (customSection[keyName]);
            }
            

            
        }

        public string GetEnvironmentValue()
        {
            return System.Configuration.ConfigurationManager.AppSettings["Environment"];
        }

        public string GetAppSettingsValue( string keyName)
        {

            return System.Configuration.ConfigurationManager.AppSettings[keyName];
        }


    }
}