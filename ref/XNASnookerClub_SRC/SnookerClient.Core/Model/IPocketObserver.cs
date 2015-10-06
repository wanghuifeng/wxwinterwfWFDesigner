using System;
using System.Collections.Generic;
using System.Text;

namespace Snooker.Client.Core.Model
{
    public interface IPocketObserver
    {
        void BallDropped(Ball ball);
    }
}
