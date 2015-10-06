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
using System.Windows.Threading;

namespace Sonic
{
    /// <summary>
    /// Interaction logic for MediaView.xaml
    /// </summary>
    public partial class MediaView : UserControl
    {
        private Int32 currentTrackNumber = -1;
        private Boolean isPlaying = false;

        public MediaView()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MediaView_Loaded);    
        }

        void MediaView_Loaded(object sender, RoutedEventArgs e)
        {
            //listen for Album clicked
            this.AddHandler(AlbumView.AlbumClickedEvent, 
                new AlbumClickedEventHandler(AlbumClicked));

            //listen for MP3File clicked
            this.AddHandler(MP3FileView.MP3FileClickedEvent,
                new MP3FileClickedEventHandler(MP3FileClicked));

            CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
            


        }


        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            if (me.NaturalDuration.HasTimeSpan)
            {
                double total = me.NaturalDuration.TimeSpan.TotalMilliseconds;
                double pos = me.Position.TotalMilliseconds / total;
                percFill.Width = 100 * pos;
            }
        }


        void AlbumClicked(object sender, AlbumClickedEventArgs e)
        {
            albumView3D.AlbumCoverArtUrl = e.ClickedAlbum.AlbumCoverArtUrl;

            double animationOffset = 100;
            double currentAnimationTime = 0;


            List<MP3FileViewModel> mp3s = new List<MP3FileViewModel>();
            foreach (var file in e.ClickedAlbum.Files)
            {
                MP3FileViewModel viewModel = new MP3FileViewModel();
                viewModel.FileName = file.FileName;
                viewModel.Title = file.Title;
                viewModel.AnimationDelayMs = currentAnimationTime += animationOffset;
                mp3s.Add(viewModel);
            }
            mp3Items.ItemsSource = mp3s;

            spControls.Visibility = Visibility.Visible;

            //clear currentTrackNumber and Stop MediaElement from playing
            currentTrackNumber = -1;
            me.Stop();

        }



        private void MP3FileClicked(object sender, MP3FileClickedEventArgs e)
        {
            SetItemBasedOnPlayingFile(e.FileName);
        }


        private void SetItemBasedOnPlayingFile(String fileName)
        {
            MP3FileViewModel MP3FileViewModel_Selected = null;
            for (int i = 0; i < mp3Items.Items.Count; i++)
            {
                MP3FileViewModel VM = mp3Items.Items[i] as MP3FileViewModel;
                VM.IsSelected = false;
                if (VM.FileName.Equals(fileName))
                {
                    MP3FileViewModel_Selected = VM;
                    currentTrackNumber = i;
                }
            }
            MP3FileViewModel_Selected.IsSelected = true;
            //now play this item with the MediaPlayer element
            me.Source = new Uri(MP3FileViewModel_Selected.FileName, 
                UriKind.RelativeOrAbsolute);
            isPlaying = false;
            DoPlay();

        }



        private void DoPlay()
        {
            if (isPlaying)
            {
                me.Pause();
                isPlaying = false;
                BtnPlayPause.Content = "4";
                BtnPlayPause.ToolTip = "Play";
            }
            else
            {
                if (currentTrackNumber >= 0)
                {
                    me.Play();
                    isPlaying = true;
                    BtnPlayPause.Content = ";";
                    BtnPlayPause.ToolTip = "Pause";
                }
            }

        }


        private void Me_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (currentTrackNumber < mp3Items.Items.Count-1)
            {
                currentTrackNumber++;
                MP3FileViewModel VM = mp3Items.Items[currentTrackNumber] as MP3FileViewModel;
                SetItemBasedOnPlayingFile(VM.FileName);
            }
        }

        private void BtnPlayPause_Click(object sender, RoutedEventArgs e)
        {
            DoPlay();
        }



        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            me.Stop();
        }

    }
}
