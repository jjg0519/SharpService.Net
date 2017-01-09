using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheService.Common
{
    public class HttpHelper
    {
        public static Uri GetUri(string strUrl)
        {
            try
            {
                return new Uri(strUrl);
            }
            catch (Exception)
            {
                throw new Exception($"{strUrl}非有效地址");
            }
        }
    }
}
