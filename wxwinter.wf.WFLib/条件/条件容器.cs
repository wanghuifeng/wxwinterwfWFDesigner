using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.ComponentModel;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace wxwinter.wf.WFLib
{

    [Designer(typeof(条件容器设计器), typeof(IDesigner))]
    public class 条件容器 : CompositeActivity, IActivityEventListener<ActivityExecutionStatusChangedEventArgs>
    {
        public 条件容器()
        {
            InitializeComponent();
        }
        [System.Diagnostics.DebuggerNonUserCode]
        private void InitializeComponent()
        {
            this.CanModifyActivities = true;
            this.Name = "条件容器";
            this.CanModifyActivities = false;
        }

        public static readonly DependencyProperty 分支表达式Property = DependencyProperty.Register("分支表达式", typeof(string), typeof(条件容器), new PropertyMetadata(""));

        public string 分支表达式
        {
            get { return (string)GetValue(分支表达式Property); }
            set { SetValue(分支表达式Property, value); }
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            foreach (条件分支 branch in EnabledActivities)
            {

                if (string.IsNullOrEmpty(this.分支表达式))
                {
                    if (branch.条件 == Root().当前节点提交结果)
                    {
                        branch.RegisterForStatusChange(Activity.ClosedEvent, this);
                        executionContext.ExecuteActivity(branch);
                        return ActivityExecutionStatus.Executing;
                    }
                }
                else
                {
                    if (branch.条件 == this.分支表达式)
                    {
                        branch.RegisterForStatusChange(Activity.ClosedEvent, this);
                        executionContext.ExecuteActivity(branch);
                        return ActivityExecutionStatus.Executing;
                    }
                }
            }

            foreach (条件分支 branch in EnabledActivities)
            {

               
                    if (branch.条件 == "else")
                    {
                        branch.RegisterForStatusChange(Activity.ClosedEvent, this);
                        executionContext.ExecuteActivity(branch);
                        return ActivityExecutionStatus.Executing;
                    }
               
            }
            return ActivityExecutionStatus.Executing;

            
        }

        public void OnEvent(object sender, ActivityExecutionStatusChangedEventArgs e)
        {
            ActivityExecutionContext aec = sender as ActivityExecutionContext;
            条件分支 branch = e.Activity as 条件分支;
            if (branch != null)
            {
                branch.UnregisterForStatusChange(Activity.ClosedEvent, this);
            }

            foreach (Activity child in this.EnabledActivities)
            {
                if (child.ExecutionStatus != ActivityExecutionStatus.Closed
                    && child.ExecutionStatus != ActivityExecutionStatus.Initialized)
                {
                    return;
                }
            }
            aec.CloseActivity();
        }



        ITemplate Root()
        {
            Activity o = this.Parent;

            while (o.Parent != null)
            {
                o = o.Parent;
            }
            ITemplate tp = o as ITemplate;

            if (tp != null)
            {
                return tp;
            }
            else
            {
                throw new System.Exception("GetRoot");
            }

        }


    }

}
