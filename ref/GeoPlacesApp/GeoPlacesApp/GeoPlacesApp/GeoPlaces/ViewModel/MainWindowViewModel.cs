using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace GeoPlaces
{
    /// <summary>
    /// Simple ViewModel for the MainWindow
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        #region Data
        private IView view = null;
        private Boolean isAuthenticatedUser = false;
        #endregion

        #region Ctor
        public MainWindowViewModel(IView view)
        {
            this.view = view;

            //register to the mediator for the IsAuthenticatedUser message
            Mediator.Instance.Register(
                (Object o) =>
                    {
                        IsAuthenticatedUser = (Boolean) o;
                    }, ViewModelMessages.IsAuthenticatedUser);
        }
        #endregion

        #region Public Properties
        public Boolean IsAuthenticatedUser
        {
            get { return isAuthenticatedUser; }
            set
            {
                isAuthenticatedUser = value;
                NotifyChanged("IsAuthenticatedUser");
            }
        }
        #endregion
    }
}
