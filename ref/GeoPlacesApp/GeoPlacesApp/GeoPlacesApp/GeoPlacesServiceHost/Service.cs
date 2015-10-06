using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.ServiceProcess;
using System.Text;
using GeoPlacesDataService;

namespace GeoPlacesServiceHost
{
    /// <summary>
    /// Windows service to host the actual Restful 
    /// WCF GEOService
    /// </summary>
    public partial class Service : ServiceBase
    {
        #region Data
        private static ServiceHost GeoPlacesServiceHost;
        #endregion

        #region Ctor
        public Service()
        {
            InitializeComponent();
        }
        #endregion

        #region Overrides
        protected override void OnStart(string[] args)
        {
            Service.StartService();
        }

        protected override void OnStop()
        {
            try
            {
                Service.StopServiceHost(GeoPlacesServiceHost);
            }
            catch (Exception ex)
            {

                Console.WriteLine(String.Format(
                    "Exception while attempting to stop GeoService " +
                    "service type {0} the following exception was thrown {1}.",
                    this.GetType().FullName, ex.ToString()));
            }
        }
        #endregion

        #region Public Methods
        public static void StartService()
        {
            try
            {
                GeoPlacesServiceHost = new WebServiceHost(typeof(GeoService),
                    new Uri(ConfigurationManager.AppSettings[
                        "GeoServiceEndpointAddress"]));

                StartServiceHost(GeoPlacesServiceHost);
            }
            catch (TargetInvocationException tiEx)
            {
                Console.WriteLine(String.Format("Exception occurred", tiEx.Message));
            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("Exception occurred", ex.Message));
            }
        }
        #endregion

        #region Private Methods
        private static void StartServiceHost(ServiceHost serviceHost)
        {

            Boolean openSucceeded = false;

            try
            {

                serviceHost = new WebServiceHost(typeof(GeoService),
                       new Uri(ConfigurationManager.AppSettings[
                           "GeoServiceEndpointAddress"]));

                serviceHost.Open();
                openSucceeded = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format(
                    "A failure occurred trying to open the " +
                    "GeoService ServiceHost, Error message : {0}",
                        ex.Message));
            }
            finally
            {
                if (!openSucceeded)
                {
                    serviceHost.Abort();
                    Console.WriteLine(String.Format("{0} Aborted.",
                        serviceHost.Description.Name));
                }
            }

            if (serviceHost.State == CommunicationState.Opened)
            {
                serviceHost.Faulted += ServiceHost_Faulted;
                Console.WriteLine("GeoService is running...");
            }
            else
            {
                Console.WriteLine("GeoService failed to open");
                Boolean closeSucceeded = false;
                try
                {
                    serviceHost.Close();
                    closeSucceeded = true;
                    Console.WriteLine(String.Format("{0} Closed.",
                        serviceHost.Description.Name));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(String.Format(
                        "A failure occurred trying to close the " +
                        "GeoServicee ServiceHost, Error message : {0}",
                            ex.Message));
                }
                finally
                {
                    if (!closeSucceeded)
                    {
                        serviceHost.Abort();
                        Console.WriteLine(String.Format("{0} Aborted.",
                            serviceHost.Description.Name));
                    }
                }
            }
        }

        private static void StopServiceHost(ServiceHostBase serviceHost)
        {
            if (serviceHost.State != CommunicationState.Closed)
            {
                Boolean closeSucceeded = false;
                try
                {
                    serviceHost.Close();
                    closeSucceeded = true;
                    Console.WriteLine(String.Format("{0} Closed.",
                        serviceHost.Description.Name));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(String.Format(
                        "A failure occurred trying to close the " +
                        "GeoService ServiceHost, Error message : {0}",
                            ex.Message));
                }
                finally
                {
                    if (!closeSucceeded)
                    {
                        serviceHost.Abort();
                        Console.WriteLine(String.Format("{0} Aborted.",
                            serviceHost.Description.Name));
                    }
                }
            }
        }

        private static void RestartServiceHost(ServiceHost serviceHost)
        {
            StopServiceHost(serviceHost);
            StartServiceHost(serviceHost);
        }

        private static void LogServiceHostInfo(ServiceHostBase serviceHost)
        {
            var strBuilder = new StringBuilder();
            strBuilder.AppendFormat("'{0}' Starting", serviceHost.Description.Name);
            strBuilder.Append(Environment.NewLine);

            // Behaviors
            var annotation = 
                serviceHost.Description.Behaviors.Find<ServiceBehaviorAttribute>();

            strBuilder.AppendFormat("Concurrency Mode = {0}", 
                annotation.ConcurrencyMode);
            strBuilder.Append(Environment.NewLine);
            strBuilder.AppendFormat("InstanceContext Mode = {0}", 
                annotation.InstanceContextMode);
            strBuilder.Append(Environment.NewLine);

            // Endpoints
            strBuilder.Append("The following endpoints are exposed:");
            strBuilder.Append(Environment.NewLine);
            foreach (ServiceEndpoint endPoint in serviceHost.Description.Endpoints)
            {
                strBuilder.AppendFormat("{0} at {1} with {2} binding; "
                       , endPoint.Contract.ContractType.Name
                       , endPoint.Address
                       , endPoint.Binding.Name);
                strBuilder.Append(Environment.NewLine);
            }

            // Metadata
            var metabehaviour = 
                serviceHost.Description.Behaviors.Find<ServiceMetadataBehavior>();

            if (metabehaviour != null)
            {
                if (metabehaviour.HttpGetEnabled)
                {
                    if (metabehaviour.HttpsGetUrl != null)
                    {
                        strBuilder.AppendFormat("Metadata enabled at {0}", 
                            serviceHost.BaseAddresses[0]);
                    }
                    else
                    {
                        strBuilder.AppendFormat("Metadata enabled at {0}", 
                            metabehaviour.HttpGetUrl);
                    }
                }
                if (metabehaviour.HttpsGetEnabled)
                    strBuilder.AppendFormat(" and {0}.", metabehaviour.HttpsGetUrl);
                if (metabehaviour.ExternalMetadataLocation != null)
                    strBuilder.AppendFormat(" Metadata can be found externally at {0}", 
                        metabehaviour.ExternalMetadataLocation);
            }

            Console.WriteLine(strBuilder.ToString());
        }

        private static void ServiceHost_Faulted(Object sender, EventArgs e)
        {
            var serviceHost = sender as ServiceHost;
            Console.Write(String.Format("{0} Faulted.  Attempting Restart.",
                serviceHost.Description.Name));
            RestartServiceHost(serviceHost);
        }
        #endregion
    }
}
