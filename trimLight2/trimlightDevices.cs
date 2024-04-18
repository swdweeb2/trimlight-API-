using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trimLight2
{
    public class deviceInfo
    {

        public string deviceId { get; set; }
        public string name { get; set; }
        public int connectivity { get; set; }
        public int state { get; set; }
        public string fwVersionName { get; set; }
    }

    public class Payload
    {
        public int total { get; set; }
        public int current { get; set; }
        public IList<deviceInfo> data { get; set; }
    }

    public class trimlightDevices
{
        public int code { get; set; }
        public string desc { get; set; }
        public Payload payload { get; set; }
    }
}
