using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheService.Extension.ConfigFactory;

namespace TheService.Extension.LoadBalance
{
    public abstract class AbstractLoadBalance : ILoadBalance
    {
        protected RefererElement _referer;

        public virtual void OnRefresh(RefererElement referer)
        {
            _referer = referer;
        }

        public virtual Referer Select(Request request)
        {
            //RefererElement referer = _referer;
            //Referer _ref = null;
            //if (referer.Addresss.Count() > 1)
            //{
            //    _ref = DoSelect(request);
            //}
            //else if (referer.Addresss.Count() == 1)
            //{
            //    _ref = new Referer()
            //    {
            //        Interface = referer.Interface,
            //        Assembly = referer.Assembly,
            //        Binding = referer.Binding,
            //        Security = referer.Security,
            //        Address = referer.Addresss[0]
            //    };
            //}
            //if (_ref != null)
            //{
            //    return _ref;
            //}
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
