using System;
using System.IO;
using System.Web;

namespace NService.Utilities
{

    public class FileUtil
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
                throw new ArgumentNullException("����relativePath���쳣��");
            }
            relativePath = relativePath.Replace("/", "\\");
            if (relativePath[0] == '\\')
            {
                relativePath = relativePath.Remove(0, 1);
            }
            //�ж���Web������window����
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