using System.Runtime.Remoting.Messaging;

namespace NService.Requester
{
    public interface IRequesterHandler
    {
        IMessage Invoke<Interface>(IMessage msg, string id);
    }
}
