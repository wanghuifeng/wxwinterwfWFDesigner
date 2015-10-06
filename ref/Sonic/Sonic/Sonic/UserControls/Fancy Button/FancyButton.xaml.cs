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

namespace Sonic
{
    /// <summary>
    /// Interaction logic for FancyButton.xaml
    /// </summary>
    public partial class FancyButton : UserControl
    {
        public FancyButton()
        {
            InitializeComponent();
        }

        #region ButtonToolTip

        /// <summary>
        /// ButtonToolTip Dependency Property
        /// </summary>
        public static readonly DependencyProperty ButtonToolTipProperty =
            DependencyProperty.Register("ButtonToolTip", typeof(String), typeof(FancyButton),
                new FrameworkPropertyMetadata((String)String.Empty,
                    new PropertyChangedCallback(OnButtonToolTipChanged)));

        /// <summary>
        /// Gets or sets the ButtonToolTip property.  
        /// </summary>
        public String ButtonToolTip
        {
            get { return (String)GetValue(ButtonToolTipProperty); }
            set { SetValue(ButtonToolTipProperty, value); }
        }

        /// <summary>
        /// Handles changes to the ButtonToolTip property.
        /// </summary>
        private static void OnButtonToolTipChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((FancyButton)d).btn.ToolTip = e.NewValue.ToString();
        }



        #endregion

        #region ButtonCommand

        /// <summary>
        /// The ICommand that the button should use when clicked
        /// </summary>
        public static readonly DependencyProperty ButtonCommandProperty =
            DependencyProperty.Register("ButtonCommand", typeof(ICommand), 
                typeof(FancyButton),
                    new FrameworkPropertyMetadata((ICommand)null,
                        new PropertyChangedCallback(OnButtonCommandChanged)));

        /// <summary>
        /// Gets or sets the ButtonCommand property.  
        /// </summary>
        public ICommand ButtonCommand
        {
            get { return (ICommand)GetValue(ButtonCommandProperty); }
            set { SetValue(ButtonCommandProperty, value); }
        }

        /// <summary>
        /// Handles changes to the ButtonCommand property.
        /// </summary>
        private static void OnButtonCommandChanged(DependencyObject d, 
            DependencyPropertyChangedEventArgs e)
        {
            ((FancyButton)d).btn.Command = (ICommand)e.NewValue;
        }



        #endregion

        
        
    }
}
