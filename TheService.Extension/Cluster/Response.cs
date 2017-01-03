using System;
using System.Runtime.Remoting.Messaging;

namespace TheService.Extension.Cluster
{
    public class Response
    {
        public IMessage resMsg { set; get; }

        public Exception exception { set; get; }

        /// <summary>
        /// 业务处理时间
        /// </summary>
        /// <returns></returns>
        public long ProcessTime { set; get; }
    }
}