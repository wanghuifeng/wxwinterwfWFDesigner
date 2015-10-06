using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace GeoPlaces
{
    /// <summary>
    /// ViewModel base class
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
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
            if(VerifyPropertyname(PropName))
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
