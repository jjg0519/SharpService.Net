using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NService.Configuration
{
    public class Referer
    {  
        public string Interface { get; set; }

        public string Assembly { get; set; }

        public string Binding { get; set; }

        public int Security { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public string Address { get; set; }
    }
}
