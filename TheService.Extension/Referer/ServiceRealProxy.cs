using TheService.Extension.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace TheService.Extension
{
    public class ServiceRealProxy<IObjcet> : RealProxy
    {
       
        private string id;

        private Dictionary<string, string> messages { set; get; }
        public ServiceRealProxy(string id, Dictionary<string, string> messages = null) : base(typeof(IObjcet))
        {
            this.id = id;
            this.messages = messages;
        }

        public override IMessage Invoke(IMessage msg)
        {
            IMethodReturnMessage methodReturn = null;
            IMethodCallMessage methodCall = (IMethodCallMessage)msg;
            var client = ClientFactory.CreateChannelFactory<IObjcet>(id, messages);
            var channel = client.CreateChannel();
            try
            {
                object[] copiedArgs = Array.CreateInstance(typeof(object), methodCall.Args.Length) as object[];
                methodCall.Args.CopyTo(copiedArgs, 0);

                object returnValue = methodCall.MethodBase.Invoke(channel, copiedArgs);

                methodReturn = new ReturnMessage(returnValue, copiedArgs, copiedArgs.Length, methodCall.LogicalCallContext, methodCall);
                //TODO:Write log
            }
            catch (Exception ex)
            {
                var exception = ex;
                if (ex.InnerException != null)
                    exception = ex.InnerException;
                methodReturn = new ReturnMessage(exception, methodCall);
            }
            finally
            {
                var commObj = channel as ICommunicationObject;
                if (commObj != null)
                {
                    try
                    {
                        commObj.Close();
                    }
                    catch (CommunicationException)
                    {
                        commObj.Abort();
                    }
                    catch (TimeoutException)
                    {
                        commObj.Abort();
                    }
                    catch (Exception)
                    {
                        commObj.Abort();
                    }
                }
            }
            return methodReturn;
        }

    }
}
