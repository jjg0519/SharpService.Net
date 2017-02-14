using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NService.Core.ConfigFactory;

namespace NService.Core.ConfigFactory
{
    public interface ILoadBalance
    {
        //void OnRefresh(RefererElement referer);

        Referer Select(Request request);

        void SelectToHolder(Request request, RefererElement referHolder);

        void SetWeightString(string weightString);
    }
}
