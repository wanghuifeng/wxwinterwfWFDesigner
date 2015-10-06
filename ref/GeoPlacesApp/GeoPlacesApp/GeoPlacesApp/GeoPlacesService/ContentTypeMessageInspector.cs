using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.ServiceModel;

namespace GeoPlacesDataService
{

    /// <summary>
    /// Taken directly from the excellent RESTFul .NET by Jon Flanders
    /// 
    /// Automatically sets the Content Type, by extending WCF namely 
    /// setting the HttpRequestMessageProperty headers
    /// </summary>
    public class ContentTypeMessageInspector : IClientMessageInspector
    {
        #region Data
        public string ContentType { get; set; }
        #endregion

        #region IClientMessageInspector Members

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            //do nothing
        }

        /// <summary>
        /// Apply the Content-Type header to the HttpRequestMessageProperty
        /// </summary>
        public object BeforeSendRequest(ref Message request, 
            IClientChannel channel)
        {
            HttpRequestMessageProperty prop = 
                request.Properties[HttpRequestMessageProperty.Name]
                    as HttpRequestMessageProperty;
            
            if (prop != null && (prop.Method == "POST" || prop.Method == "PUT"))
            {
                prop.Headers["Content-Type"] = this.ContentType;
            }
            return null;

        }

        #endregion
    }
}
