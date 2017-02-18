using SharpService.ServiceProvider;
using System;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;

namespace SharpService.Requester
{
    public class DelegatingHandler : IRequesterHandler
    {
        public virtual IMessage Handler<Interface>(IMessage msg, string id, bool throwex = false)
        {
            IMethodReturnMessage methodReturn = null;
            IMethodCallMessage methodCall = (IMethodCallMessage)msg;
            Interface channel = default(Interface);
            try
            {
                var factory = WCFClientFactory.CreateChannelFactory<Interface>(id);
                channel = factory.CreateChannel();
                object[] copiedArgs = Array.CreateInstance(typeof(object), methodCall.Args.Length) as object[];
                methodCall.Args.CopyTo(copiedArgs, 0);
                object returnValue = methodCall.MethodBase.Invoke(channel, copiedArgs);
                methodReturn = new ReturnMessage(returnValue, copiedArgs, copiedArgs.Length, methodCall.LogicalCallContext, methodCall);
            }
            catch (Exception ex)
            {
                var exception = ex;
                if (ex.InnerException != null)
                    exception = ex.InnerException;
                if (!throwex)
                {
                    methodReturn = new ReturnMessage(exception, methodCall);
                }
                else
                {
                    throw new RequestServerException(exception.Message, exception);
                }
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
