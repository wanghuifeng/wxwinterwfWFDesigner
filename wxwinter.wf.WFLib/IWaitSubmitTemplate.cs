using System;
namespace wxwinter.wf.WFLib
{
  public  interface IWaitSubmitTemplate
    {
        string 办理查看业务表单 { get; set; }
  
        string 办理人员 { get; set; }

        int 办理时限 { get; set; }
        string 办理提交选项 { get; set; }
        string 办理添写业务表单 { get; set; }

        string 处理方式 { get; set; }
        string 触发器类型 { get; set; }


        string 接件部门 { get; set; }


        string 接件职能 { get; set; }

        string 启动窗体 { get; set; }


        string 提交部门 { get; set; }
        int 提交次数 { get; set; }
        string 提交方式 { get; set; }
        string 提交结果 { get; set; }
        string 提交人员 { get; set; }
        DateTime 提交日期 { get; set; }
        string 提交说明 { get; set; }
        string 提交职能 { get; set; }


        string 状态名称 { get; set; }

        string 状态说明 { get; set; }
    }
}
