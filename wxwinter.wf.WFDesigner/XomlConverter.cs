using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Workflow.ComponentModel;
using System.Workflow.Activities;
using System.IO;
using System.Workflow.ComponentModel.Serialization;
using System.ComponentModel.Design.Serialization;
using System.Workflow.Activities.Rules;

namespace wxwinter.wf.WFDesigner
{
  public  class XomlConverter : IConverter
    {
        List<Element> ElementList =new List<Element>();

        List<ElementRelation> ElementRelationList = new List<ElementRelation>(); 

        string Xoml;
        
        string Rules;
        
        string Layout;

       

        public XomlConverter()
        {

        }

        public XomlConverter(string xoml,string rules, string layout)
        {
            Xoml = xoml;
            Rules = rules;
            Layout = layout;
        }

        void ShowState(object activity)
        {
            if (activity is System.Workflow.ComponentModel.CompositeActivity)
            {
                CompositeActivity Workflow = activity as CompositeActivity;
                foreach (var v in Workflow.EnabledActivities)
                {
                    var ee = new Element();
                    ee.类型 = "节";
                    ee.说明 = v.Description;
                    ee.Name = v.Name;
                    ee.X坐标 = 0;
                    ee.Y坐标 = 0;
                    ee.分支集合 = new List<string>();
                    if (v is StateActivity)
                    {
                        StateActivity sta = v as StateActivity;

                        foreach (var c in sta.EnabledActivities)
                        {
                            foreach (var cc in ((CompositeActivity)c).EnabledActivities)
                            {

                                if (cc is wxwinter.wf.WFLib.等待提交)
                                {
                                    wxwinter.wf.WFLib.等待提交 waitsubmit = cc as wxwinter.wf.WFLib.等待提交;
                                    WaitControlData wd = new WaitControlData();
                                    wd.办理查看业务表单 = waitsubmit.办理查看业务表单;
                                    wd.办理人员 = waitsubmit.办理人员;
                                   
                                    wd.办理时限 = waitsubmit.办理时限;
                                    wd.办理提交选项 = waitsubmit.办理提交选项;
                                    wd.办理添写业务表单 = waitsubmit.办理添写业务表单;
                                    wd.处理方式 = waitsubmit.处理方式;
                                    wd.接件部门 = waitsubmit.接件部门;
                                    wd.接件职能 = waitsubmit.接件职能;
                                    wd.启动窗体 = waitsubmit.启动窗体;
                                    wd.说明 = waitsubmit.Description;
                                    ee.结点数据 = wd;
                                }

                                if (cc is wxwinter.wf.WFLib.条件容器)
                                {
                                    wxwinter.wf.WFLib.条件容器 ic = cc as wxwinter.wf.WFLib.条件容器;

                                    foreach (var fz in ic.EnabledActivities)
                                    {
                                        wxwinter.wf.WFLib.条件分支 ifz = fz as wxwinter.wf.WFLib.条件分支;
                                        ee.分支集合.Add(ifz.条件);
                                    }
                                }
                            }
                        }
                    }


                    ElementList.Add(ee);
                }
            }
        }

        void ShowTargetState(object activity)
        {
            if (activity is CompositeActivity)
            {
                CompositeActivity wxd = activity as CompositeActivity;

                foreach (object temp in wxd.Activities)
                {
                    ShowTargetState(temp);
                }
            }
            else
            {
                if (activity is SetStateActivity)
                {
                    Activity at = activity as Activity;
                    string s = "";
                    while (true)
                    {
                        at = at.Parent;
                        if (at is StateActivity)
                        {
                            s = at.Name;
                            break;
                        }
                    }

                   SetStateActivity wxd = activity as SetStateActivity;

                   string ly = "";
                   if (wxd.Parent is wxwinter.wf.WFLib.条件分支)
                   {
                       ly = ((wxwinter.wf.WFLib.条件分支)wxd.Parent).条件;
                   }

                    ElementRelationList.Add(new ElementRelation() {Name= wxd.Name, 起点 = s, 目标 = wxd.TargetStateName, 说明 = wxd.Description , 路由=ly});

                }
            }
        }

        void ShowLocation(string layout)
        {
            if(string.IsNullOrEmpty(layout))
            {
                return;
            }
            UTF8Encoding utf8 = new UTF8Encoding();

            byte[] a = utf8.GetBytes(layout);

            MemoryStream m1 = new MemoryStream(a);

            XmlReader r = XmlReader.Create(m1);



            XmlDocument DOC = new XmlDocument();
            DOC.Load(r);

            XmlNodeList s;

            s = DOC.GetElementsByTagName("StateDesigner", "http://schemas.microsoft.com/winfx/2006/xaml/workflow");


            foreach (XmlNode temp in s)
            {
                try
                {
                    XmlElement em = temp as XmlElement;
                    string 名称 = em.GetAttribute("Name");
                    string Location = em.GetAttribute("Location");
                    string[] xy = Location.Split(',');

                    var v = ElementList.Single(p => p.Name == 名称);
                    v.X坐标 = int.Parse(xy[0]);
                    v.Y坐标 = int.Parse(xy[1]);
                }
                catch
                {
                }

            }

        }

