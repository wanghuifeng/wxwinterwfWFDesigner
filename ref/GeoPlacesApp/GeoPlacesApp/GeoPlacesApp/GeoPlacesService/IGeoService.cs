using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.ServiceModel.Web;

using GeoPlacesModel;

namespace GeoPlacesDataService
{
    [ServiceContract(SessionMode = SessionMode.NotAllowed)]
    public interface IGeoService
    {

        #region GETs
        [OperationContract]
        [WebGet(UriTemplate = "/users/{userId}",
            ResponseFormat = WebMessageFormat.Xml)]
        Users GetUser(String userId);

        [OperationContract]
        [WebGet(UriTemplate = "/places/{placeId}",
            ResponseFormat = WebMessageFormat.Xml)]
        Places GetPlace(String placeId);

        [OperationContract]
        [WebGet(UriTemplate = "/placesList/{userId}",
        ResponseFormat = WebMessageFormat.Xml)]
        List<Places> GetAllPlacesForUser(String userId);
        #endregion

        #region POSTs
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "User/Add/",
            ResponseFormat = WebMessageFormat.Xml)]
        Users AddUser(Users newUser);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "User/Authenticate/",
            ResponseFormat = WebMessageFormat.Xml)]
        Users AuthenticateUser(Users userToAuthenticate);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "Place/Add/",
            ResponseFormat = WebMessageFormat.Xml)]
        Users AddPlacesToUser(Users newPlace);
        #endregion
    }
}
