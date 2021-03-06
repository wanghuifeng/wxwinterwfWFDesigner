﻿using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

using System.Collections.Generic;


namespace wxwinter.wf.WFLib
{
	public partial class 初始化:Activity
	{
    
        状态机模板 Root()
        {
            Activity o = this.Parent;

            while (o.Parent != null)
            {
                o = o.Parent;
            }
            状态机模板 tp = o as 状态机模板;

            if (tp != null)
            {
                return tp;
            }
            else
            {
                throw new System.Exception("GetRoot");
            }

        }

   
        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            
                List<string> ls = executionContext.GetService(typeof(List<string>)) as List<string>;

                if (ls != null)
                {

                    ls.Add("--------------------------初始化-----------------------------------");
                    ls.Add("流程编号:" + Root().流程编号.ToString());
                    ls.Add("启动部门" + Root().启动部门);
                    ls.Add("启动职能" + Root().启动职能);
                    ls.Add("启动人员" + Root().启动人员);
                    ls.Add("启动时间" + Root().启动时间.ToString());
                    ls.Add("");
                }
            
    
            return base.Execute(executionContext);
        }
	}
}
