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

namespace GeoPlaces
{
    /// <summary>
    /// Add new places control
    /// </summary>
    public partial class AddViewPlacesControl : UserControl, IView
    {
        #region Data
        private PlacesViewModel placesViewModel = null;
        #endregion

        #region Data
        public AddViewPlacesControl()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(AddViewPlacesControl_Loaded);
            placesViewModel = new PlacesViewModel(this);
        }
        #endregion

        #region Data
        private void AddViewPlacesControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = placesViewModel;
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
