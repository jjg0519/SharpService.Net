using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheService.Extension.Cluster;
using TheService.Extension.ConfigFactory;

namespace TheService.Extension.LoadBalance
{
    public class RandomLoadBalance : AbstractLoadBalance
    {
        public override Referer DoSelect(Request request)
        {
            RefererElement referer = _referer;
            int idx = (int)(new Random().Next() * referer.Addresss.Count());
            int i = (int)(new Random().Next(0, referer.Addresss.Count()));
            return new Referer()
            {
                Interface = referer.Interface,
                Assembly = referer.Assembly,
                Binding = referer.Binding,
                Security = referer.Security,
                Address = referer.Addresss[(i + idx) % referer.Addresss.Count()]
            };
        }

    }
}
