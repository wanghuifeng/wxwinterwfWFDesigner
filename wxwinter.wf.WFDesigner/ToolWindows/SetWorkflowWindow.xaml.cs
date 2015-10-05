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
using System.Windows.Shapes;

namespace wxwinter.wf.WFDesigner.ToolWindows
{
    /// <summary>
    /// SetWorkflowWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SetWorkflowWindow : Window
    {
        public SetWorkflowWindow()
        {
            InitializeComponent();
        }

        private string buttonSelect = "cancel";

        public string ButtonSelect
        {
            get
            {
                this.Hide();
                return buttonSelect;
            }
            set
            {
                this.Hide();
                buttonSelect = value;
            }
        }
        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            if (this.模板编号.Text == "")
            {
                return;
            }
            ButtonSelect = "ok";
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            ButtonSelect = "cancel";
        }

        private void 模板编号_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.模板名称.Text = this.模板编号.Text;
        }
    }
}
