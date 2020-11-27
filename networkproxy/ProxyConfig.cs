using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hu.czompisoftware.libraries.networkproxy
{

    public class ProxyConfig
    {
        public String Address { get; set; }
        public Int32 Port { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
        public bool BypassProxyOnLocal { get; set; }
    }
}