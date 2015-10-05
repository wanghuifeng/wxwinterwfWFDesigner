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
    /// SetWindowWaitControl.xaml 的交互逻辑
    /// </summary>
    public partial class SetWindowWaitControl : Window
    {
        WaitControl activity;
        List<string> designerNameList;
        System.Random rd = new Random();
        public SetWindowWaitControl(WaitControl waitActivity, List<string> nameList)
        {
            InitializeComponent();
            

            this.designerNameList = nameList;
            this.activity = waitActivity;

            this.activityName.Text = activity.Name;
            

            this.activityX.Text = rd.Next(10, 200).ToString();

            this.activityY.Text = rd.Next(10, 200).ToString();
            
            
           
            this.activityTitle.Text = activity.标题;

            this.lbSubmtOption.ItemsSource = activity.分支集合;

         
            WaitControlData wd = waitActivity.结点数据 as WaitControlData;
           
            if (wd != null)
            {

                this.办理查看业务表单.Text = wd.办理查看业务表单;
                this.办理人员.Text = wd.办理人员;
                this.办理时特殊权限.Text = wd.办理时特殊权限;
                this.办理时限.Text = wd.办理时限.ToString();
                this.办理添写业务表单.Text = wd.办理添写业务表单;

                this.接件部门.Text = wd.接件部门;
                this.接件职能.Text = wd.接件职能;
                this.启动窗体.Text = wd.启动窗体;
                this.activityDescription.Text = wd.说明;
                this.处理方式.Text = wd.处理方式;
            }
        }
 
        public SetWindowWaitControl(WaitControl waitActivity)
        {
            InitializeComponent();
           
            
            this.activity = waitActivity;

            this.activityName.Text = activity.Name;
          
            this.activityX.Text = activity.X坐标.ToString();
            this.activityY.Text = activity.Y坐标.ToString();
            this.activityDescription.Text = activity.说明;
            this.activityTitle.Text = activity.标题;

            this.lbSubmtOption.ItemsSource = activity.分支集合;

           
                this.activityName.IsEnabled = false;
                buttonRemovesubmtItem.IsEnabled = false;

                WaitControlData wd = waitActivity.结点数据 as WaitControlData;

                if (wd != null)
                {
                    this.办理查看业务表单.Text = wd.办理查看业务表单;
                    this.办理人员.Text = wd.办理人员;
                    this.办理时特殊权限.Text = wd.办理时特殊权限;
                    this.办理时限.Text = wd.办理时限.ToString();
                    this.办理添写业务表单.Text = wd.办理添写业务表单;

                    this.接件部门.Text = wd.接件部门;
                    this.接件职能.Text = wd.接件职能;
                    this.启动窗体.Text = wd.启动窗体;
                    this.activityDescription.Text = wd.说明;
                    this.处理方式.Text = wd.处理方式;
                }
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

            switch (this.处理方式.Text)
            {
                case "到人":
                    this.activityDescription.Text ="[" +this.办理人员.Text + "]用户,直接办理";
                    break;

                case "组接件":
                    this.activityDescription.Text = "[" + this.接件部门.Text + "]部门[" + this.接件职能.Text+ "]职能,接件办理";
                    break;


            }




            if (this.activityName.Text == "")
            {
                ErrorInfo.Text = "Name没有添写";
                return;
            }

            if (this.activityName.IsEnabled)
            {
                if (designerNameList.Exists(p => p == this.activityName.Text))
                {
                    ErrorInfo.Text = "指定的Name已存在";
                    return;
                }
            }

            if (this.处理方式.Text  == "")
            {
                ErrorInfo.Text = "办理方式没有添写";
                return;
            }

            double x;
            if (!double.TryParse(this.activityX.Text,out x))
            {
                ErrorInfo.Text = "X坐标应为数字";
                return;
            }

            double y;
            if (!double.TryParse(this.activityY.Text, out y))
            {
                ErrorInfo.Text = "Y坐标应为数字";
                return;
            }

            if (activity.分支集合.Count==0)
            {
                ErrorInfo.Text = "至少要的一个分支选项";
                return;
            }

           activity.Name =this.activityName.Text  ;
           activity.类型 = "节";
           activity.X坐标 = x;
           activity.Y坐标 = y;
           activity.说明=this.activityDescription.Text;
           activity.标题 =this.activityTitle.Text ;


            //-

           WaitControlData wd = new WaitControlData();

          
               wd.办理查看业务表单=this.办理查看业务表单.Text  ;
                wd.办理人员=this.办理人员.Text ;
               wd.办理时特殊权限=this.办理时特殊权限.Text  ;
             
               wd.办理添写业务表单= this.办理添写业务表单.Text ;
               wd.说明= this.activityDescription.Text;
               wd.接件部门=this.接件部门.Text  ;
               wd.接件职能=this.接件职能.Text  ;
               wd.启动窗体=this.启动窗体.Text  ;

               wd.处理方式=this.处理方式.Text  ;

               int n = 0;
               if (int.TryParse(this.办理时限.Text,out n))
               {
                   wd.办理时限 = n;
               }
               else
               {
                   ErrorInfo.Text = "办理时限应为一个数字";
                   return;
               }

               activity.结点数据 = wd;

            //-



            ButtonSelect = "ok";
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            ButtonSelect = "cancel";
        }

        private void buttonAddsubmtItem_Click(object sender, RoutedEventArgs e)
        {
            if (this.txtSubmtItem.Text != null && !activity.分支集合.Exists(p => p == this.txtSubmtItem.Text))
            {
                activity.分支集合.Add(this.txtSubmtItem.Text);
            }
            this.lbSubmtOption.ItemsSource = null;
            this.lbSubmtOption.ItemsSource = activity.分支集合;
           
        }

        private void buttonRemovesubmtItem_Click(object sender, RoutedEventArgs e)
        {
            if (this.lbSubmtOption.SelectedItem != null)
            {
                activity.分支集合.Remove(this.lbSubmtOption.SelectedItem.ToString());
            }
            this.lbSubmtOption.ItemsSource = null;

            this.lbSubmtOption.ItemsSource = activity.分支集合;
        }

        private void editDocButton1_Click(object sender, RoutedEventArgs e)
        {
            this.办理添写业务表单.Text = this.办理查看业务表单.Text;
        }

        private void editDocButton2_Click(object sender, RoutedEventArgs e)
        {
            this.办理添写业务表单.Text = "";
        }

        private void activityName_TextChanged(object sender, TextChangedEventArgs e)
        {
            activityTitle.Text = activityName.Text;
        }
    }
}
