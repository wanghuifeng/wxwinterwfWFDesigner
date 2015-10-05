using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Activities;
using System.Workflow.ComponentModel;
using System.ComponentModel;

namespace wxwinter.wf.WFLib
{

    public class 条件分支 : SequenceActivity
    {


        public static DependencyProperty 条件Property = DependencyProperty.Register("条件", typeof(string), typeof(条件分支), new PropertyMetadata(""));

        public string 条件
        {
            get
            {
                return ((string)(base.GetValue(条件分支.条件Property)));
            }
            set
            {
                base.SetValue(条件分支.条件Property, value);
            }
        }

    }

}
