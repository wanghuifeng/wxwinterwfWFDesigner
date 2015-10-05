using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wxwinter.wf.WFDesigner
{
   public  class  XomlLoder : ILoder
    {
      
       public void LoadFlow<T>(T obj, IDesigner designer)
       {
           designer.Clear(false);
           WindowsWorkflowObject wf = obj as WindowsWorkflowObject;

           FlowChart flowChart = (new XomlConverter(wf.Xoml, wf.Rules, wf.Layout)).GetFlowChart();

           designer.流程数据 = flowChart.FlowData;
           foreach (var v in flowChart.ElementList)
           {

               if (v.类型 == "节")
               {
                   
                   ActivityControl q = new WaitControl() {   Name = v.Name, X坐标 = v.X坐标, Y坐标 = v.Y坐标, 标题 = v.Name, 说明 = ((WaitControlData)v.结点数据).说明, 类型 = v.类型, 分支集合 = v.分支集合, 结点数据=v.结点数据 };
                   designer.AddActivity(q);
               }
               if (v.类型 == "头")
               {
                   ActivityControl q = new BeginControl() { Name = v.Name, X坐标 = v.X坐标, Y坐标 = v.Y坐标, 类型 = v.类型, 分支集合 = v.分支集合 };
                   designer.AddActivity(q);
               }
               if (v.类型 == "归档")
               {

                   ActivityControl q = new EndControl() { Name = v.Name };
                   designer.AddActivity(q);
               }
           }

           foreach (var v in flowChart.ElementRelationList)
           {
               if (v.Name == "启动流程")
               {
                   v.路由 = "开始状态";
               }
               designer.AddActivityPath(v.Name, v.起点, v.目标, v.路由, v.说明);

           }
       }


       public T GetFlow<T>(IDesigner designer)
       {
           FlowChart flowChart=new FlowChart();
           flowChart.ElementList = new List<Element>();
           foreach (var v in designer.GetActivityControlList())
           {
               flowChart.ElementList.Add(new Element(){ Name=v.Name, X坐标=v.X坐标,Y坐标=v.Y坐标 , 分支集合 =v.分支集合, 结点数据=v.结点数据, 类型=v.类型, 说明=v.说明});

           }

           flowChart.ElementRelationList = new List<ElementRelation>();
           foreach (var r in designer.GetActivityPathList())
           {
               flowChart.ElementRelationList.Add(new ElementRelation() { Name = r.Name, 路由 = r.路由, 目标 = r.目标, 起点 = r.起点, 说明 = r.说明, });
           }

           flowChart.FlowData = designer.流程数据;
           //-

           WindowsWorkflowObject wf = (new XomlConverter()).GetFlowObject<WindowsWorkflowObject>(flowChart);

           T t = (T)((object)wf);

           return (T)t;
       }
   
   }

 
}
