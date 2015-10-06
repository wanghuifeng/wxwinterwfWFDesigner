using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using GeoPlacesModel;
using System.ServiceModel.Web;

namespace GeoPlaces
{
    /// <summary>
    /// Holds method that call the WCF Service proxy
    /// </summary>
    public static class ServiceCalls
    {
        #region Public Methods
        /// <summary>
        /// Logs a user in, and returns the logged in user
        /// 
        /// Calls WCF Service method
        /// [OperationContract]
        /// [WebInvoke(Method = "POST", UriTemplate = "User/Authenticate/",
        //             ResponseFormat = WebMessageFormat.Xml)]
        /// Users AuthenticateUser(Users userToAuthenticate);
        /// </summary>
        public static Users LoginUser(String username, String password)
        {
            Boolean isAuthenticatedUser = false;


            Users currentUser = new Users
            {
                Name = username,
                Password = password,
                Places = null
            };

            Users dbReadUser = null;

            try
            {
                //Use the GEOPlacesService Proxy
                Service.Use((client) =>
                {
                    //need OperationContextScope to use WebOperationContext(s)
                    //and HttpStatusCode(s)
                    using (new OperationContextScope((IContextChannel)client))
                    {
                        dbReadUser = client.AuthenticateUser(currentUser);

                        IncomingWebResponseContext rctx =
                            WebOperationContext.Current.IncomingResponse;
                        if (rctx.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            if (dbReadUser != null)
                                isAuthenticatedUser = dbReadUser.ID >= 0;
                        }
                    }

                });

                if (isAuthenticatedUser)
                    return dbReadUser;
                else
                {
                    Console.WriteLine("Error logging in user");
                    return null;
                }
            }
            catch (ApplicationException Ex)
            {
                return null;
            }
        }


        /// <summary>
        /// Logs a user in, and returns the logged in user
        /// 
        /// Calls WCF Service method
        /// [OperationContract]
        /// [WebInvoke(Method = "POST", UriTemplate = "User/Add/",
        ///     ResponseFormat = WebMessageFormat.Xml)]
        /// Users AddUser(Users newUser);
        /// </summary>
        public static Users RegisterUser(String username, String password)
        {
            Boolean isAuthenticatedUser = false;


            Users currentUser = new Users
            {
                Name = username,
                Password = password,
                Places = null
            };

            Users dbReadUser = null;

            try
            {
                //Use the GEOPlacesService Proxy
                Service.Use((client) =>
                {
                    //need OperationContextScope to use WebOperationContext(s)
                    //and HttpStatusCode(s)
                    using (new OperationContextScope((IContextChannel)client))
                    {

                        dbReadUser = client.AddUser(currentUser);

                        IncomingWebResponseContext rctx =
                            WebOperationContext.Current.IncomingResponse;
                        if (rctx.StatusCode == System.Net.HttpStatusCode.Created)
                        {
                            if (dbReadUser != null)
                                isAuthenticatedUser = dbReadUser.ID >= 0;
                        }
                    }

                });

                if (isAuthenticatedUser)
                    return dbReadUser;
                else
                {
                    Console.WriteLine("Error registering user");
                    return null;
                }
            }
            catch (ApplicationException Ex)
            {
                return null;
            }
        }


        /// <summary>
        /// Adds a place to an existing user and returns
        /// the newly populated Users object with the new
        /// Place added
        /// 
        /// Calls WCF Service method
        /// [OperationContract]
        /// [WebInvoke(Method = "POST", UriTemplate = "Place/Add/",
        //    ResponseFormat = WebMessageFormat.Xml)]
        /// Users AddPlacesToUser(Users newPlace);
        /// </summary>
        public static Users AddPlacesToUser(
            Users associatedUser, 
            String name, String description,
            Double latitude, Double longitude)
        {

            Boolean completedOk = false;
            Users updatedUser = null;

            Places newPlace = new Places
            {
                Name = name,
                Description = description,
                Latitude = latitude,
                Longitude = longitude,
            };

            associatedUser.Places.Add(newPlace);

            try
            {
                //Use the GEOPlacesService Proxy
                Service.Use((client) =>
                {
                    //need OperationContextScope to use WebOperationContext(s)
                    //and HttpStatusCode(s)
                    using (new OperationContextScope((IContextChannel)client))
                    {
                        updatedUser = client.AddPlacesToUser(associatedUser);

                        IncomingWebResponseContext rctx =
                            WebOperationContext.Current.IncomingResponse;
                        if (rctx.StatusCode == System.Net.HttpStatusCode.Created)
                        {
                            completedOk = updatedUser != null;
                        }
                    }

                });

                if (completedOk)
                    return updatedUser;
                else
                {
                    Console.WriteLine("Error adding place to user");
                    return null;
                }

            }
            catch (ApplicationException Ex)
            {
                return null;
            }
        }



        /// <summary>
        /// Logs a user in, and returns the logged in user
        /// 
        /// Calls WCF Service method
        /// [OperationContract]
        /// [WebGet(UriTemplate = "/placesList/{userId}",
        ///    ResponseFormat = WebMessageFormat.Xml)]
        /// List<Places> GetAllPlacesForUser(String userId);
        /// </summary>
        public static ObservableCollection<Places> GetAllPlacesForUser(String userId)
        {
            Boolean completedOk = false;
            ObservableCollection<Places> places = null;
            List<Places> placesReturned = null;


            try
            {
                //Use the GEOPlacesService Proxy
                Service.Use((client) =>
                {
                    //need OperationContextScope to use WebOperationContext(s)
                    //and HttpStatusCode(s)
                    using (new OperationContextScope((IContextChannel)client))
                    {
                        placesReturned = client.GetAllPlacesForUser(userId.ToString());

                        IncomingWebResponseContext rctx =
                            WebOperationContext.Current.IncomingResponse;
                        if (rctx.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            completedOk = placesReturned != null;
                        }
                    }

                });

                if (completedOk)
                {
                    places = new ObservableCollection<Places>(placesReturned);
                    return places;
                }
                else
                {
                    Console.WriteLine("Error getting all places for user");
                    return null;
                }
            }
            catch (ApplicationException Ex)
            {
                return null;
            }
        }
        #endregion
    }
}
