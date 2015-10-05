using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Workflow.Runtime;
using System.Xml;
using System.Workflow.Activities;
using wxwinter.wf.WFLib;

namespace wxwinter.wf.Test
{
    delegate void UpdateUI();
    public partial class test : Form
    {
        public test(string xoml, string rules)
        {
            InitializeComponent();
            RefreshWFEvent = new UpdateUI(refreshWFInfoMethod);
            setSubmitEnabled = new UpdateUI(setSubmitEnabledMethod);
            setSubmitResult = new UpdateUI(setSubmitResultMethod);

            this.llSubmit.Enabled = false;

            rtbXoml = xoml;
            rtbRules = rules;
            this.start();
        }

        public void refreshWFInfoMethod()
        {
            lbWFEvent.DataSource = null;
            lbWFEvent.DataSource = lsWFEvent;

            lbWFInfo.DataSource = null;
            if (lsWFInfo != null)
            {
                clearInvalidateProperty(ref lsWFInfo);
            }
            lbWFInfo.DataSource = lsWFInfo;

        }
        public void clearInvalidateProperty(ref List<string> lsWFInfo)
        {
            List<string> exceptList = new List<string>();
            foreach (string p in lsWFInfo)
            {
                if (p.IndexOf(":") != -1)
                {
                    string[] myArray = p.Split(':');
                    if (myArray.Length > 0)
                    {
                        if (string.IsNullOrEmpty(myArray[1]))
                        {
                            exceptList.Add(p);
                        }
                    }
                }
            }


            foreach (string ep in exceptList)
            {
                if (lsWFInfo.Contains(ep))
                {
                    lsWFInfo.Remove(ep);
                }
            }
        }

        public void setSubmitEnabledMethod()
        {
            this.llSubmit.Enabled = false;

        }

        public void setSubmitResultMethod()
        {
            提交结果.Items.Clear();
            string strSumitResult = lsWFInfo.Where(p => p.IndexOf("办理提交选项") != -1).ToList().LastOrDefault();
            if (strSumitResult != null)
            {
                if (strSumitResult.IndexOf(":") != -1)
                {
                    string sr = strSumitResult.Split(':')[1];
                    string[] srs = sr.Split(',');
                    foreach (var v in srs)
                    {
                        提交结果.Items.Add(v);
                    }
                }

            }
        }

        string rtbXoml;
        string rtbRules;

         WorkflowRuntime wfRuntime;
         ExternalDataExchangeService exExchangeService;
         ExternalEvent exEvent;


         WorkflowInstance wfInstance;

         List<string> lsWFEvent;
         List<string> lsWFInfo;

         UpdateUI RefreshWFEvent;
         UpdateUI setSubmitEnabled;
         UpdateUI setSubmitResult;

   
      
        private void llStartWf_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Dictionary<string, object> list = new Dictionary<string, object>();


            list.Add(this.lblpBusinessDescription.Text, this.tbpBusinessDescription.Text);
            list.Add(this.lblpBusinessID.Text, this.tbpBusinessID.Text);
            list.Add(this.lblpBusinessName.Text, this.tbpBusinessName.Text);
            list.Add(this.lblpBusinessType.Text, this.tbpBusinessType.Text);
            list.Add(this.lblpMasterBusinessID.Text, this.tbpMasterBusinessID.Text);
            list.Add(this.lblpSecrecy.Text, this.tbpSecrecy.Text);
            list.Add(this.lblpStartDepartment.Text, this.tbpStartDepartment.Text);
            list.Add(this.lblpStartDuty.Text, this.tbpStartDuty.Text);
            list.Add(this.lblpStartTime.Text, DateTime.Now);
            list.Add(this.lblpStartUser.Text, this.tbpStartUser.Text);
            list.Add(this.lblpUrgency.Text, this.tbpUrgency.Text);

            wfInstance = createWorkflowFormXomlString(rtbXoml, rtbRules, list);
            if (wfInstance != null)
            {
                tbGUID.Text = wfInstance.InstanceId.ToString();
                tbGuid1.Text = wfInstance.InstanceId.ToString();
                this.llSubmit.Enabled = true;

            }
            else
            {
                this.pbError.Visible = true;
                this.lblMessage.Visible = true;
                this.lblMessage.Text = "工作流初始化失败";
            }
        }

