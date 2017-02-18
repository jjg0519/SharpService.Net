using SharpService.Components;

namespace SharpService.Requester
{
    public class RequestClient
    {
        private IRequesterHandler requesterHandler { set; get; }
        private string id { set; get; }

        public RequestClient UseId(string id)
        {
            this.id = id;
            return this;
        }

        public RequestClient UseRequesterHandler<Handler>()
        {
            requesterHandler = ObjectContainer.ResolveNamed<IRequesterHandler>(typeof(Handler).FullName);
            return this;
        }

        public Interface BuilderClient<Interface>()
        {
            return (Interface)new RequestRealProxy<Interface>(id, requesterHandler).GetTransparentProxy();
        }
    }
}
