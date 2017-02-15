namespace NService.Requester
{
    public class RequestProxy
    {
        private IRequesterHandler requesterHandler { set; get; } = new DelegatingHandler();
        private string id { set; get; }

        public RequestProxy UseId(string id)
        {
            this.id = id;
            return this;
        }

        public RequestProxy ConfigurerRequester(IRequesterHandler requesterHandler)
        {
            this.requesterHandler = requesterHandler;
            return this;
        }

        public Interface BuilderClient<Interface>()
        {
            return (Interface)new RequestRealProxy<Interface>(id, requesterHandler).GetTransparentProxy();
        }
    }
}
