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

namespace GeoPlaces
{
    /// <summary>
    /// A simple pulsing ring control
    /// </summary>
    public partial class PulsingRingControl : UserControl
    {
        #region Ctor
        public PulsingRingControl()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(PulsingRingControl_Loaded);
        }
        #endregion

        #region Private Methods
        private void PulsingRingControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (StartAnimatingFromStart)
            {
                Storyboard sb = this.TryFindResource("pulseRingsFromBeginning") as Storyboard;
                sb.Begin(this);
            }
            else
            {
                Storyboard sb = this.TryFindResource("pulseRingsFromEnd") as Storyboard;
                sb.Begin(this);
            }
        }
        #endregion

        #region DPs
        #region StartAnimatingFromStart

        /// <summary>
        /// StartAnimatingFromStart Dependency Property
        /// </summary>
        public static readonly DependencyProperty StartAnimatingFromStartProperty =
            DependencyProperty.Register("StartAnimatingFromStart", typeof(bool), typeof(PulsingRingControl),
                new FrameworkPropertyMetadata((bool)false));

        /// <summary>
        /// Gets or sets the StartAnimatingFromStart property.  
        /// </summary>
        public bool StartAnimatingFromStart
        {
            get { return (bool)GetValue(StartAnimatingFromStartProperty); }
            set { SetValue(StartAnimatingFromStartProperty, value); }
        }




        #endregion
        #endregion

    }
}
