using System;
using System.IO;
using System.Net;
using System.Security;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Configuration;

namespace Sonic
{
	public partial class App: System.Windows.Application
	{
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //read the musicLocations from the App.Config
            try
            {
                Globals.ReReadAllFiles = Sonic.Properties.Settings.Default.ReReadAllFiles;

                MusicLocationLookupConfigSection section = (MusicLocationLookupConfigSection)
                    ConfigurationManager.GetSection("MusicLocationLookup");

                string musicLocation = string.Empty;

                Boolean allValidDirectories = true;
                foreach (MusicLocationElement musicLocationElement in section.MusicLocations)
                {
                    allValidDirectories = Directory.Exists(musicLocationElement.musicPath);
                    Globals.MusicLocations.Add(musicLocationElement.musicPath);
                }
                if (!allValidDirectories)
                {
                    MessageBox.Show("There are invalid paths in the App.Config, please rectify",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Application.Current.Shutdown(-1);
                }
                else
                {
                    //store the current app location
                    Globals.AppLocation = Directory.GetCurrentDirectory();
                }

            }
            catch (ConfigurationErrorsException ceEx)
            {
                MessageBox.Show("There was a problem applying your chosen skin, please retry",
                    "Error",MessageBoxButton.OK,MessageBoxImage.Error);
            }




        }
	}
}
