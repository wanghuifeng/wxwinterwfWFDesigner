using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;
using GeoPlacesModel;
using System.ComponentModel;

namespace GeoPlaces
{
    /// <summary>
    /// Application class, which also holds the globally
    /// available application list of Places
    /// </summary>
    public partial class App : Application, INotifyPropertyChanged
    {
        #region Data
        private ObservableCollection<Places> allPlaces = new ObservableCollection<Places>();
        #endregion

        #region Public Properties
        public ObservableCollection<Places> AllPlaces
        {
            get { return allPlaces; }
            set
            {
                allPlaces = value;
                NotifyChanged("AllPlaces");
            }
        }
        #endregion

        #region INotifyPropertyChanged Implementation
        /// <summary>
        /// Occurs when any properties are changed on this object.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raise the changes event
        /// </summary>
        /// <param name="PropName"></param>
        public void NotifyChanged(string PropName)
        {
            if (VerifyPropertyname(PropName))
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(PropName));
        }

        /// <summary>
        /// Verify the property
        /// </summary>
        public Boolean VerifyPropertyname(string propname)
        {
            return TypeDescriptor.GetProperties(this)[propname] != null;
        }

        #endregion

    }
}