        void ShowHardCauda(object activity)
        {
            if (activity is StateMachineWorkflowActivity)
            {
               StateMachineWorkflowActivity sa = activity as StateMachineWorkflowActivity;

                var v1 = ElementList.Single(p => p.Name == sa.InitialStateName);
                v1.类型 = "头";

                var v2 = ElementList.Single(p => p.Name == sa.CompletedStateName);
                v2.类型 = "尾";

                var v3 = ElementList.Single(p => p.Name == "归档状态");
                v3.类型 = "归档";
            }
        }

        public FlowChart GetFlowChart()
        {

            UTF8Encoding ut8 = new UTF8Encoding();
            byte[] bs = ut8.GetBytes(Xoml);

            MemoryStream ms = new MemoryStream(bs);

            XmlReader xmlReader = XmlReader.Create(ms);
            WorkflowMarkupSerializer serializer = new WorkflowMarkupSerializer();
            Activity activity = (Activity)serializer.Deserialize(xmlReader);


            ShowState(activity);
            ShowTargetState(activity);
            ShowLocation(Layout);
            ShowHardCauda(activity);

            FlowChart chart = new FlowChart();

            wxwinter.wf.WFLib.状态机模板 workflow = activity as wxwinter.wf.WFLib.状态机模板;

            if (workflow != null)
            {
                FlowChartData fcd = new FlowChartData();

                fcd.模板版本 = workflow.模板版本;
                fcd.模板编号 = workflow.模板编号;
                fcd.模板名称 = workflow.模板名称;
                fcd.模板说明 = workflow.模板说明;
                fcd.启动时填写的表单 = workflow.启动时填写的表单;
                fcd.数据表单列表 = workflow.数据表单列表;

                chart.FlowData = fcd;
            }


            chart.ElementList= ElementList;
            chart.ElementRelationList = ElementRelationList;
            

    
         
            return chart;
        }


