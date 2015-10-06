using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Configuration;
using System.Net;
using System.Net.NetworkInformation;

namespace SnookerService
{
    #region SnookerService
    /// <summary>
    /// This simple console application just hosts the WCF service, so that the clients
    /// can connect in a multi player environment.
    /// Please modify the endpoint's address in app.config to conform to 
    /// This class creates the initial <see cref="SnookerService">SnookerService </see>which
    /// is used by all the connected snooker clients. The app.config is used to creat the
    /// service bindings
    /// </summary>
    class Program
    {
        static Uri uri = null;

        /// <summary>
        /// Main access point, creates the initial <see cref="SnookerService">SnookerService </see>which
        /// is used by all the connected snooker clients. The app.config is used to creat the
        /// service bindings
        /// </summary>
        /// <param name="args">The command line args</param>
        static void Main(string[] args)
        {
            // Get host name
            String strHostName = Dns.GetHostName();

            // Find host by name
            IPHostEntry iphostentry = Dns.GetHostEntry(strHostName);

            // Enumerate IP addresses
            int nIP = 0;
            foreach (IPAddress ipaddress in iphostentry.AddressList)
            {
                Console.WriteLine("Server IP: #{0}: {1}", ++nIP, ipaddress);
            }

            //Concatenates the configuration address with the ip obtained from this server
            uri = new Uri(string.Format(ConfigurationManager.AppSettings["address"], strHostName));

            ServiceHost host = new ServiceHost(typeof(SnookerService), uri);
            host.Opened += new EventHandler(host_Opened);
            host.Closed += new EventHandler(host_Closed);
            host.Open();
            Console.ReadLine();
            host.Abort();
            host.Close();
        }

        static void host_Opened(object sender, EventArgs e)
        {
            Console.WriteLine("Snooker service is open and listening on endpoint {0}", uri.ToString());
        }

        static void host_Closed(object sender, EventArgs e)
        {
            Console.WriteLine("Snooker service has just been closed");
        }
    }
    #endregion
}
