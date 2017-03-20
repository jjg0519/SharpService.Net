using SharpService.Components;

namespace SharpService.ServiceRequester
{
    public class RequestClient
    {
        private IDelegatingHandler delegatingHandler { set; get; }
        private string id { set; get; }

        public RequestClient UseId(string id)
        {
            this.id = id;
            return this;
        }

        public RequestClient UseDelegatingHandler<Handler>()
        {
            delegatingHandler = ObjectContainer.ResolveNamed<IDelegatingHandler>(typeof(Handler).FullName);
            return this;
        }

        public Interface BuilderClient<Interface>()
        {
            return (Interface)new RequestRealProxy<Interface>(id, delegatingHandler).GetTransparentProxy();
        }
    }
}
