using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace NService.Core.Behavior
{
    public class AttachMessageBehavior : IClientMessageInspector
    {
        private Dictionary<string, string> messages { set; get; }

        public AttachMessageBehavior(Dictionary<string, string> messages)
        {
            this.messages = messages;
        }

        #region IClientMessageInspector 成员

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {

        }


        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            if (messages != null)
            {
                foreach (var key in messages.Keys)
                {
                    MessageHeader header = MessageHeader.CreateHeader(key, "http://tempuri.org", messages[key], false, "");
                    request.Headers.Add(header);
                }
            }
            return null;
        }

        #endregion
    }
}
