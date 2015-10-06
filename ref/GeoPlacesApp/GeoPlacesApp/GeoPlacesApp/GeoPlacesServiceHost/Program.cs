using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.ServiceProcess;

namespace GeoPlacesServiceHost
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// Runs as console app if in debug
        /// </summary>
        static void Main()
        {
#if (!DEBUG)
            try
            {
                Console.WriteLine(String.Format(
                      "Initialising GeoPlacesDataService.GeoService in assembly " +
                      "{0} RELEASE windows service mode.",
                      Assembly.GetExecutingAssembly().FullName));

                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] { new Service() };
                ServiceBase.Run(ServicesToRun);
            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("Exception Occurred :", ex.Message));
            }
#else
            try
            {
                Console.WriteLine(String.Format(
                      "Initialising GeoPlacesDataService.GeoService in assembly {0} DEBUG console mode.",
                      Assembly.GetExecutingAssembly().FullName));

                Console.WriteLine("Starting GeoPlacesDataService.GeoService");
                Service.StartService();
                Console.WriteLine("GeoPlacesDataService.GeoService Started");

                System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("Exception Occurred :", ex.Message));
            }
#endif
        }
    }
}
