using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheService.Extension.ConfigFactory;

namespace TheService.Extension.ConfigFactory
{
    public interface ILoadBalance
    {
        //void OnRefresh(RefererElement referer);

        Referer Select(Request request);

        void SelectToHolder(Request request, RefererElement referHolder);

        void SetWeightString(string weightString);
    }
}
