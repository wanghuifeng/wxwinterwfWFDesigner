using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Sonic
{
    /// <summary>
    /// MP3File clicked event args
    /// </summary>
    public class MP3FileClickedEventArgs : RoutedEventArgs
    {
        #region Instance fields
        public String FileName { get; private set; }
        #endregion

        #region Ctor
        /// <summary>
        /// Constructs a new MP3FileClickedEventArgs object
        /// using the parameters provided
        /// </summary>
        /// <param name="fileName">the fileName of the MP3</param>
        public MP3FileClickedEventArgs(RoutedEvent routedEvent,
            String fileName)
            : base(routedEvent)
        {
            this.FileName = fileName;
        }
        #endregion
    }

}
