using System;
using System.Collections.Generic;
using System.Text;

namespace Snooker.Client.Core.Model
{
    public interface IBorderObserver
    {
        void Hit(string sound);
    }
}
