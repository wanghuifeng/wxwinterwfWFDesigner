using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Workflow.Runtime;
using System.Workflow.Runtime.Hosting;
using System.Xml;


namespace wxwinter.wf.WFDesigner
{
    public class FlowChart
    {
        public List<Element> ElementList = new List<Element>();

        public List<ElementRelation> ElementRelationList = new List<ElementRelation>();

        public FlowChartData FlowData
        { set; get; }

      }

    public class Element
    {
        public string Name
        { set; get; }

        public string 类型
        { set; get; }

        public string 说明
        { set; get; }
       
        public Double X坐标
        { set; get; }

        public Double Y坐标
        { set; get; }

        public object 结点数据
        { set; get; }

        public List<string> 分支集合
        { set; get; }
    }

    public class ElementRelation
    {
        public string Name
        { set; get; }

        public string 起点
        { set; get; }

        public string 目标
        { set; get; }

        public string 路由
        { set; get; }

        public string 说明
        { set; get; }

    }
}
