using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheService.Extension.ConfigFactory;

namespace TheService.Extension.LoadBalance
{
    public abstract class AbstractLoadBalance : ILoadBalance
    {
        protected List<Referer> _referers;

        public virtual void OnRefresh()
        {

        }

        public virtual Referer Select(Request request)
        { 
            throw new Exception(string.Format("No available referers for call request:{0}", request.id));
        }

        public abstract Referer DoSelect(Request request);
   

        public void SelectToHolder(Request request, RefererElement referHolder)
        {
            throw new NotImplementedException();
        }

        public void SetWeightString(string weightString)
        {
            throw new NotImplementedException();
        }
    }
}
