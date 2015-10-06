using System;
using System.Configuration;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;
using GeoPlacesDataService;
using Microsoft.ServiceModel.Web;


namespace GeoPlaces
{
    /// <summary>
    /// The client proxy delegate, which is typically an anonomous delegate
    /// in the actual client code
    /// </summary>
    public delegate void UseServiceDelegate(IGeoService proxy);

    /// <summary>
    /// This section of code was originally obtained from the following source
    /// http://www.iserviceoriented.com/blog/post/Indisposable+-+WCF+Gotcha+1.aspx
    /// 
    /// It was subsequently changed in order to make it work with the web based
    /// RestFul WCf service. This class should handle restarting the proxy in case
    /// of a faulted channel
    /// </summary>
    public static class Service
    {
        #region Data

        private static IClientChannel proxy = null;
        public static ChannelFactory<IGeoService> _channelFactory = null;

        #endregion

        #region Ctor
        static Service()
        {
            try
            {
                Uri serviceUri = new Uri(
                    ConfigurationManager.AppSettings["GeoServiceEndpointAddress"]);
                _channelFactory = new WebChannelFactory<IGeoService>(serviceUri);
                _channelFactory.Endpoint.Behaviors.Add(
                    new ContentTypeBehaviour { ContentType = "text/xml" });
            }
            catch (Exception e)
            {
                ApplicationException ae = new ApplicationException(
                    "Error initiating WCF channel for The GeoService", e);
                Console.WriteLine(String.Format("An exception occurred : {0}", ae));
                throw ae;
            }
        }
        #endregion

        #region Public Methods
        public static void Use(UseServiceDelegate codeBlock)
        {
            bool success = false;

            if (proxy != null && (proxy.State == CommunicationState.Opened ||
                                  proxy.State == CommunicationState.Opening))
            {
                //do nothing, all ok
            }
            else
                proxy = (IClientChannel)_channelFactory.CreateChannel();

            //try to create the Proxy
            try
            {
                codeBlock((IGeoService)proxy);
                success = true;
            }


            //WebProtocolException is only avaiable WCF REST Starter Kit Preview 2
            //http://aspnet.codeplex.com/Release/ProjectReleases.aspx?ReleaseId=24644
            catch (WebProtocolException webExp)
            {
                Console.WriteLine(String.Format("An exception occurred : {0}",
                    webExp.Message));

                throw new ApplicationException(
                    "A GeoService WebProtocolException occured", webExp);
            }
            catch (WebException ex)
            {
                using (System.IO.Stream respStream = ex.Response.GetResponseStream())
                    using(System.IO.StreamReader reader = 
                        new System.IO.StreamReader(respStream))
                            Console.WriteLine(String.Format("An exception occurred : {0}",
                            reader.ReadToEnd()));
            }
            catch (FaultException fex)
            {
                Console.WriteLine(String.Format("An exception occurred : {0}",
                    fex.Message));
                throw new ApplicationException(
                    "A GeoService FaultException occured", fex);
            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("An exception occurred : {0}",
                    ex.Message));
                throw new ApplicationException(
                    "A GeoService Exception occured", ex);
            }
            finally
            {
                if (!success && proxy != null)
                    proxy.Abort();
            }
        }
        #endregion
    }

}
