using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using GeoPlacesModel;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Collections.ObjectModel;

namespace GeoPlaces
{
    /// <summary>
    /// Simple ViewModel for the AddViewPlacesControl
    /// </summary>
    public class PlacesViewModel : ViewModelBase
    {
        #region Data
        private String name = String.Empty;
        private String description = String.Empty;
        private Double latitude = 0.0;
        private Double longitude = 0.0;
        private Boolean isBusy = false;
        private Boolean isAuthenticatedUser = false;
        //private ObservableCollection<Places> allPlaces = new ObservableCollection<Places>();
        private ICommand saveNewPlaceCommand = null;
        private IView view = null;

        #endregion

        #region Ctor
        public PlacesViewModel(IView view)
        {
            this.view = view;

            //wire up loginCommand
            saveNewPlaceCommand = new SimpleCommand
            {
                CanExecuteDelegate = x => !IsBusy,
                ExecuteDelegate = x => SaveNewPlace()
            };

            //register to the mediator for the IsAuthenticatedUser message
            Mediator.Instance.Register(
                (Object o) =>
                {
                    isAuthenticatedUser = (Boolean)o;
                    if (isAuthenticatedUser)
                        GetAllPlaces();
                }, ViewModelMessages.IsAuthenticatedUser);

        }
        #endregion

        #region Public Properties
        public ICommand SaveNewPlaceCommand
        {
            get { return saveNewPlaceCommand; }
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

        public String Name
        {
            get { return name; }
            set
            {
                name = value;
                NotifyChanged("Name");
            }
        }

        public String Description
        {
            get { return description; }
            set
            {
                description = value;
                NotifyChanged("Description");
            }
        }

        public Double Latitude
        {
            get { return latitude; }
            set
            {
                latitude = value;
                NotifyChanged("Latitude");
            }
        }

        public Double Longitude
        {
            get { return longitude; }
            set
            {
                longitude = value;
                NotifyChanged("Longitude");
            }
        }


        //public ObservableCollection<Places> AllPlaces
        //{
        //    get { return allPlaces; }
        //    set
        //    {
        //        allPlaces = value;
        //        NotifyChanged("AllPlaces");
        //    }
        //}
        #endregion

        #region Private Methods

        /// <summary>
        /// Saves a new place against a user
        /// </summary>
        private void SaveNewPlace()
        {
            isBusy = true;

            //NOTE :This is not the way to do validation you should
            //really use IDataErrorInfo or ValidationRules, but
            //for this demos purposes this was enough I felt
            if (this.Name == string.Empty ||
                Description == string.Empty ||
                Latitude == 0 ||
                Longitude == 0)
            {
                view.ShowMessage("You must fill all the Place fields\r\nPlease rectify");
                isBusy = false;
                return;
            }


            if (App.Current.Properties["CurrentUser"] == null)
                return;
            if (!(App.Current.Properties["CurrentUser"] is Users))
                return;

            Users currentUser = App.Current.Properties["CurrentUser"] as Users;
            App.Current.Properties.Remove("CurrentUser");

            Users dbReadUser = ServiceCalls.AddPlacesToUser(
                currentUser, this.Name, this.Description, this.Latitude, this.Longitude);

            if (dbReadUser != null)
            {
                App.Current.Properties.Add("CurrentUser", dbReadUser);
                GetAllPlaces();
                view.ShowMessage(String.Format(
                  "Sucessfully added new place for user {0}",
                  currentUser.Name));
            }
            else
            {
                App.Current.Properties.Add("CurrentUser", null);
                view.ShowMessage(String.Format(
                  "Could not add places to user {0}, please try again",
                  currentUser.Name));
            }

            isBusy = false;
        }


        /// <summary>
        /// Gets all places for a user
        /// </summary>
        private void GetAllPlaces()
        {
            isBusy = true;

            if (App.Current.Properties["CurrentUser"] == null)
                return;
            if (!(App.Current.Properties["CurrentUser"] is Users))
                return;

            Int32 userId = (App.Current.Properties["CurrentUser"] as Users).ID;

            (App.Current as App).AllPlaces = ServiceCalls.GetAllPlacesForUser(userId.ToString());
            isBusy = false;
        }
        #endregion

    }
}
