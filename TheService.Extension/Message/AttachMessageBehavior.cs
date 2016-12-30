using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace TheService.Extension.Message
{
    public class AttachMessageBehavior : IClientMessageInspector
    {
        private Dictionary<string, string> messages { set; get; }

        public AttachMessageBehavior(Dictionary<string, string> messages)
        {
            this.messages = messages;
        }

        #region IClientMessageInspector 成员

        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {

        }


        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, IClientChannel channel)
        {
            if (messages != null)
            {
                foreach (var key in messages.Keys)
                {
                    MessageHeader userNameHeader = MessageHeader.CreateHeader(key, "http://tempuri.org", messages[key], false, "");
                    request.Headers.Add(userNameHeader);
                }
            }
            Console.WriteLine(request);
            return null;
        }

        #endregion
    }
}
