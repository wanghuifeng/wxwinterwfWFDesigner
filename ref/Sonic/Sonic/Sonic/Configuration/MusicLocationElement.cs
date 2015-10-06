using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Sonic
{
    /// <summary>
    /// The class that holds information for a single 
    /// element returned by the configuration manager.
    /// </summary>
    public class MusicLocationElement 
        : ConfigurationElement
    {
        #region Public Properties
        [ConfigurationProperty("musicLocationPath", 
            DefaultValue = "", IsKey = true, 
            IsRequired = true)]
        public string musicPath
        {
            get { return ((string)(base["musicLocationPath"])); }
            set { base["musicLocationPath"] = value; }
        }
        #endregion
    }
}
