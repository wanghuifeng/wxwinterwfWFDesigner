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


namespace wxwinter.wf.WFDesigner
{
    /// <summary>
    /// DesignerWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DesignerWindow : Window
    {
       
        public DesignerWindow()
        {
            InitializeComponent();

            this.designer.PageHeight = 1500;
        }

  
        bool isCreate = false;
        private void buttonWaitActivity_Click(object sender, RoutedEventArgs e)
        {
            if (isCreate)
            {
                WaitControlData wd = new WaitControlData();
                wd.办理查看业务表单 = this.designer.流程数据.启动时填写的表单;
                wd.办理时限 = 5;
                wd.处理方式 = "组接件";

                WaitControl wc = new WaitControl() { X坐标 = 150, Y坐标 = 200, 类型 = "节", 结点数据=wd };

                ToolWindows.SetWindowWaitControl swc = new ToolWindows.SetWindowWaitControl(wc, designer.NameList);
                swc.办理人员.IsEnabled = false;
                swc.处理方式.IsReadOnly = true;
                swc.ShowDialog();

                if (swc.ButtonSelect == "ok")
                {

                    designer.AddActivity(wc);
                }
                swc.Close();
            }
        }

        private void buttonScaleTransform_Click(object sender, RoutedEventArgs e)
        {
            MenuItem bt = sender as MenuItem;
            switch (bt.Header.ToString())
            {
                case "10%" :
                    this.designer.PageSize = 0.1;
                    break;
                case "30%":
                    this.designer.PageSize = 0.3;
                    break;
                case "50%":
                    this.designer.PageSize = 0.5;
                    break;
                case "70%":
                    this.designer.PageSize = 0.7;
                    break;
                case "90%":
                    this.designer.PageSize = 0.9;
                    break;
                case "100%":
                    this.designer.PageSize = 1;
                    break;
                case "120%":
                    this.designer.PageSize = 1.2;
                    break;
                case "150%":
                    this.designer.PageSize = 1.5;
                    break;
                case "200%":
                    this.designer.PageSize = 2;
                    break;
                case "300%":
                    this.designer.PageSize = 3;

                    break;

            }
        }

        private void buttonSize_Click(object sender, RoutedEventArgs e)
        {
            if (isCreate)
            {
                ToolWindows.SetWindowFlowChart setWindowFlowChart = new ToolWindows.SetWindowFlowChart();
                setWindowFlowChart.tbHeight.Text = this.designer.PageHeight.ToString();
                setWindowFlowChart.tbWidth.Text = this.designer.PageWidth.ToString();
                setWindowFlowChart.ShowDialog();
                if (setWindowFlowChart.ButtonSelect == "ok")
                {

                    double w;
                    if (double.TryParse(setWindowFlowChart.tbWidth.Text, out w))
                    {
                        this.designer.PageWidth = w;
                    }
                    double h;

                    if (double.TryParse(setWindowFlowChart.tbHeight.Text, out h))
                    {
                        this.designer.PageHeight = h;
                    }
                }
                setWindowFlowChart.Close();
            }

        }

        private void menu_Clear_Click(object sender, RoutedEventArgs e)
        {
            this.designer.Clear(false);
            isCreate = false;
        }

        private void menu_Create_Click(object sender, RoutedEventArgs e)
        {
            this.designer.Clear(true);
            ToolWindows.SetWorkflowWindow sw = new wxwinter.wf.WFDesigner.ToolWindows.SetWorkflowWindow();
            if (designer.流程数据 != null)
            {
                sw.模板版本.Text = designer.流程数据.模板版本;
                sw.模板编号.Text = designer.流程数据.模板编号;
                sw.模板名称.Text = designer.流程数据.模板名称;
                sw.模板说明.Text = designer.流程数据.模板说明;
                sw.启动时填写的表单.Text = designer.流程数据.启动时填写的表单;
                sw.数据表单列表.Text = designer.流程数据.数据表单列表;
         
                sw.ShowDialog();
                
                designer.流程数据.模板版本 = sw.模板版本.Text;
                designer.流程数据.模板编号 = sw.模板编号.Text;
                designer.流程数据.模板名称 = sw.模板名称.Text;
                designer.流程数据.模板说明 = sw.模板说明.Text;
                designer.流程数据.启动时填写的表单 = sw.启动时填写的表单.Text;
                designer.流程数据.数据表单列表 = sw.数据表单列表.Text;
                
                sw.Close();


            isCreate = true;
            }
        }

        private void buttonSetWorkflow_Click(object sender, RoutedEventArgs e)
        {
            if (isCreate)
            {
                ToolWindows.SetWorkflowWindow sw = new wxwinter.wf.WFDesigner.ToolWindows.SetWorkflowWindow();
                if (designer.流程数据 !=null )
                {
                    sw.模板版本.Text = designer.流程数据.模板版本;
                    sw.模板编号.Text = designer.流程数据.模板编号;
                    sw.模板名称.Text = designer.流程数据.模板名称;
                    sw.模板说明.Text = designer.流程数据.模板说明;
                    sw.启动时填写的表单.Text = designer.流程数据.启动时填写的表单;
                    sw.数据表单列表.Text = designer.流程数据.数据表单列表;
                
                    sw.ShowDialog();
                       designer.流程数据.模板版本 = sw.模板版本.Text ;
                       designer.流程数据.模板编号  = sw.模板编号.Text;
                       designer.流程数据.模板名称 = sw.模板名称.Text  ;
                       designer.流程数据.模板说明 = sw.模板说明.Text  ;
                       designer.流程数据.启动时填写的表单 = sw.启动时填写的表单.Text  ;
                       designer.流程数据.数据表单列表 = sw.数据表单列表.Text  ;
                   sw.Close();
                
                }
            }
        }

        private void buttonSelfWaitActivity_Click(object sender, RoutedEventArgs e)
        {
            if (isCreate)
            {
                WaitControlData wd = new WaitControlData();
                wd.处理方式 = "到人";
                wd.办理时限 = 5;

                wd.办理查看业务表单 = this.designer.流程数据.启动时填写的表单;
                wd.办理人员 = "";
                
                WaitControl wc = new WaitControl() { X坐标 = 150, Y坐标 = 200, 类型 = "节", 结点数据=wd };
                
                ToolWindows.SetWindowWaitControl swc = new ToolWindows.SetWindowWaitControl(wc, designer.NameList);

                swc.接件部门.IsEnabled = false;
                swc.接件职能.IsEnabled = false;
                swc.处理方式.IsReadOnly = true;

                swc.ShowDialog();

                if (swc.ButtonSelect == "ok")
                {

                    designer.AddActivity(wc);
                }
                swc.Close();
            }
        }

        private void menu_Test_Click(object sender, RoutedEventArgs e)
        {
            if (isCreate)
            {
                XomlLoder xl = new XomlLoder();
                WindowsWorkflowObject wfo = xl.GetFlow<WindowsWorkflowObject>(designer);




                if (wfo == null)
                {
                    MessageBox.Show("流程验证失败");
                    return;
                }

                wxwinter.wf.Test.test ts = new wxwinter.wf.Test.test(wfo.Xoml, wfo.Rules);
                ts.Show();

            }

        }




    }
}