        public T GetFlowObject<T>(FlowChart flowChart)
        {

           

            string lzm = string.Format(Resource.myState, "结束状态", "900", "700");
            
            //-------------------------------------------------------------------------
            wxwinter.wf.WFLib.状态机模板 workflow = new wxwinter.wf.WFLib.状态机模板();
            RuleConditionReference ruleconditionreference1 = new RuleConditionReference();
            ruleconditionreference1.ConditionName = "启用";
            workflow.SetValue(WorkflowChanges.ConditionProperty, ruleconditionreference1);

            FlowChartData fcd = flowChart.FlowData as FlowChartData;

            if (fcd != null)
            {
               workflow.模板版本 = fcd.模板版本 ;
               workflow.模板编号 =fcd.模板编号  ;
               workflow.模板名称 =fcd.模板名称  ;
               workflow.模板说明 =fcd.模板说明  ;
               workflow.启动时填写的表单  =fcd.启动时填写的表单 ;
               workflow.数据表单列表= fcd.数据表单列表  ;
            }


            //-开始结点
            workflow.InitialStateName = "开始状态";

            StateActivity 开始状态 = new StateActivity("开始状态");
            StateInitializationActivity 开始状态自动容器 = new StateInitializationActivity("开始状态自动容器");
            wxwinter.wf.WFLib.初始化 开始启动流程 = new wxwinter.wf.WFLib.初始化();
            开始启动流程.Name = "开始启动流程";

            workflow.Activities.Add(开始状态);
            开始状态.Activities.Add(开始状态自动容器);
            开始状态自动容器.Activities.Add(开始启动流程);


   
            ////////////////
            foreach (var node in flowChart.ElementList)
            {
                lzm = lzm + string.Format(Resource.myState, node.Name, node.X坐标, node.Y坐标);

                if (node.类型 == "头")
                {
                    
                        var r = flowChart.ElementRelationList.FirstOrDefault(p => p.起点 == node.Name && p.路由 == "开始状态");

                        if (r != null)
                        {
                            SetStateActivity 启动流程 = new SetStateActivity("启动流程");
                            启动流程.TargetStateName = r.目标;
                            开始状态自动容器.Activities.Add(启动流程);
                        }
                   
                }
           }
            //----------------------------------------------------------------

            //-完成结点

            StateActivity 结束状态 = new StateActivity("结束状态");
            workflow.Activities.Add(结束状态);
            workflow.CompletedStateName = "结束状态";


            StateActivity 归档状态 = new StateActivity("归档状态");
            StateInitializationActivity 归档状态自动容器 = new StateInitializationActivity("归档状态自动容器");
            wxwinter.wf.WFLib.归档 流程归档 = new wxwinter.wf.WFLib.归档();
            流程归档.Name = "流程归档";

            workflow.Activities.Add(归档状态);
            归档状态.Activities.Add(归档状态自动容器);
            归档状态自动容器.Activities.Add(流程归档);

            SetStateActivity 完成流程 = new SetStateActivity("完成流程");
            完成流程.TargetStateName = "结束状态";
            归档状态自动容器.Activities.Add(完成流程);
            
            
            
            //


            WindowsWorkflowObject wf = new WindowsWorkflowObject();

            foreach (var node in flowChart.ElementList)
            {
    


                if (node.类型 == "节")
                {

                    string n = node.Name;


                    System.Workflow.Activities.StateActivity state = new System.Workflow.Activities.StateActivity();
                    state.Name = n;

                    System.Workflow.Activities.EventDrivenActivity eda = new System.Workflow.Activities.EventDrivenActivity();
                    eda.Name = n + "_等待容器";

                    wxwinter.wf.WFLib.等待提交 waitsubmit = new wxwinter.wf.WFLib.等待提交();
                    waitsubmit.Name = n + "_处理中";
                    waitsubmit.状态名称 = n + "_处理中";

                    WaitControlData wd = node.结点数据 as WaitControlData;

               
                   waitsubmit.办理查看业务表单= wd.办理查看业务表单  ;
                    waitsubmit.办理人员= wd.办理人员 ;
                 
                   waitsubmit.办理时限= wd.办理时限  ;
                   waitsubmit.办理提交选项= wd.办理提交选项  ;
                   waitsubmit.办理添写业务表单 = wd.办理添写业务表单 ;
                   waitsubmit.处理方式 =wd.处理方式  ;
                   waitsubmit.接件部门 =wd.接件部门  ;
                   waitsubmit.接件职能 =wd.接件职能  ;
                    waitsubmit.启动窗体 =wd.启动窗体 ;

                    waitsubmit.Description = wd.说明;










                    wxwinter.wf.WFLib.条件容器 ir = new wxwinter.wf.WFLib.条件容器();
                    ir.Name = n + "_处理分支";

                    string ts = "";
                    foreach (var v in node.分支集合)
                    {
                        wxwinter.wf.WFLib.条件分支 fz = new wxwinter.wf.WFLib.条件分支();
                        fz.Name = n + "_处理分支_" + v.ToString();
                        fz.条件 = v.ToString();
                        ir.Activities.Add(fz);
                        ts = ts + v.ToString() + ",";



                        var r = flowChart.ElementRelationList.FirstOrDefault(p => p.起点 == node.Name && p.路由 == v);

                        if (r == null)
                        {

                            wf = null;
                            T tp = (T)((object)wf);

                            return (T)tp;
                        }
                        else
                        {
                            SetStateActivity tp = new SetStateActivity(node.Name +"_到_" + r.目标);
                            tp.TargetStateName = r.目标;
                            fz.Activities.Add(tp);

                        }


                    }
                    if (ts != "")
                    {
                        waitsubmit.办理提交选项 = ts.Remove(ts.Length - 1);
                    }


                    eda.Activities.Add(waitsubmit);
                    eda.Activities.Add(ir);
                    state.Activities.Add(eda);

                    workflow.Activities.Add(state);

                }


                

           }

            string q = WorkflowClassToXomlString(workflow);

            wf.Xoml = q.Replace("utf-16", "utf-8");
            wf.Rules = Resource.myRule;

            wf.Layout = string.Format(Resource.myLayout, lzm);





            T t = (T)((object)wf);

            return (T)t;
        }



        public string WorkflowClassToXomlString(Activity workflow)
        {
            WorkflowMarkupSerializer wfSerializer = new WorkflowMarkupSerializer();
            DesignerSerializationManager sm = new DesignerSerializationManager();
            sm.CreateSession();

            System.Text.StringBuilder s = new StringBuilder();
            XmlWriter xmlwriter = XmlWriter.Create(s);

            wfSerializer.Serialize(sm, xmlwriter, workflow);
            if (sm.Errors.Count > 0)
            {
                throw new Exception("出现错误:" + sm.Errors.Count.ToString());
            }
            return s.ToString();
        }




    }


  public class WindowsWorkflowObject
  {
      public string Xoml
      { set; get; }

      public string Rules
      { set; get; }

      public string Layout
      { set; get; }

   
  }
}
