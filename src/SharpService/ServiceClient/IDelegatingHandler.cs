using System.Runtime.Remoting.Messaging;

namespace SharpService.ServiceRequester
{
    public interface IDelegatingHandler
    {
        IMessage Handler<Interface>(IMessage msg, string id, bool throwex = false);
    }
}
