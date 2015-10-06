using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Sonic
{
    /// <summary>
    /// Album clicked event args
    /// </summary>
    public class AlbumClickedEventArgs : RoutedEventArgs
    {
        #region Instance fields
        public AlbumOfMP3ViewModel ClickedAlbum { get; private set; }
        #endregion

        #region Ctor
        /// <summary>
        /// Constructs a new AlbumClickedEventArgs object
        /// using the parameters provided
        /// </summary>
        /// <param name="clickedAlbum">the AlbumOfMP3ViewModel for the events args</param>
        public AlbumClickedEventArgs(RoutedEvent routedEvent,
            AlbumOfMP3ViewModel clickedAlbum)
            : base(routedEvent)
        {
            this.ClickedAlbum = clickedAlbum;
        }
        #endregion
    }

}
