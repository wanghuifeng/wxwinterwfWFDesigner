using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Input;




namespace Sonic
{



	public partial class MainWindow
	{
        private MainWindowViewModel mainWindowViewModel = new MainWindowViewModel();
        private DragAndDropHelper dragAndDropHelper = new DragAndDropHelper();

        public MainWindow()
		{
			this.InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        #region Private Methods

        /// <summary>
        /// On load set DataContext to MainWindowViewModel and 
        /// initalise it, which basically means create the XML
        /// Library file for the MP3 directories that match
        /// the locations stored in the App.Config
        /// </summary>
        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = mainWindowViewModel;
            mainWindowViewModel.Initialise();
        }
        #endregion

        #region Window Control Buttons

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnMax_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            //Settings are stored at C:\Users\sacha\AppData\Local\Sonic\Sonic.vshost.exe_Url_hgefzieuxosag5swhmgx4xzo5rtoslkn\0.0.0.0
            //TODO : Need to uncomment this when happy
            Sonic.Properties.Settings.Default.ReReadAllFiles = false;
            Sonic.Properties.Settings.Default.Save();
            Application.Current.Shutdown();
        }

        private void btnSize_Click(object sender, RoutedEventArgs e)
        {
            this.Height = this.MinHeight;
            this.Width = this.MinWidth;
            this.WindowState = WindowState.Normal;
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }


        private void StackPanel_DragOver(object sender, DragEventArgs e)
        {
            if (mainWindowViewModel.IsNotBusy)
                dragAndDropHelper.DragOver(e);
        }

        private void StackPanel_Drop(object sender, DragEventArgs e)
        {
            if (mainWindowViewModel.IsNotBusy)
                dragAndDropHelper.Drop(e);
        }

        #endregion

	}
}