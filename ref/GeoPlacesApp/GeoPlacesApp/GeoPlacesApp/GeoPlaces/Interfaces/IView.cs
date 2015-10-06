using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoPlaces
{
    /// <summary>
    /// Used by Views to expose View services
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// Message service
        /// </summary>
        void ShowMessage(string message);
    }
}
