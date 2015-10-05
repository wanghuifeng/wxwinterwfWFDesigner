using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wxwinter.wf.WFLib
{
    [System.Workflow.Activities.ExternalDataExchange()]
	public interface IExternalEvent
	{
        event System.EventHandler<SubmitEventArgs> submitEvent;
	}

    public class ExternalEvent : IExternalEvent
    {
        //事件
          public event EventHandler<SubmitEventArgs> submitEvent;


        //触发事件的方法
          public void submitResult(Guid guid, Guid 状态实例编号, string 提交结果, string 提交人员, string 提交部门, string 提交职能, string 提交方式, string 提交说明, string 触发器类型, string 下一状态办理人员, string 数据表单) 
        {

            SubmitEventArgs e = new SubmitEventArgs(guid);

            e.状态实例编号 = 状态实例编号;
            e.提交结果 = 提交结果;
            e.提交人员 = 提交人员;
            e.提交部门 = 提交部门;
            e.提交职能 = 提交职能;
            e.提交方式 = 提交方式;
            e.提交说明 = 提交说明;
            e.触发器类型 = 触发器类型;
            e.提交说明 = 提交说明;
            e.提交日期 = System.DateTime.Now;

            e.下一状态办理人员 = 下一状态办理人员;
            e.数据表单 = 数据表单;


            submitEvent(null, e);

        }


    }
}
