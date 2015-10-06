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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Sonic
{

    public delegate void MP3FileClickedEventHandler(object sender, MP3FileClickedEventArgs e);


    /// <summary>
    /// Interaction logic for MP3FileView.xaml
    /// </summary>
    public partial class MP3FileView : UserControl
    {
        public MP3FileView()
        {
            InitializeComponent();

            this.DataContextChanged += MP3FileView_DataContextChanged;

        }

        #region Events
        /// <summary>
        /// Raised when Album item clicked
        /// </summary>
        public static readonly RoutedEvent MP3FileClickedEvent =
                EventManager.RegisterRoutedEvent(
                "MP3FileClicked", RoutingStrategy.Bubble,
                typeof(MP3FileClickedEventHandler),
                typeof(MP3FileView));




        public event MP3FileClickedEventHandler MP3FileClicked
        {
            add { AddHandler(MP3FileClickedEvent, value); }
            remove { RemoveHandler(MP3FileClickedEvent, value); }
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Hook up the associated MP3FileViewModel
        /// AnimationStartTimerExpiredEvent event
        /// </summary>
        private void MP3FileView_DataContextChanged(object sender, 
            DependencyPropertyChangedEventArgs e)
        {
            MP3FileViewModel viewModel = e.NewValue as MP3FileViewModel;
            if (viewModel != null)
            {
                viewModel.StartDelayedAnimationTimer();
                viewModel.AnimationStartTimerExpiredEvent += 
                    ViewModel_AnimationStartTimerExpiredEvent;
            }
        }

        /// <summary>
        /// Start the animation Storyboard after the associated AlbumOfMP3ViewModel
        /// timer expires and raises its AnimationStartTimerExpiredEvent event
        /// </summary>
        private void ViewModel_AnimationStartTimerExpiredEvent(object sender, EventArgs e)
        {
            //As the call that populated this control was on a different thread, 
            //we need to do some threading trickery
            this.Dispatcher.InvokeIfRequired(() =>
            {
                Storyboard sb = this.TryFindResource("OnLoaded1") as Storyboard;
                if (sb != null)
                {
                    sb.Begin(this.spMP3s);
                }
            }, DispatcherPriority.Normal);
        }

        /// <summary>
        /// When an item is clicked raise the event so that the MediaView can
        /// play the selected file
        /// </summary>
        private void SpMP3s_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //raise our custom AlbumClickedEvent event
            MP3FileClickedEventArgs args = new
                MP3FileClickedEventArgs(MP3FileClickedEvent,
                    (this.DataContext as MP3FileViewModel).FileName);
            RaiseEvent(args);
        }
        #endregion


    }
}
