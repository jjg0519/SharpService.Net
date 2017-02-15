using System;
using ProtoBuf;
using System.Threading;

namespace ServiceTestLib
{
    public class HelloService : IHelloService
    {
        public string SendMessage(string message)
        {
            return message;
        }

        public Data SendData(Data data)
        {
            return data;
        }

        public void Error()
        {
            throw new Exception("Error");
        }

        public void OK() { }

        public void TimeOut()
        {
            Thread.Sleep(TimeSpan.FromMinutes(1.5));
        }
    }

    [ProtoContract]
    public class Data
    {
        [ProtoMember(1)]
        public string Name { set; get; }
    }
}
