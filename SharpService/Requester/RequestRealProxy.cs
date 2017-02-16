using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace SharpService.Requester
{
    public class RequestRealProxy<Interface> : RealProxy
    {
        private IRequesterHandler requesterHandler { set; get; }
        private string id { set; get; }

        public RequestRealProxy(string id, IRequesterHandler requesterHandler) : base(typeof(Interface))
        {
            this.id = id;
            this.requesterHandler = requesterHandler;
        }

        public override IMessage Invoke(IMessage msg)
        {
            return requesterHandler.Invoke<Interface>(msg, id);
        }
    }
}
