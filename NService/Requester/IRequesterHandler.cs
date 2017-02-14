using System.Runtime.Remoting.Messaging;

namespace NService.Requester
{
    public interface IRequesterHandler
    {
        IMessage Invoke<T>(IMessage msg, string id);
    }
}
