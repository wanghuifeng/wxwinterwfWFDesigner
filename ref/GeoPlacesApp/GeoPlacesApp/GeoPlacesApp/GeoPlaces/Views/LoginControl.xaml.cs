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

namespace GeoPlaces
{
    /// <summary>
    /// A simple Login control that allows existing
    /// users to login, or new users to register
    /// </summary>
    public partial class LoginControl : UserControl, IView
    {
        #region Data
        private LoginViewModel loginViewModel = null;
        #endregion

        #region Ctor
        public LoginControl()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(LoginControl_Loaded);
            loginViewModel = new LoginViewModel(this);
        }
        #endregion

        #region Private Methods
        private void LoginControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = loginViewModel;
        }
        #endregion

        #region IView Members

        public void ShowMessage(string message)
        {
            MessageBox.Show(message,"information",
                MessageBoxButton.OK,MessageBoxImage.Information);
        }

        #endregion

    }
}
