using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Sonic
{
    /// <summary>
    /// The Class that will have the XML config file data 
    /// loaded into it via the configuration Manager.
    /// </summary>
    public class MusicLocationLookupConfigSection : ConfigurationSection
    {
        #region Public Properties
        /// <summary>
        /// The value of the property here "SkinNameToTheme" 
        /// needs to match that of the config file section
        /// </summary>
        [ConfigurationProperty("MusicRepository")]
        public MusicLocationElementCollection MusicLocations
        {
            get { return 
                ((MusicLocationElementCollection)
                    (base["MusicRepository"])); }
        }
        #endregion
    }
}
