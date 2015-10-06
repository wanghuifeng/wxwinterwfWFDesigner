using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace GeoPlacesDataService
{

    /// <summary>
    /// Taken directly from the excellent RESTFul .NET by Jon Flanders
    /// 
    /// It simply alters the endpoint behaviour by automatically setting the
    /// Content Type, by the use of the ContentTypeMessageInspector class
    /// </summary>
    public class ContentTypeBehaviour : IEndpointBehavior
    {
        #region Data
        public string ContentType { get; set; }
        #endregion

        #region IEndpointBehavior Members

        public void AddBindingParameters(ServiceEndpoint endpoint, 
            BindingParameterCollection bindingParameters)
        {
            //do nothing
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, 
            ClientRuntime clientRuntime)
        {
            //work out what the correct ContentType should be
            ContentTypeMessageInspector mi = new 
                ContentTypeMessageInspector { ContentType = this.ContentType };
            clientRuntime.MessageInspectors.Add(mi);
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, 
            EndpointDispatcher endpointDispatcher)
        {
            //do nothing
        }

        public void Validate(ServiceEndpoint endpoint)
        {
            //do nothing
        }

        #endregion
    }
}
