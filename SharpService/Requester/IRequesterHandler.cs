using System.Runtime.Remoting.Messaging;

namespace SharpService.Requester
{
    public interface IRequesterHandler
    {
        IMessage Handler<Interface>(IMessage msg, string id, bool throwex = false);
    }
}
