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

    public delegate void AlbumClickedEventHandler(object sender, 
    AlbumClickedEventArgs e);


    /// <summary>
    /// Interaction logic for AlbumView.xaml
    /// </summary>
    public partial class AlbumView : UserControl
    {
        public AlbumView()
        {
            InitializeComponent();
            this.DataContextChanged+=AlbumView_DataContextChanged;
        }

        #region Events
        /// <summary>
        /// Raised when Album item clicked
        /// </summary>
        public static readonly RoutedEvent AlbumClickedEvent =
                EventManager.RegisterRoutedEvent(
                "AlbumClicked", RoutingStrategy.Bubble,
                typeof(AlbumClickedEventHandler),
                typeof(AlbumView));

        public event AlbumClickedEventHandler AlbumClicked
        {
            add { AddHandler(AlbumClickedEvent, value); }
            remove { RemoveHandler(AlbumClickedEvent, value); }
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Hook up the associated AlbumOfMP3ViewModel
        /// AnimationStartTimerExpiredEvent event
        /// </summary>
        private void  AlbumView_DataContextChanged(object sender, 
            DependencyPropertyChangedEventArgs e)
        {
            AlbumOfMP3ViewModel viewModel = e.NewValue as AlbumOfMP3ViewModel;
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
                    sb.Begin(this.btn);
                }
            }, DispatcherPriority.Normal);
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            //raise our custom AlbumClickedEvent event
            AlbumClickedEventArgs args = new
                AlbumClickedEventArgs(AlbumClickedEvent, 
                    this.DataContext as AlbumOfMP3ViewModel);
            RaiseEvent(args);
        }
        #endregion
    }
}
