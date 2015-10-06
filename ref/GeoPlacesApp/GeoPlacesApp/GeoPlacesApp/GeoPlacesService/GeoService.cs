#define HTTP
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;
using Microsoft.ServiceModel.Web;

using GeoPlacesModel;
using System.Configuration;
using System.Security.Cryptography;
using System.ServiceModel.Web;



namespace GeoPlacesDataService
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = false, 
        InstanceContextMode = InstanceContextMode.Single, 
        ConcurrencyMode = ConcurrencyMode.Single)]
    public class GeoService : IGeoService
    {
        #region IGeoService Members


        /// <summary>
        /// Gets a user based on its Id
        /// </summary>
        public Users GetUser(String userId)
        {

            Int32 id = -1;
            if (Int32.TryParse(userId, out id))
            {
                try
                {
                    Users u = FindUser(id);
#if HTTP
                    string etag = GenerateETag(u.ID + u.Name + u.Password);

                    if (CheckETag(etag))
                        return null;

                    if (u == null)
                    {
                        OutgoingWebResponseContext ctx =
                            WebOperationContext.Current.OutgoingResponse;
                        ctx.SetStatusAsNotFound();
                        ctx.SuppressEntityBody = true;
                    }

                    SetETag(etag);
#endif
                    return u;

                }
                catch (Exception ex)
                {
                    //WebProtocolException is part of WCF REST Starter Kit Preview 2
                    throw new WebProtocolException(HttpStatusCode.BadRequest,
                        String.Format("Couldn't find user with id {0}", userId), null);
                }
            }
            else
                return null;
        }


        /// <summary>
        /// Gets a place based on its Id
        /// </summary>
        public Places GetPlace(String placeId)
        {

            Int32 id = -1;
            if (Int32.TryParse(placeId, out id))
            {

                try
                {
                    Places pl = FindPlace(id);
#if HTTP
                    string etag = GenerateETag(pl.ID.ToString() +
                        pl.Latitude.ToString() + pl.Longitude.ToString() +
                        pl.Name.ToString());

                    if (CheckETag(etag))
                        return null;

                    if (pl == null)
                    {
                        OutgoingWebResponseContext ctx =
                            WebOperationContext.Current.OutgoingResponse;
                        ctx.SetStatusAsNotFound();
                        ctx.SuppressEntityBody = true;
                    }

                    SetETag(etag);
#endif
                    return pl;

                }
                catch (Exception ex)
                {
                    //WebProtocolException is part of WCF REST Starter Kit Preview 2
                    throw new WebProtocolException(HttpStatusCode.BadRequest,
                        String.Format("Couldn't find place with id {0}", placeId), null);
                }
            }
            else
                return null;
        }



        /// <summary>
        /// Gets all places for a particular user
        /// </summary>
        public List<Places> GetAllPlacesForUser(String userId)
        {
            Int32 id = -1;
            if (Int32.TryParse(userId, out id))
            {
                try
                {
                    GeoPlacesEntities model = new GeoPlacesEntities();
                    return model.Places.Where((pl) => pl.Users.ID == id).ToList();
                }
                catch (Exception ex)
                {
                    //WebProtocolException is part of WCF REST Starter Kit Preview 2
                    throw new WebProtocolException(HttpStatusCode.BadRequest,
                        String.Format("Couldn't find places for user id {0}", userId), null);

                }
            }
            else
                return null;
        }



        /// <summary>
        /// Adds a user to the System
        /// </summary>
        public Users AddUser(Users newUser)
        {
            try
            {
                GeoPlacesEntities model = new GeoPlacesEntities();
                model.AddToUsers(newUser);
                model.SaveChanges();

                Users userFromDb = model.Users.Where((u) =>
                   u.Name.ToLower().Equals(newUser.Name) &&
                   u.Password.ToLower().Equals(newUser.Password)
                   ).First();

#if HTTP
                OutgoingWebResponseContext ctx = 
                    WebOperationContext.Current.OutgoingResponse;
                ctx.SetStatusAsCreated(CreateNewUserUri(userFromDb));
#endif

                return userFromDb;
            }
            catch(Exception ex)
            {
                //WebProtocolException is part of WCF REST Starter Kit Preview 2
                throw new WebProtocolException(HttpStatusCode.BadRequest,
                    "Couldn't add new user", null);
            }
        }



        /// <summary>
        /// Authenticates an existing user
        /// </summary>
        public Users AuthenticateUser(Users userToAuthenticate)
        {
            try
            {
                GeoPlacesEntities model = new GeoPlacesEntities();
                Users userFromDb = model.Users.Where((u) =>
                   u.Name.ToLower().Equals(userToAuthenticate.Name) &&
                   u.Password.ToLower().Equals(userToAuthenticate.Password)
                   ).First();

                return userFromDb;
            }
            catch (Exception ex)
            {
                //WebProtocolException is part of WCF REST Starter Kit Preview 2
                throw new WebProtocolException(HttpStatusCode.BadRequest,
                    "Couldn't add new user", null);
            }
        }


        /// <summary>
        /// Adds a place to a user
        /// </summary>
        public Users AddPlacesToUser(Users newUserWithPlaces)
        {
            try
            {
                GeoPlacesEntities model = new GeoPlacesEntities();
                var userFromDB  = model.Users.Where(
                   (u) =>
                   u.ID == newUserWithPlaces.ID).First();

                Places addedPlace = newUserWithPlaces.Places.First();


                userFromDB.Places.Add(new Places
                {
                    Name = addedPlace.Name,
                    Description = addedPlace.Description,
                    Latitude = addedPlace.Latitude,
                    Longitude = addedPlace.Longitude
                });

                model.SaveChanges();


#if HTTP
                OutgoingWebResponseContext ctx =
                    WebOperationContext.Current.OutgoingResponse;
                ctx.SetStatusAsCreated(CreateNewPlacesUri(addedPlace));
#endif

                return newUserWithPlaces;
            }
            catch (Exception ex)
            {
                //WebProtocolException is part of WCF REST Starter Kit Preview 2
                throw new WebProtocolException(HttpStatusCode.BadRequest,
                    "Couldn't add users with places", null);
            }
        }
  


        #endregion

        #region Private Methods

        /// <summary>
        /// Finds a user based on its id
        /// </summary>
        private Users FindUser(Int32 userId)
        {
            try
            {
                GeoPlacesEntities model = new GeoPlacesEntities();
                return model.Users.Where(
                   (u) =>
                   u.ID == userId).First();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Finds a place based on its id
        /// </summary>
        private Places FindPlace(Int32 placeId)
        {
            try
            {
                GeoPlacesEntities model = new GeoPlacesEntities();
                return model.Places.Where(
                   (pl) =>
                   pl.ID == placeId).First();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Sets a ETag (caching for the object) on the current
        /// OutgoingResponse context
        /// </summary>
        private void SetETag(string etag)
        {
            OutgoingWebResponseContext ctx =
                WebOperationContext.Current.OutgoingResponse;
            ctx.ETag = etag;
        }


        /// <summary>
        /// Creates  a ETag (caching for the object)
        /// </summary>
        private  string GenerateETag(String valueToHash)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(valueToHash);
            byte[] hash = MD5.Create().ComputeHash(bytes);
            string etag = Convert.ToBase64String(hash);
            return etag;
        }


        /// <summary>
        /// Examines the incoming request context to see if there
        /// is a cached ETag for the object requested
        /// </summary>
        private bool CheckETag(string currentETag)
        {
            IncomingWebRequestContext ctx =
                WebOperationContext.Current.IncomingRequest;
            string incomingEtag =
                ctx.Headers[HttpRequestHeader.IfNoneMatch];
            if (incomingEtag != null)
            {
                if (currentETag == incomingEtag)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Create a new User Uri for the Resource
        /// </summary>
        private Uri CreateNewUserUri(Users u)
        {
            UriTemplate ut = new UriTemplate("/users/{user_id}");
            Uri baseUri = WebOperationContext.Current.
                IncomingRequest.UriTemplateMatch.BaseUri;
            Uri ret = ut.BindByPosition(baseUri, u.ID.ToString());
            return ret;
        }


        /// <summary>
        /// Create a new Places Uri for the Resource
        /// </summary>
        private Uri CreateNewPlacesUri(Places pl)
        {
            UriTemplate ut = new UriTemplate("/places/{place_id}");
            Uri baseUri = WebOperationContext.Current.
                IncomingRequest.UriTemplateMatch.BaseUri;
            Uri ret = ut.BindByPosition(baseUri, pl.ID.ToString());
            return ret;
        }


        #endregion
    }
}
