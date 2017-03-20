using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace SharpService.ServiceRequester
{
    public class RequestRealProxy<Interface> : RealProxy
    {
        private IDelegatingHandler delegatingHandler { set; get; }
        private string id { set; get; }

        public RequestRealProxy(string id, IDelegatingHandler delegatingHandler) : base(typeof(Interface))
        {
            this.id = id;
            this.delegatingHandler = delegatingHandler;
        }

        public override IMessage Invoke(IMessage msg)
        {
            return delegatingHandler.Handler<Interface>(msg, id);
        }
    }
}
