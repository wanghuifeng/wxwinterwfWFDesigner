using System;
namespace wxwinter.wf.WFDesigner
{
   public  interface IConverter
    {
       T GetFlowObject<T>(FlowChart flowChart);
       FlowChart GetFlowChart();
    }
}
