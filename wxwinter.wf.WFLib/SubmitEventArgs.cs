using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace wxwinter.wf.WFLib
{
    [Serializable()]
   public class SubmitEventArgs : System.Workflow.Activities.ExternalDataEventArgs
	{
       public SubmitEventArgs(Guid instanceId)
            : base(instanceId)
        { }

       //
       [DescriptionAttribute("该结点的状态实例编号")]
       [CategoryAttribute("用户触发时传入")]
       [BrowsableAttribute(true)]
       public Guid 状态实例编号
       { set; get; }


       //[10.1]
       [DescriptionAttribute("[办理提交选项]例表中的一项")]
       [CategoryAttribute("用户触发时传入")]
       [BrowsableAttribute(true)]
       public string 提交结果
       { set; get; }

       //[10.2]

       [DescriptionAttribute("进行操作的用户")]
       [CategoryAttribute("用户触发时传入")]
       [BrowsableAttribute(true)]
       public string 提交人员
       { set; get; }

       //[10.3]
       [DescriptionAttribute("进行操作的部门")]
       [CategoryAttribute("用户触发时传入")]
       [BrowsableAttribute(true)]
       public string 提交部门
       { set; get; }

       //[10.4]
       [DescriptionAttribute("进行操作的职能")]
       [CategoryAttribute("用户触发时传入")]
       [BrowsableAttribute(true)]
       public string 提交职能
       { set; get; }

       //[10.5]
       [DescriptionAttribute("进行操作的日期")]
       [CategoryAttribute("用户触发时传入")]
       [BrowsableAttribute(true)]
       public DateTime 提交日期
       { set; get; }
       
       
       
       //[10.6]
       [DescriptionAttribute("个人|部门|职能|部门职能")]
       [CategoryAttribute("用户触发时传入")]
       [BrowsableAttribute(true)]
       public string 提交方式
       { set; get; }



       //[10.7]

       [DescriptionAttribute("进行操作的备注说明")]
       [CategoryAttribute("用户触发时传入")]
       [BrowsableAttribute(true)]
       public string 提交说明
       { set; get; }


       //[10.8]
       [DescriptionAttribute("进行操作的触发器")]
       [CategoryAttribute("用户触发时传入")]
       [BrowsableAttribute(true)]
       public string 触发器类型
       { set; get; }

       //[10.9]
       [DescriptionAttribute("进行下一结点操作的用户")]
       [CategoryAttribute("用户触发时传入")]
       [BrowsableAttribute(true)]
       public string 下一状态办理人员
       { set; get; }

       //[10.10]

       [DescriptionAttribute("留做扩展")]
       [CategoryAttribute("用户触发时传入")]
       [BrowsableAttribute(true)]
       public string 数据表单
       { set; get; }

       //[10.]




	}

}
