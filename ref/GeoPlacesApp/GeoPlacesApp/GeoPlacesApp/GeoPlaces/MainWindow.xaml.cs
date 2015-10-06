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
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Media.Animation;

using GeoPlacesModel;
using GeoPlacesDataService;
using System.Threading;
using System.Text.RegularExpressions;
using System.Globalization;

namespace GeoPlaces
{
    /// <summary>
    /// Holds 3 controls in a scrollable area
    /// the 3 controls are
    /// -LoginControl
    /// -AddViewPlacesControl
    /// -VEMapControl
    /// </summary>
    public partial class MainWindow : Window, IView
    {
        #region Data
        private Boolean isCurrentlyScrolling = false;
        private MainWindowViewModel mainWindowViewModel = null;
        private Int32 currentViewablePart = 1;
        private const Int32 MAX_PARTS = 3;
        private Double currentViewOffset = 0;
        #endregion

        #region IView Members

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        #endregion

        #region Ctor
        public MainWindow()
        {
            InitializeComponent();

            mainWindowViewModel = new MainWindowViewModel(this);

            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }
        #endregion

        #region Private Methods
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = mainWindowViewModel;
        }

        private void win_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            canv.Width = 3 * gd.ActualWidth;

            part1.SetValue(Canvas.LeftProperty, (double)0);
            part2.SetValue(Canvas.LeftProperty, (double)gd.ActualWidth);
            part3.SetValue(Canvas.LeftProperty, (double)gd.ActualWidth * 2);

            if (IsLoaded)
            {

                switch (currentViewablePart)
                {
                    case 1:
                        currentViewOffset = 0;
                        break;
                    case 2:
                    case 3:
                        currentViewOffset = -1 * (gd.ActualWidth * (currentViewablePart - 1));
                        break;
                }
                AnimateToViewPosition();
            }
        }


        private void SlideButtonLeft_Clicked(object sender, RoutedEventArgs e)
        {
            if (isCurrentlyScrolling)
                return;

            if (currentViewablePart == 1)
                return;

            currentViewOffset += gd.ActualWidth;
            currentViewablePart--;

            AnimateToViewPosition();
        }


        private void SlideButtonRight_Clicked(object sender, RoutedEventArgs e)
        {
            if (isCurrentlyScrolling)
                return;

            if (currentViewablePart == MAX_PARTS)
                return;

            currentViewOffset = (double)-1 * (currentViewablePart * gd.ActualWidth);
            currentViewablePart++;

            AnimateToViewPosition();
        }


        private void AnimateToViewPosition()
        {
            DoubleAnimation anim = new DoubleAnimation(currentViewOffset, TimeSpan.FromMilliseconds(250));
            anim.DecelerationRatio = 0.4;
            anim.Completed += new EventHandler(TranslateAnim_Completed);
            isCurrentlyScrolling = true;
            trans.BeginAnimation(TranslateTransform.XProperty, anim);
        }

        private void TranslateAnim_Completed(object sender, EventArgs e)
        {
            isCurrentlyScrolling = false;
        }
        #endregion
    }

}
