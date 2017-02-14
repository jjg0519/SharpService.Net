using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;



namespace TheService.Utilities
{

    public class FileHelper
    {
        public static string GetAbsolutePath(string relativePath)
        {
            string directory = string.Empty;
            return GetAbsolutePath(relativePath, ref directory);
        }

        public static string GetAbsolutePath(string relativePath, ref string directory)
        {
            if (string.IsNullOrEmpty(relativePath))
            {
                throw new ArgumentNullException("参数relativePath空异常！");
            }
            relativePath = relativePath.Replace("/", "\\");
            if (relativePath[0] == '\\')
            {
                relativePath = relativePath.Remove(0, 1);
            }
            //判断是Web程序还是window程序
            if (HttpContext.Current != null)
            {
                directory = HttpRuntime.AppDomainAppPath;
                return Path.Combine(HttpRuntime.AppDomainAppPath, relativePath);
            }
            else
            {
                directory = AppDomain.CurrentDomain.BaseDirectory;
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
            }
        }
    }
}