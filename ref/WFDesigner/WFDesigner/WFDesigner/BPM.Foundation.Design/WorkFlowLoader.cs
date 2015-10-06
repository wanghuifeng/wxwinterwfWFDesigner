using System;
using System.IO;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Xml;
using System.Workflow.Activities.Rules;


namespace wxwinter.WFDesigner.Design
{
    //工作流加载器
    public sealed class WorkFlowLoader : WorkflowDesignerLoader
    {
       public  Activity  rootActivity = null;

        string xoml;
        public string Xoml
        {
            set { xoml = value; }
            get 
            {

                IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));

                if (host != null && host.RootComponent != null)
                {
                    Activity service = host.RootComponent as Activity;

                    if (service != null)
                    {
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        XmlWriter xmlWriter = XmlWriter.Create(sb);

                        WorkflowMarkupSerializer xomlSerializer = new WorkflowMarkupSerializer();
                        xomlSerializer.Serialize(xmlWriter, service);
                        return sb.ToString().Replace("utf-16", "utf-8");
                    }
                }
                
                return ""; 
            
            
            }

        }

        string layout;
        public string Layout
        {
            set 
            { 
                layout = value; 
            }
            
            get
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                XmlWriter xmlWriter = XmlWriter.Create(sb);
                IList layoutSaveErrors = new ArrayList() as IList;
                IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));
                ActivityDesigner rootDesigner = host.GetDesigner(host.RootComponent) as ActivityDesigner;
                this.SaveDesignerLayout(xmlWriter, rootDesigner, out layoutSaveErrors);

                return sb.ToString().Replace("utf-16", "utf-8"); 
            }
        }

        string rules;
        public string Rules
        {
            set { rules = value; }
            
            get {
                IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));

                if (host != null && host.RootComponent != null)
                {
                    Activity service = host.RootComponent as Activity;

                    if (service != null)
                    {

                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        XmlWriter xmlWriter = XmlWriter.Create(sb);
                        WorkflowMarkupSerializer xomlSerializer = new WorkflowMarkupSerializer();
                        object obj = service.GetValue(RuleDefinitions.RuleDefinitionsProperty);
                        xomlSerializer.Serialize(xmlWriter, obj);
                        return sb.ToString().Replace("utf-16", "utf-8");
                    }
                    
                }
                
                    return "";
            }
        }

        public void LoadLayout()
        {

            if (string.IsNullOrEmpty(this.layout))
            {
                return;
            }
            TextReader readerLayout = new StringReader(this.layout);
            using (XmlReader xmlReader = XmlReader.Create(readerLayout))
            {
                IList alist = new ArrayList();

                LoadDesignerLayout(xmlReader, out alist);
            }
        }


        string activityToolItemFile;

        internal WorkFlowLoader(string activityToolItemFile)
        {
            this.activityToolItemFile = activityToolItemFile;

        }

        protected override void Initialize()
        {
            base.Initialize();
            //提供一个接口，该接口可扩展设计器宿主以支持从序列化状态加载
            IDesignerLoaderHost host;

            //获取加载程序宿主
            host = this.LoaderHost;

            if (host != null)
            {
                //加载菜单服务
                host.AddService(typeof(IMenuCommandService), new WFMenu(host));

                //加载工具栏服务
                host.AddService(typeof(IToolboxService), new ToolBar(host, activityToolItemFile));

                TypeProvider typeProvider = new TypeProvider(host);
                typeProvider.AddAssemblyReference(typeof(string).Assembly.Location);
                host.AddService(typeof(ITypeProvider), typeProvider, true);
            }
        }

        public override void Dispose()
        {
            IDesignerLoaderHost host = LoaderHost;
            if (host != null)
            {
                host.RemoveService(typeof(IMenuCommandService));
                host.RemoveService(typeof(IToolboxService));
                host.RemoveService(typeof(ITypeProvider), true);
            }

            base.Dispose();
        }

        public override void ForceReload()
        {
        }

        public override string FileName
        {
            get { return string.Empty; }
        }

        public override TextReader GetFileReader(string filePath)
        {
            return new StreamReader(new FileStream(filePath, FileMode.OpenOrCreate));
        }

        public override TextWriter GetFileWriter(string filePath)
        {
               return new StreamWriter(new FileStream(filePath, FileMode.OpenOrCreate));
        }

        protected override void PerformLoad(IDesignerSerializationManager serializationManager)
        {
            if (string.IsNullOrEmpty(this.xoml))
            {
                return;
            }

            IDesignerHost designerHost = (IDesignerHost)GetService(typeof(IDesignerHost));
           

            
            
      
                TextReader readerXoml = new StringReader(this.xoml);
                TextReader readerRules = new StringReader(this.rules);

                WorkflowMarkupSerializer mySerializer = new WorkflowMarkupSerializer();
                try
                {
                    using (XmlReader xmlReader = XmlReader.Create(readerXoml))
                    {
                        rootActivity = mySerializer.Deserialize(xmlReader) as Activity;

                    }

                    using (XmlReader xmlReader = XmlReader.Create(readerRules))
                    {
                        object obj = mySerializer.Deserialize(xmlReader);
                        RuleDefinitions rule = obj as RuleDefinitions;
                        rootActivity.SetValue(RuleDefinitions.RuleDefinitionsProperty, rule);
                    }
   

                }

                finally
                {
                    readerXoml.Close();
                    readerRules.Close();
                }
          

            if (rootActivity != null && designerHost != null)
            {
                AddObjectGraphToDesignerHost(designerHost, rootActivity);
            }
        }

        protected override void PerformFlush(IDesignerSerializationManager manager)
        {
        }

         private static void AddObjectGraphToDesignerHost(IDesignerHost designerHost, Activity activity)
        {

            Guid Definitions_Class = new Guid("11111111-2222-3333-4444-555555555555");

            if (designerHost == null)
                throw new ArgumentNullException("designerHost");
            if (activity == null)
                throw new ArgumentNullException("activity");

            string rootSiteName = activity.QualifiedName;
            if (activity.Parent == null)
            {
                string fullClassName = activity.UserData[Definitions_Class] as string;
                if (fullClassName == null)
                    fullClassName = activity.GetType().FullName;
                rootSiteName = (fullClassName.LastIndexOf('.') != -1) ? fullClassName.Substring(fullClassName.LastIndexOf('.') + 1) : fullClassName;
                designerHost.Container.Add(activity, rootSiteName);
            }
            else
            {
                designerHost.Container.Add(activity, activity.QualifiedName);
            }

            if (activity is CompositeActivity)
            {
                     foreach (Activity activity2 in GetNestedActivities(activity as CompositeActivity))
                    { designerHost.Container.Add(activity2, activity2.QualifiedName); }
        
            }
        }

        private static Activity[] GetNestedActivities(CompositeActivity compositeActivity)
        {
            if (compositeActivity == null)
                throw new ArgumentNullException("compositeActivity");

            IList<Activity> childActivities = null;
            ArrayList nestedActivities = new ArrayList();
            Queue compositeActivities = new Queue();
            compositeActivities.Enqueue(compositeActivity);
            while (compositeActivities.Count > 0)
            {
                CompositeActivity compositeActivity2 = (CompositeActivity)compositeActivities.Dequeue();
                childActivities = compositeActivity2.Activities;

                foreach (Activity activity in childActivities)
                {
                    nestedActivities.Add(activity);
                    if (activity is CompositeActivity)
                        compositeActivities.Enqueue(activity);
                }
            }
            return (Activity[])nestedActivities.ToArray(typeof(Activity));
        }

        internal static void DestroyObjectGraphFromDesignerHost(IDesignerHost designerHost, Activity activity)
        {
            if (designerHost == null)
                throw new ArgumentNullException("designerHost");
            if (activity == null)
                throw new ArgumentNullException("activity");

            designerHost.DestroyComponent(activity);

            if (activity is CompositeActivity)
            {
                foreach (Activity activity2 in GetNestedActivities(activity as CompositeActivity))
                    designerHost.DestroyComponent(activity2);
            }
        }

    }
}