        private WorkflowInstance createWorkflowFormXomlString(string xomlstring, string rulesstring, Dictionary<string, object> list)
        {
            Guid guid = Guid.NewGuid();
            try
            {
                System.Text.UTF8Encoding utf8 = new System.Text.UTF8Encoding();

                byte[] a = utf8.GetBytes(rulesstring);

                System.IO.MemoryStream m1 = new System.IO.MemoryStream(a);

                XmlReader rules = XmlReader.Create(m1);

                byte[] b = utf8.GetBytes(xomlstring);

                System.IO.MemoryStream m2 = new System.IO.MemoryStream(b);

                XmlReader xoml = XmlReader.Create(m2);

                System.Workflow.Runtime.WorkflowInstance instance;

                instance = wfRuntime.CreateWorkflow(xoml, rules, list, guid);
                if (instance != null)
                {
                    instance.Start();

                }

                return instance;


            }
            catch (System.Workflow.ComponentModel.Compiler.WorkflowValidationFailedException ex)
            {
                System.Console.WriteLine("err:" + ex.Message);
                return null;
            }
        }

        public void start()
        {

            wfRuntime = new WorkflowRuntime();

            //--通信
            exExchangeService = new ExternalDataExchangeService();
            exEvent = new ExternalEvent();
            wfRuntime.AddService(exExchangeService);
            exExchangeService.AddService(exEvent);

            //-

            lsWFInfo = new List<string>();
            lsWFEvent = new List<string>();

            wfRuntime.AddService(lsWFInfo);

            //事件
            this.wfRuntime.WorkflowCompleted += new EventHandler<WorkflowCompletedEventArgs>(wfRuntime_WorkflowCompleted);
            this.wfRuntime.Started += new EventHandler<WorkflowRuntimeEventArgs>(wfRuntime_Started);
            this.wfRuntime.WorkflowIdled += new EventHandler<WorkflowEventArgs>(wfRuntime_WorkflowIdled);
            this.wfRuntime.WorkflowTerminated += new EventHandler<WorkflowTerminatedEventArgs>(wfRuntime_WorkflowTerminated);
            this.wfRuntime.WorkflowAborted += new EventHandler<WorkflowEventArgs>(wfRuntime_WorkflowAborted);



            //-启动引擎
            wfRuntime.StartRuntime();


        }

        void wfRuntime_WorkflowAborted(object sender, WorkflowEventArgs e)
        {
            Console.WriteLine(e.WorkflowInstance + ":发生错误");
        }

        void wfRuntime_Started(object sender, WorkflowRuntimeEventArgs e)
        {
            string v = string.Format("{0}引擎,在{1},发生{2}", wfRuntime.Name, System.DateTime.Now.ToString(), "Started");
            lsWFEvent.Add(v);

        }

        void wfRuntime_WorkflowTerminated(object sender, WorkflowTerminatedEventArgs e)
        {

            string v = string.Format("{0},{1},{2},{3}", e.WorkflowInstance.InstanceId.ToString(), "WorkflowTerminated", System.DateTime.Now.ToString(), e.Exception.Message);
            lsWFEvent.Add(v);
            this.Invoke(setSubmitEnabled);

        }

        void wfRuntime_WorkflowIdled(object sender, WorkflowEventArgs e)
        {
            string v = string.Format("{0},{1},{2},{3}", e.WorkflowInstance.InstanceId.ToString(), "WorkflowIdled", System.DateTime.Now.ToString(), "-");
            lsWFEvent.Add(v);

            this.Invoke(RefreshWFEvent);
            this.Invoke(setSubmitResult);

        }

        void wfRuntime_WorkflowCompleted(object sender, WorkflowCompletedEventArgs e)
        {
            string v = string.Format("{0},{1},{2},{3}", e.WorkflowInstance.InstanceId.ToString(), "WorkflowCompleted", System.DateTime.Now.ToString(), "-");
            lsWFEvent.Add(v);
            this.Invoke(RefreshWFEvent);
            this.Invoke(setSubmitEnabled);

        }


        private void bt_submit_Click(object sender, EventArgs e)
        {
          
            if (this.提交结果.SelectedItem  != null )
            {
                exEvent.submitResult(new Guid(tbGUID.Text), Guid.Empty, this.提交结果.SelectedItem.ToString(), this.提交人员.Text, this.提交部门.Text, this.提交职能.Text, this.提交方式.Text, this.提交说明.Text, this.触发器类型.Text, this.下一状态办理人员.Text, this.数据表单.Text);
                this.errorProvider1.Clear();
            }
            else
            {
                this.errorProvider1.SetError(this.提交结果, "请输入提交结果");
            }
            提交结果.Text = "";
        }

        private void llClearWFInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.lsWFInfo.Clear();
            this.lbWFInfo.DataSource = null;
        }

        private void llClearWFEvent_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.lsWFEvent.Clear();
            this.lbWFEvent.DataSource = null;
        }
    }
}
