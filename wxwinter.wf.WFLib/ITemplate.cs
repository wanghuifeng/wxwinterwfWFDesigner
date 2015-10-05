using System;
namespace wxwinter.wf.WFLib
{
   public interface ITemplate
    {
   
        int 测试编号 { get; set; }
      
        string 当前节点提交结果 { get; set; }
        string 当前节点提交人员 { get; set; }
       
        bool 回归主流程 { get; set; }
   
        string 扩展数据 { get; set; }
        Guid 流程编号 { get; set; }
        string 流程回归组 { get; set; }
        string 模板编号 { get; set; }
        string 模板类型 { get;  }
        string 模板名称 { get; set; }
        string 模板说明 { get; set; }
        string 启动部门 { get; set; }
        string 启动窗体 { get; set; }
        string 启动人员 { get; set; }

        DateTime 启动时间 { get; set; }
        string 启动时填写的表单 { get; set; }
        string 启动职能 { get; set; }

       
        string 数据表单列表 { get; set; }
        string 业务保密度 { get; set; }
        string 业务编号 { get; set; }
        string 业务紧急度 { get; set; }
        string 业务类型 { get; set; }
        string 业务描述 { get; set; }
        string 业务名称 { get; set; }
        Guid 主流程编号 { get; set; }
        string 主业务编号 { get; set; }
        string 状态编号 { get; set; }
        int 状态跟踪器 { get; set; }
        string 模板版本 { get; set; }

    }
}
