using System;

namespace NService.Utilities
{
    public class HttpUtil
    {
        public static Uri GetUri(string strUrl)
        {
            try
            {
                return new Uri(strUrl);
            }
            catch
            {
                throw new Exception($"{strUrl}非有效地址");
            }
        }
    }
}
