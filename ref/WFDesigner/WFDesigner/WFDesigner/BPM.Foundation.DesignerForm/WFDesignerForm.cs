using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Workflow.Activities;

using System.Workflow.ComponentModel;
using wxwinter.WFDesigner.DesignerForm;
using wxwinter.WFDesigner.Design;
using wxwinter.WFDesigner.DesignerTools;


namespace wxwinter.WFDesigner.DesignerForm
{
    public partial class WFDesignerForm : Form  
    {
        public WFDesignerForm()
        {
            InitializeComponent();
        }
        wxwinter.WFDesigner.Design.WFDesigner myDesigner;


        private void 工作流设计器_Load(object sender, EventArgs e)
        {
            myDesigner = new wxwinter.WFDesigner.Design.WFDesigner();

            myDesigner.Dock = DockStyle.Fill;

            this.面板.Controls.Add(myDesigner);


        }

        private void 新建空工作流ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            myDesigner.XomlFileName = "";
            myDesigner.CreateWorkFlow(@"WFTemplate\状态机工作流.xoml");
        }

        private void 打开Xoml文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            myDesigner.OpenXomlFile();
        }

        private void 保存Xoml文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (myDesigner.RootActivity == null)
            {
                return;
            }
            myDesigner.SaveXomlFile();
        }

        private void xoml文件另存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (myDesigner.RootActivity == null)
            {
                return;
            }
            myDesigner.XomlFileName = "";
            myDesigner.SaveXomlFile();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
     
            myDesigner.XomlFileName = "";
            myDesigner.CreateWorkFlow(@"WFTemplate\顺序工作流.xoml");
        }

        private void 设置CallExternalMethod绑定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CallExternalMethodActivity activity = myDesigner.SelectActivity as CallExternalMethodActivity;
           
            if (activity != null)
            {
                SetCallExternalMethod scm = new SetCallExternalMethod( activity);

                scm.ShowDialog();

                myDesigner.rf();

            }
        }

        private void 设置HandleExternalEvent绑定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HandleExternalEventActivity activity = myDesigner.SelectActivity as HandleExternalEventActivity;

            if (activity != null)
            {
                SetHandleExternalEvent scm = new SetHandleExternalEvent(activity);
                scm.ShowDialog();

                myDesigner.rf();
            }
        }

        private void 查看绑定信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (myDesigner.RootActivity == null)
            {
                return;
            }
            SeeBindingProperty spf = new SeeBindingProperty(myDesigner.RootActivity);
            spf.Show();
        }

        private void 生成业务流程图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (myDesigner.RootActivity is StateActivity)
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    WFImageShow wfi = new WFImageShow(myDesigner.MyXoml, myDesigner.MyLayout);
                    wfi.Draw(saveFileDialog.FileName, "");

                }
            }
        }
   
    }
}