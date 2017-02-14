using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NService.Core.Requester;

namespace NService.Core.Client
{
    public class TheProxy
    {
        public static Interface TheRequestProxy<Interface>(string id)
        {
            return (Interface)new RequestRealProxy<Interface>(id).GetTransparentProxy();
        }
    }
}
