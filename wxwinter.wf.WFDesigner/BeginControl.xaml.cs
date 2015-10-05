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

namespace wxwinter.wf.WFDesigner
{
    /// <summary>
    /// BeginControl.xaml 的交互逻辑
    /// </summary>
    public partial class BeginControl :ActivityControl 
    {
        public BeginControl()
        {
            InitializeComponent();
        }

        private void thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            Canvas.SetLeft(this, Canvas.GetLeft(this) + e.HorizontalChange);
            Canvas.SetTop(this, Canvas.GetTop(this) + e.VerticalChange);

            On刷新事件();
        }
        private void link_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string v;
            if (sender is TextBlock)
            {
                TextBlock ui = sender as TextBlock;

                v = this.Name + "," + ui.Text;

                DragDrop.DoDragDrop(ui, v, DragDropEffects.Link);
            }
            if (sender is Image)
            {
                Image ui = sender as Image;

                v = this.Name + "," + this.Name;

                DragDrop.DoDragDrop(ui, v, DragDropEffects.Link);
            }
        }
        private void thumb_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            int iZindex = Canvas.GetZIndex(this);
            Canvas.SetZIndex(this, iZindex - 1);
        }
        private void thumb_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            int iZindex = Canvas.GetZIndex(this);
            Canvas.SetZIndex(this, iZindex + 1);
        }
    }
}
