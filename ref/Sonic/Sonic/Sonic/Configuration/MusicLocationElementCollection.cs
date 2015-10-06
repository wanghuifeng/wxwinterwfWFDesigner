using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Sonic
{
    /// <summary>
    /// A MusicLocation collection class that will store the 
    /// list of each MusicLocationElement item that is 
    /// returned back from the configuration manager.
    /// </summary>
    [ConfigurationCollection(typeof(MusicLocationElement))]
    public class MusicLocationElementCollection 
        : ConfigurationElementCollection
    {
        #region Overrides
        protected override ConfigurationElement 
            CreateNewElement()
        {
            return new MusicLocationElement();
        }


        protected override object GetElementKey(
            ConfigurationElement element)
        {
            return ((MusicLocationElement)(element)).musicPath;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets ThemeElement at the index provided
        /// </summary>
        public MusicLocationElement this[int idx]
        {
            get
            {
                return (MusicLocationElement)BaseGet(idx);
            }
        }
        #endregion
    }
}
