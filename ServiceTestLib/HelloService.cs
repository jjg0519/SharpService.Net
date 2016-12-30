using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Web;
using TheService.Extension.Service;

namespace ServiceTestLib
{
    public class HelloService : IHelloService
    {

        public string GetMessage(string name)
        {
            var msg = ServiceUtils.GetHeaderValue("test");
            return "Hello " + name + msg;
        }
    }
}
