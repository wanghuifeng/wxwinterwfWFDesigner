using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.Activities;
using System.Workflow.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Workflow.ComponentModel.Serialization;
using System.Xml;
using System.CodeDom.Compiler;
using System.Workflow.Runtime;
using System.Reflection;
using System.Threading;
using System.Drawing.Imaging;
namespace wxwinter.WFDesigner.Design
{
    public class WFDesigner : UserControl, IDisposable, IServiceProvider
    {
        public event System.EventHandler FileOpened = null;
        public event System.EventHandler FileSaveed = null;
        public event System.EventHandler WorkFlowLoaded = null;
        public event System.EventHandler CreateWorkFlowed = null;
  
        // Xoml字串
        public string MyXoml
        {
            get
            {
                 this.loader.Flush();
                 string s = this.loader.Xoml;
                 return s;
            }



        }

        //Rules字串
        public string MyRules
        {
            get
            {
                this.loader.Flush();
                string s = this.loader.Rules;
                return s;
            }

   
        
        }

        public string MyLayout
        {
            get
            {
                this.loader.Flush();
                string s = this.loader.Layout;
                return s;
            }

        

        }

        public Activity RootActivity
        {

            get
            {
                if (loader == null)
                {
                    return null;
                }
                if (loader.rootActivity == null)
                {
                    return null;
                }
                return loader.rootActivity; 
            }
        }

        public string XomlFileName="";

        string ToolsFile = "";
    
        private WorkflowView workflowView;
        private DesignSurface designSurface;
        private WorkFlowLoader loader;

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private Splitter splitter1;
        private SplitContainer splitContainer1;

        private System.ComponentModel.IContainer components = null;

        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(159, 547);
            this.panel1.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(401, 547);
            this.panel2.TabIndex = 4;
            // 
            // propertyGrid
            // 
            this.propertyGrid.CommandsVisibleIfAvailable = false;
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(132, 547);
            this.propertyGrid.TabIndex = 0;
            this.propertyGrid.ToolbarVisible = false;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(159, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 547);
            this.splitter1.TabIndex = 5;
            this.splitter1.TabStop = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(162, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.propertyGrid);
            this.splitContainer1.Size = new System.Drawing.Size(537, 547);
            this.splitContainer1.SplitterDistance = 401;
            this.splitContainer1.TabIndex = 6;
            // 
            // WFDesigner
            // 
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel1);
            this.Name = "WFDesigner";
            this.Size = new System.Drawing.Size(699, 547);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                UnloadWorkflow();
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        public WFDesigner()
        {
            var v = WorkflowTheme.Load(@"myTheme.wtm");
            if (v != null)
            {
                WorkflowTheme.CurrentTheme = v;
            }

            this.ToolsFile = System.AppDomain.CurrentDomain.BaseDirectory + "ActivityTooIteml.txt";
            InitializeComponent();

            ToolBar toolbox = new ToolBar(this, ToolsFile);

            panel1.Controls.Add(toolbox);
            toolbox.Dock = DockStyle.Fill;
            toolbox.BackColor = BackColor;
            toolbox.Font = WorkflowTheme.CurrentTheme.AmbientTheme.Font;

            WorkflowTheme.CurrentTheme.ReadOnly = false;
            WorkflowTheme.CurrentTheme.AmbientTheme.ShowConfigErrors = true;


        
            

            this.propertyGrid.BackColor = BackColor;
            this.propertyGrid.Font = WorkflowTheme.CurrentTheme.AmbientTheme.Font;



        }

