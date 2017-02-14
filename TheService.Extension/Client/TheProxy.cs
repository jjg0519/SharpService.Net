using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheService.Extension.Requester;

namespace TheService.Extension.Client
{
    public class TheProxy
    {
        public static Interface TheRequestProxy<Interface>(string id)
        {
            return (Interface)new RequestRealProxy<Interface>(id).GetTransparentProxy();
        }
    }
}
