using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Sonic
{
    [DebuggerDisplay("{ToString()}")]
    public class MP3File : INotifyPropertyChanged
    {
        #region Data
        private String fileName = String.Empty;
        private String album = String.Empty;
        private String artist = String.Empty;
        private String genreName = String.Empty;
        private String title = String.Empty;
        #endregion

        #region Public Properties

        public String FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                NotifyPropertyChanged("FileName");
            }
        }


        public String Album
        {
            get { return album; }
            set
            {
                album = value;
                NotifyPropertyChanged("Album");
            }
        }

        public String Artist
        {
            get { return artist; }
            set
            {
                artist = value;
                NotifyPropertyChanged("Artist");
            }
        }

        public String GenreName
        {
            get { return genreName; }
            set
            {
                genreName = value;
                NotifyPropertyChanged("GenreName");
            }
        }

        public String Title
        {
            get { return title; }
            set
            {
                title = value;
                NotifyPropertyChanged("Title");
            }
        }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return String.Format(
                "Album : {0}, Artist : {1}, Title : {2}",
                Album, Artist, Title);
        }
        #endregion

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion
    }
}
