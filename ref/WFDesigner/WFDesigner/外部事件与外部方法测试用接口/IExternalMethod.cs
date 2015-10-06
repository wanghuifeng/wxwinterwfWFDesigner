using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImyExternal
{
    [System.Workflow.Activities.ExternalDataExchange()]
	public interface IExternalMethod
	{
        string myExternalMethod1(Guid guid, object message);
        string myExternalMethod2(Guid guid, object message);
	}
}
