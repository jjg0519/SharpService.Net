using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheService.Extension.Consumer
{
    public class TheProxy
    {
        public static IServiceProxy TheServiceProxy<IServiceProxy>(string id)
        {
            return (IServiceProxy)new ServiceRealProxy<IServiceProxy>(id).GetTransparentProxy();
        }
    }
}
