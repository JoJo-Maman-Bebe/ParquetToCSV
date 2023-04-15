using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace FileReaderAPI
{
    public class FileSystemSettings : ConfigurationSection
    {
        [ConfigurationProperty("rootPath", DefaultValue = "", IsRequired = true)]
        public string RootPath
        {
            get { return (string)this["rootPath"]; }
            set { this["rootPath"] = value; }
        }
    }
}