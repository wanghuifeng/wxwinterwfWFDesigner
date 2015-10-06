using System;
using System.Collections.Generic;
using System.Text;
using SnookerCore;

namespace Snooker.Client.Core.Model
{
    public interface IBallObserver
    {
        void Hit(GameSound sound);
    }
}
