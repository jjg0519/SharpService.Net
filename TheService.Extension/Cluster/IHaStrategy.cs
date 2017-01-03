using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheService.Extension.Cluster
{
    public interface IHAStrategy
    {
        Response Call(Request request, ILoadBalance loadBalance);
    }
}
