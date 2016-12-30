using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace TheService.Extension.Service
{
    public class ServiceUtils
    {
        public static string GetHeaderValue(string key)
        {
            int index = OperationContext.Current.IncomingMessageHeaders.FindHeader(key, "http://tempuri.org");
            if (index >= 0)
            {
                return OperationContext.Current.IncomingMessageHeaders.GetHeader<string>(index).ToString();
            }
            return null;
        }
    }
}
