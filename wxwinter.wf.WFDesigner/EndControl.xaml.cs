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
    /// EndControl.xaml 的交互逻辑
    /// </summary>
    public partial class EndControl : ActivityControl 
    {
        public EndControl()
        {
            InitializeComponent();
        }

        private void thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {

            Canvas.SetLeft(this, Canvas.GetLeft(this) + e.HorizontalChange);
            Canvas.SetTop(this, Canvas.GetTop(this) + e.VerticalChange);

            On刷新事件();
        }

        private void UserControl_PreviewDrop(object sender, DragEventArgs e)
        {
            object o = e.Data.GetData(e.Data.GetFormats()[0]);

            On连接事件(o.ToString());

            On刷新事件();
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
