using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SnookerCore
{
    /// <summary>
    /// Enumerates the different sounds that can be played by the client application.
    /// Notice that, since it's a DataContract enumeration, you have to apply  to all members.
    /// </summary>
    [Serializable]
    public enum GameSound
    {
        None,
        Bank01,
        Bank02,
        Click01,
        Click02,
        Fall,
        Hit01,
        Hit02,
        Hit03,
        Hit04,
        Hit05,
        Hit06,
        Shot01,
        Jungle
    }
}
