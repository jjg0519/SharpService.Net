using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace NService.Requester
{
    public class RequestRealProxy<T> : RealProxy
    {
        private IRequesterHandler requesterHandler { set; get; }
        private string id { set; get; }

        public RequestRealProxy(string id, IRequesterHandler requesterHandler) : base(typeof(T))
        {
            this.id = id;
            this.requesterHandler = requesterHandler;
        }

        public override IMessage Invoke(IMessage msg)
        {
            return requesterHandler.Invoke<T>(msg, id);
        }
    }
}
