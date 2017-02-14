using System;

namespace TheService.Utilities
{
    public class HttpUtil
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
