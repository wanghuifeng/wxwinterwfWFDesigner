using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Threading;
using System.Windows.Threading;

namespace Sonic
{
    /// <summary>
    /// Interaction logic for AlbumView3D.xaml
    /// </summary>
    public partial class AlbumView3D : UserControl
    {
        private DispatcherTimer imageLoadingTimer = new DispatcherTimer();

        public AlbumView3D()
        {
            InitializeComponent();
            imageLoadingTimer.Interval = TimeSpan.FromSeconds(4);
            imageLoadingTimer.Tick += ImageLoadingTimer_Tick;
        }

        void ImageLoadingTimer_Tick(object sender, EventArgs e)
        {
            imageLoadingTimer.IsEnabled = false;
            imageLoadingTimer.Stop();
            //animate 3d stuff
            vp3D.Visibility = Visibility.Visible;
            borderOffScreen.Visibility = Visibility.Visible;
            Storyboard sb = this.Resources["OnLoaded1"] as Storyboard;
            sb.Begin(vp3D);
        }

        public String AlbumCoverArtUrl
        {
            set
            {
                BitmapImage bmp = new BitmapImage(new Uri(value,
                    UriKind.RelativeOrAbsolute));
                imgOffScreen.BeginInit();
                imgOffScreen.Source = bmp;
                imgOffScreen.EndInit();
                vp3D.Visibility = Visibility.Hidden;
                imageLoadingTimer.IsEnabled = true;
                imageLoadingTimer.Start();
            }
        }






    }
}