        public void SaveToPNG()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Png files (*.Png)|*.Png|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.FileName = loader.rootActivity.Name;
            saveFileDialog.ShowDialog();
            workflowView.SaveWorkflowImage(saveFileDialog.FileName, ImageFormat.Png);
        }

   
        public void Zoom(int zoom)
        {
            this.workflowView.Zoom = zoom;
            this.workflowView.Update();
        }


        public void 删除选中结点()
        {
            ISelectionService selectionService = (ISelectionService)this.GetService(typeof(ISelectionService));

            if (selectionService != null)
            {
                if (selectionService.PrimarySelection is Activity)
                {
                    Activity activity = (Activity)selectionService.PrimarySelection;

                    //if (activity.Name != this.WorkflowName)
                    {
                        activity.Parent.Activities.Remove(activity);
                        this.workflowView.Update();
                    }
                }
            }
        }


        public void OpenXomlFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "xoml files (*.xoml)|*.xoml|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {

                OpenXomlFile(openFileDialog.FileName);

                if (this.FileOpened != null)
                {
                    FileOpened(this, System.EventArgs.Empty);
                }
                          
            }

        }


        void OpenXomlFile(string XomlFileName)
        {
            string xoml="";
            string rules="";
            string layout="";

            using (System.IO.StreamReader sr = new StreamReader(XomlFileName))
            {
                xoml = sr.ReadToEnd();
            }
            if (System.IO.File.Exists(XomlFileName.Replace(".xoml", ".rules")))
            {
                using (System.IO.StreamReader sr = new StreamReader(XomlFileName.Replace(".xoml", ".rules")))
                {
                    rules = sr.ReadToEnd();
                }
            }
            if (System.IO.File.Exists(XomlFileName.Replace(".xoml", ".layout")))
            {
                using (System.IO.StreamReader sr = new StreamReader(XomlFileName.Replace(".xoml", ".layout")))
                {
                    layout = sr.ReadToEnd();
                }
            }
            LoadWorkflow(xoml, rules, layout);
        }

        public void SaveXomlFile()
        {

            if (loader.rootActivity == null)
            {
                return;
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "xoml files (*.xoml)|*.xoml|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.FileName = loader.rootActivity.Name;

            if (XomlFileName != "")
            {
                save();
        
            }
            else
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {

                    //保存xoml
                    XomlFileName = saveFileDialog.FileName;
                    save();
                }

                if (this.FileSaveed  != null)
                {
                    this.FileSaveed(this, System.EventArgs.Empty);
                }
            }
        }


        void save()
        {

            using (System.IO.StreamWriter s = new StreamWriter(XomlFileName, false, System.Text.UTF8Encoding.UTF8))
            {
                string k1 = loader.Xoml;
                s.Write(k1);
                s.Flush();
            }

            //保存rules
            string rulesfilename = XomlFileName.Replace(".xoml", ".rules");
            using (System.IO.StreamWriter s = new StreamWriter(rulesfilename, false, System.Text.UTF8Encoding.UTF8))
            {
                string k2 = loader.Rules;
                s.Write(k2);
                s.Flush();
            }

            //保存layout
            string layoutfilename = XomlFileName.Replace(".xoml", ".layout");
            using (System.IO.StreamWriter s = new StreamWriter(layoutfilename, false, System.Text.UTF8Encoding.UTF8))
            {
                string k3 = loader.Layout;
                s.Write(k3);
                s.Flush();
            }

            XomlFileName = "";
        }

        public new object GetService(Type serviceType)
        {
            return (this.workflowView != null) ? ((IServiceProvider)this.workflowView).GetService(serviceType) : null;
        }

        public  void LoadWorkflow(string xoml, string rules, string layout)
        {
            SuspendLayout();

            DesignSurface designSurface = new DesignSurface();
            loader = new WorkFlowLoader(ToolsFile);
            loader.Xoml = xoml;
            loader.Rules = rules;
            loader.Layout = layout;
            
            designSurface.BeginLoad(loader);
            IDesignerHost designerHost;
            designerHost = designSurface.GetService(typeof(IDesignerHost)) as IDesignerHost;
            if (designerHost != null && designerHost.RootComponent != null)
            {
                propertyGrid.Site = designerHost.RootComponent.Site;

                IRootDesigner rootDesigner = designerHost.GetDesigner(designerHost.RootComponent) as IRootDesigner;
                if (rootDesigner != null)
                {

                    UnloadWorkflow();

                    this.designSurface = designSurface;
                    
                    this.workflowView = rootDesigner.GetView(ViewTechnology.Default) as WorkflowView;
                    panel2.Controls.Add(this.workflowView);
                    this.workflowView.Dock = DockStyle.Fill;
                    this.workflowView.TabIndex = 1;
                    this.workflowView.TabStop = true;
                    this.workflowView.HScrollBar.TabStop = false;
                    this.workflowView.VScrollBar.TabStop = false;
                    this.workflowView.Focus();
                

                    ISelectionService selectionService = GetService(typeof(ISelectionService)) as ISelectionService;

                    if (selectionService != null)
                    {
                        selectionService.SelectionChanged += new EventHandler(OnSelectionChanged);
                    }
                }
            }
            loader.LoadLayout();
            ResumeLayout(true);
            if (this.WorkFlowLoaded != null)
            {
                WorkFlowLoaded(this, System.EventArgs.Empty);
            }
        }

        private void UnloadWorkflow()
        {
            IDesignerHost designerHost;
            designerHost = GetService(typeof(IDesignerHost)) as IDesignerHost;
            if (designerHost != null && designerHost.Container.Components.Count > 0)
                WorkFlowLoader.DestroyObjectGraphFromDesignerHost(designerHost, designerHost.RootComponent as Activity);

            if (this.designSurface != null)
            {
                this.designSurface.Dispose();
                this.designSurface = null;
            }

            if (this.workflowView != null)
            {
                Controls.Remove(this.workflowView);
                this.workflowView.Dispose();
                this.workflowView = null;
            }
        }
        public Activity SelectActivity = null;
        private void OnSelectionChanged(object sender, EventArgs e)
        {
            ISelectionService selectionService = GetService(typeof(ISelectionService)) as ISelectionService;

            if (selectionService != null)
            {
               object[] list = new ArrayList(selectionService.GetSelectedComponents()).ToArray();
                if (list != null && list.Length > 0)
                {
                    SelectActivity = list[0] as Activity;
                    this.propertyGrid.SelectedObjects = list;
                }
            }
        }
        
        public void CreateWorkFlow(string XomlFileName)
        {
            OpenXomlFile(XomlFileName);

            if (this.CreateWorkFlowed != null)
            {
                CreateWorkFlowed(this, System.EventArgs.Empty);
            }
        }

        public void rf()
        {
            LoadWorkflow(MyXoml, MyRules, MyLayout);
        }

    }
}
