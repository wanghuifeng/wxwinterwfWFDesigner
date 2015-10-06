using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HundredMilesSoftware.UltraID3Lib;

namespace Sonic
{

    public class MP3s
    {
        private IQueryProvider provider;

        public IQueryable<MP3> Files;

        public MP3s() 
        {
            this.provider = new MP3Provider();
            this.Files = new Query<MP3>(this.provider);
        }


        public IQueryProvider Provider
        {
            get { return this.provider; }
        }
    }
}
