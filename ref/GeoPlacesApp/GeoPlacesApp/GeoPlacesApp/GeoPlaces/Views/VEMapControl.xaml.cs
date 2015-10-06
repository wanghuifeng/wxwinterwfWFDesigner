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
using System.Linq;
using GeoPlacesModel;

namespace GeoPlaces
{
    /// <summary>
    /// Virtual Earth control and a List of Places
    /// </summary>
    public partial class VEMapControl : UserControl, IView
    {
        #region Data
        private PlacesViewModel placesViewModel = null;
        private VEMap map = null;
        private VEPushPin newPin = null;
        #endregion

        #region Ctor
        public VEMapControl()
        {
            InitializeComponent();
            SetUpMap();
            this.Loaded += new RoutedEventHandler(VEMapControl_Loaded);
            placesViewModel = new PlacesViewModel(this);
            map.Show3DCursor = true;

            //wire up PlaceClicked
            this.AddHandler(PlaceControlDetailed.PlaceClickedEvent, 
                new PlaceClickedRoutedEventHandler(PlaceClicked));
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Had to resort to setting up Map in code, as intellisense
        /// was being lost (though all was ok at runtime) if map was
        /// set up in XAML
        /// </summary>
        private void SetUpMap()
        {
            //Create the VE Map
            map = new VEMap
              {
                  Width = 590,
                  Height = 590,
                  Margin = new Thickness(5),
                  VerticalAlignment = VerticalAlignment.Top,
                  HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                  MapStyle = VEMapStyle.Hybrid,
                  LatLong = new Point(38.9444195081574, -77.0630161230201),
                  Clip = new EllipseGeometry
                     {
                         RadiusX = 230,
                         RadiusY = 230,
                         Center = new Point(295, 295)
                     }
              };

            //Ceeate a default pin location (my house)
            newPin = new VEPushPin(new VELatLong(50.826958333333337, -0.16388055555555556));
            newPin.SetResourceReference(VEPushPin.StyleProperty, "PushPinStyle");
            newPin.Content = new Label
            {
                Content = "Waiting",
                HorizontalAlignment = HorizontalAlignment.Center,
                FontSize = 20
            };
            newPin.Click += VEPushPin_Click;
            map.Items.Add(newPin);


            //I do not like doing this with indexes that may change, but
            //I had no choice as I wanted map to be exactly 4th child, and 
            //when setting up map in XAML it would sometimes loose intellisense
            mainGrid.Children.Insert(4,map);
        }


        private void VEMapControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = placesViewModel;
        }

        private void btnZoomIn_Click(object sender, RoutedEventArgs e)
        {
            map.DoMapZoom(1000, false);
        }

        private void btnZoomOut_Click(object sender, RoutedEventArgs e)
        {
            map.DoMapZoom(-1000, false);
        }

        private void BtnRoad_Click(object sender, RoutedEventArgs e)
        {
            map.MapStyle = InfoStrat.VE.VEMapStyle.Road;
        }
        private void BtnAerial_Click(object sender, RoutedEventArgs e)
        {
            map.MapStyle = InfoStrat.VE.VEMapStyle.Aerial;
        }
        private void BtnHybrid_Click(object sender, RoutedEventArgs e)
        {
            map.MapStyle = InfoStrat.VE.VEMapStyle.Hybrid;
        }
  
        private void VEPushPin_Click(object sender, VEPushPinClickedEventArgs e)
        {
            VEPushPin pin = sender as VEPushPin;
            map.FlyTo(pin.LatLong, -90, 0, 300, null);
        }

        private void btnPanUp_Click(object sender, RoutedEventArgs e)
        {
            map.DoMapMove(0, 1000, false);
        }

        private void btnPanDown_Click(object sender, RoutedEventArgs e)
        {
            map.DoMapMove(0, -1000, false);
        }

        private void btnPanLeft_Click(object sender, RoutedEventArgs e)
        {
            map.DoMapMove(1000, 0, false);
        }

        private void btnPanRight_Click(object sender, RoutedEventArgs e)
        {
            map.DoMapMove(-1000, 0, false);
        }

        /// <summary>
        /// Remove old pin and add new pin when user selects a place to view
        /// </summary>
        private void PlaceClicked(Object sender, PlaceClickedEventArgs args)
        {
            Places selectedPlace = args.PlaceSelected;
            newPin.Latitude = selectedPlace.Latitude;
            newPin.Longitude = selectedPlace.Longitude;
            newPin.Content = new Label
            {
                Content = selectedPlace.Name,
                HorizontalAlignment = HorizontalAlignment.Center,
                FontSize = 20
            };
            map.FlyTo(new VELatLong(selectedPlace.Latitude, selectedPlace.Longitude), -80, 0, 300, null);
        }
        #endregion

        #region IView Members

        public void ShowMessage(string message)
        {
            MessageBox.Show(message, "information",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #endregion

    }
}
