using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sonic
{
    public class Globals
    {
        public static List<String> MusicLocations = new List<string>();
        public static String AppLocation = String.Empty;
        public readonly static String LibraryFile = "Library.xml";
        public static Boolean ReReadAllFiles = false;
    }
}
