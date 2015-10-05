using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Activities;
using System.Workflow.ComponentModel;
using System.Collections.ObjectModel;

namespace wxwinter.wf.WFLib
{
    public class 条件容器设计器 :ParallelActivityDesigner
    {
        protected override System.Workflow.ComponentModel.CompositeActivity OnCreateNewBranch()
        {
            return new 条件分支();
            
           
        }

        public override bool CanMoveActivities(HitTestInfo moveLocation, ReadOnlyCollection<Activity> activitiesToMove)
        {
             return true;
        }

        public override bool CanRemoveActivities(ReadOnlyCollection<Activity> activitiesToRemove)
        {
            return true ;
        }
 
       

    }

}
