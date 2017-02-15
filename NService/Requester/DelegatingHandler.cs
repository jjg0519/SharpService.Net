using NService.Factory;
using System;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;

namespace NService.Requester
{
    public class DelegatingHandler : IRequesterHandler
    {
        public virtual IMessage Invoke<Interface>(IMessage msg, string id)
        {
            IMethodReturnMessage methodReturn = null;
            IMethodCallMessage methodCall = (IMethodCallMessage)msg;
            Interface channel = default(Interface);
            try
            {
                var factory = ClientFactory.CreateChannelFactory<Interface>(id);
                channel = factory.CreateChannel();
                object[] copiedArgs = Array.CreateInstance(typeof(object), methodCall.Args.Length) as object[];
                methodCall.Args.CopyTo(copiedArgs, 0);
                object returnValue = methodCall.MethodBase.Invoke(channel, copiedArgs);
                methodReturn = new ReturnMessage(returnValue, copiedArgs, copiedArgs.Length, methodCall.LogicalCallContext, methodCall);
            }
            catch (CommunicationException ex)
            {
                var exception = ex;
                methodReturn = new ReturnMessage(exception, methodCall);
            }
            catch (TimeoutException ex)
            {
                var exception = ex;
                methodReturn = new ReturnMessage(exception, methodCall);
            }
            catch (Exception ex)
            {
                var exception = ex;
                if (ex.InnerException != null)
                    exception = ex.InnerException;
                methodReturn = new ReturnMessage(new RequestException(exception.Message, exception), methodCall);
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
