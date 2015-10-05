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

    public partial class WaitControl : ActivityControl
    {
        public WaitControl()
        {
            InitializeComponent();
            显示分支选项();
        }

        public  override  string 标题
        {
            set
            { this.title.Text = value; }
            get 
            { return this.title.Text; }
        }
        public  override string  说明
        {
            set
            { this.description.Text = value; }
            get
            { return this.description.Text; }
        }

        public override void 显示分支选项()
        {
            submtOption.ItemsSource = null;
            submtOption.ItemsSource = this.分支集合;
        }

        public override  void 设为活动()
        {
            img_activite.Visibility = Visibility.Visible;
        }

   
        private void thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            Canvas.SetLeft(this, Canvas.GetLeft(this) + e.HorizontalChange);
            Canvas.SetTop(this,Canvas.GetTop(this) + e.VerticalChange);

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
        private void Thumb_Link(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            System.Windows.UIElement ui = (System.Windows.UIElement)sender;

            double x = Canvas.GetLeft(ui) + e.HorizontalChange; //移动的水平距离
            double y = Canvas.GetTop(ui) + e.VerticalChange; //移动的坚直距离

            Canvas.SetLeft(ui, x);
            Canvas.SetTop(ui, y);
        
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

        private void UserControl_PreviewDrop(object sender, DragEventArgs e)
        {
         
            object o = e.Data.GetData(e.Data.GetFormats()[0]);

            On连接事件(o.ToString());

            On刷新事件();
        }

        private void del_MouseUp(object sender, MouseButtonEventArgs e)
        {
           var v= System.Windows.MessageBox.Show("删除" + this.标题 + "?","", MessageBoxButton.YesNo);
           if (v == MessageBoxResult.Yes)
           {
               this.删除();
           }
            
        }

        private void setData_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ToolWindows.SetWindowWaitControl swc = new ToolWindows.SetWindowWaitControl(this);

            swc.ShowDialog();

            swc.Close();

            this.显示分支选项();
        }



  
    }

    
}
