using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImyExternal
{
    [System.Workflow.Activities.ExternalDataExchange()]
	public interface IExternalEvent
	{
        event System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs> myExternalEvent1;
        event System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs> myExternalEvent2;
	}
}
