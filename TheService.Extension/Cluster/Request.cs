using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

namespace TheService.Extension.Cluster
{
    public class Request
    {
        public string id { set; get; }

        public Dictionary<string, string> messages { set; get; }

        public IMessage reqMsg { set; get; }
    }
}