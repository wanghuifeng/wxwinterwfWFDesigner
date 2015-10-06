using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using GeoPlacesModel;

namespace GeoPlaces
{
    /// <summary>
    /// Simple ViewModel for the LoginControl
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        #region Data
        private String userName = String.Empty;
        private String password = String.Empty;
        private Boolean isAuthenticatedUser = false;
        private Boolean isBusy = false;
        private ICommand loginCommand = null;
        private ICommand registerCommand = null;
        private IView view = null;
        #endregion

        #region Ctor
        public LoginViewModel(IView view)
        {

            this.view = view;

            //wire up loginCommand
            loginCommand = new SimpleCommand
            {
                CanExecuteDelegate = x => !IsBusy,
                ExecuteDelegate = x => Login()
            };

            //wire up registerCommand
            registerCommand = new SimpleCommand
            {
                CanExecuteDelegate = x => !IsBusy,
                ExecuteDelegate = x => Register()
            };
        }
        #endregion

        #region Public Properties
        public ICommand RegisterCommand
        {
            get { return registerCommand; }
        }


        public ICommand LoginCommand
        {
            get { return loginCommand; }
        }

        public Boolean IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                NotifyChanged("IsBusy");
            }
        }


        public String UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                isAuthenticatedUser = false;
                NotifyChanged("IsAuthenticatedUser");
                NotifyChanged("UserName");
            }
        }

        public String Password
        {
            get { return password; }
            set
            {
                password = value;
                isAuthenticatedUser = false;
                NotifyChanged("IsAuthenticatedUser");
                NotifyChanged("Password");
            }
        }

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

        #region Private Methods
        private void Login()
        {
            isBusy = true;

            App.Current.Properties.Remove("CurrentUser");


            Users dbReadUser = ServiceCalls.LoginUser(userName, password);
            if (dbReadUser != null)
            {
                App.Current.Properties.Add("CurrentUser", dbReadUser);
                Mediator.Instance.NotifyColleagues(
                    ViewModelMessages.IsAuthenticatedUser, true);
                view.ShowMessage(String.Format(
                    "Sucessfully logged in user {0}, please proceed to view/add your places",
                        userName));
            }
            else
            {
                App.Current.Properties.Add("CurrentUser", null);
                Mediator.Instance.NotifyColleagues(
                    ViewModelMessages.IsAuthenticatedUser, false);
                view.ShowMessage(String.Format(
                    "Could not log in user {0}, please try again",
                        userName));
            }

            isBusy = false;
        }


        private void Register()
        {
            isBusy = true;

            App.Current.Properties.Remove("CurrentUser");

            Users dbReadUser = ServiceCalls.RegisterUser(userName, password);
            if (dbReadUser != null)
            {
                App.Current.Properties.Add("CurrentUser", dbReadUser);
                Mediator.Instance.NotifyColleagues(
                    ViewModelMessages.IsAuthenticatedUser, true);
                view.ShowMessage(String.Format(
                    "Sucessfully added user {0}, please proceed to view/add your places",
                        userName));

            }
            else
            {
                App.Current.Properties.Add("CurrentUser", null);
                Mediator.Instance.NotifyColleagues(
                    ViewModelMessages.IsAuthenticatedUser, false);
                view.ShowMessage(String.Format(
                    "Could not add user {0}, please try again",
                        userName));
            }

            isBusy = false;
        }
        #endregion
    }
}
