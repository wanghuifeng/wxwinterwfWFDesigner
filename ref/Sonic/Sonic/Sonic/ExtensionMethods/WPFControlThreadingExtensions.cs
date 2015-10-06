using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;

namespace Sonic
{
    public static class WPFControlThreadingExtensions
    {
        public static void InvokeIfRequired(this Dispatcher disp, 
            Action dotIt, DispatcherPriority priority)
        {
            if (disp.Thread != Thread.CurrentThread)
            {
                disp.Invoke(priority, dotIt);
            }
            else
                dotIt();
        }
    }
}
