using System;
using System.Collections.Generic;
namespace wxwinter.wf.WFDesigner
{
   public interface IActivity
    {
        double X坐标 { get; set; }
        double Y坐标 { get; set; }
        string 类型 { get; set; }
        string 标题 { get; set; }
        List<string> 分支集合 { get; set; }
        void 删除();
        IDesigner 设计器 { get; set; }
        void 设为活动();
        string 说明 { get; set; }

       object 结点数据 { set; get; }
    }
}
