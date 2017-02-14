using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NService.Core.ConfigFactory
{
    public interface IHAStrategy
    {
        Response Call(Request request, ILoadBalance loadBalance);
    }
}
