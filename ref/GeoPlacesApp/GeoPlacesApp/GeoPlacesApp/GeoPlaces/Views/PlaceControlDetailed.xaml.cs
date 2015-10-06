using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using InfoStrat.VE;
using GeoPlacesModel;

namespace GeoPlaces
{
    /// <summary>
    /// Delegate use for Place Selected event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void PlaceClickedRoutedEventHandler(Object sender, PlaceClickedEventArgs args);

    /// <summary>
    /// IDetailed Place Item Control
    /// </summary>
    public partial class PlaceControlDetailed : UserControl
    {

        #region Events

        #region PlaceClicked

        /// <summary>
        /// PlaceClicked Routed Event
        /// </summary>
        public static readonly RoutedEvent PlaceClickedEvent =
            EventManager.RegisterRoutedEvent("PlaceClicked",
                RoutingStrategy.Bubble, typeof(PlaceClickedRoutedEventHandler),
                    typeof(PlaceControlDetailed));

        /// <summary>
        /// Bubbled event that occurs when the user clicks a Place
        /// </summary>
        public event PlaceClickedRoutedEventHandler PlaceClicked
        {
            add { AddHandler(PlaceClickedEvent, value); }
            remove { RemoveHandler(PlaceClickedEvent, value); }
        }
        #endregion

        #endregion

        #region Ctor
        public PlaceControlDetailed()
        {
            InitializeComponent();
        }
        #endregion

        #region Private Methods
        private void PlaceButton_Click(object sender, RoutedEventArgs e)
        {
            PlaceClickedEventArgs args = 
                new PlaceClickedEventArgs(PlaceControlDetailed.PlaceClickedEvent, 
                    this.DataContext as Places);
            RaiseEvent(args);
        }
        #endregion
    }



    #region PlaceClickedEventArgs
    /// <summary>
    /// Event args
    /// </summary>
    public class PlaceClickedEventArgs : RoutedEventArgs
    {
        public Places PlaceSelected { get; private set; }

        /// <summary>
        /// ctor
        /// </summary>
        public PlaceClickedEventArgs (RoutedEvent routedEvent,
            Places placeSelected) : base(routedEvent)
	    {
            this.PlaceSelected = placeSelected;
	    }
    }
    #endregion

 
}
