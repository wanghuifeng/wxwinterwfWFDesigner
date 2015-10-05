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
    /// SetWindowFlowChart.xaml 的交互逻辑
    /// </summary>
    public partial class SetWindowFlowChart : Window
    {
        public SetWindowFlowChart()
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
            ButtonSelect = "ok";
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            ButtonSelect = "cancel";
        }
    }
}
