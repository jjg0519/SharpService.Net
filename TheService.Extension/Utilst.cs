using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheService.Extension
{
    public class Utility
    {
        public static void UrlCheck(string strUrl)
        {
            try
            {
                new Uri(strUrl);
            }
            catch (Exception)
            {
                throw new Exception($"{strUrl}非有效地址");
            }
        }
    }
}
