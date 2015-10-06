using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;

namespace Wxwinter.BPM.ActivitySample.会签
{
    [System.ComponentModel.Designer(typeof(parallelItemDesigner))]
    public sealed class parallelItem : CodeActivity
    {

        public InArgument<string> userName { get; set; }


        protected override void Execute(CodeActivityContext context)
        {

            string text = context.GetValue(this.userName);

            System.Console.WriteLine(text);
        }
    }
}
