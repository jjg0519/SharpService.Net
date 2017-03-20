using SharpService.Configuration;
using SharpService.ServiceDiscovery;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;

namespace SharpService.ServiceRequester
{
    public class WCFDelegatingHandler : IDelegatingHandler
    {
        private const string refererConfig = "serviceGroup/refererConfig";
        private static List<RefererConfiguration> refererConfigurations = ConfigurationManager.GetSection(refererConfig) as List<RefererConfiguration>;

        public virtual IMessage Handler<Interface>(IMessage msg, string id, bool throwex = false)
        {
            IMethodReturnMessage methodReturn = null;
            IMethodCallMessage methodCall = (IMethodCallMessage)msg;
            Interface channel = default(Interface);
            RegistryInformation service = default(RegistryInformation);
            try
            {
                var refererConfiguration = refererConfigurations.FirstOrDefault(x => x.Id == id);
                if (refererConfiguration == null)
                {
                    throw new ArgumentNullException("can not find referer config");
                }
                service = WCFClientFactory.GetService(refererConfiguration).Result;
                var factory = WCFClientFactory.CreateChannelFactory<Interface>(service);
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
                    throw new UnableRequestException(exception.Message, exception) { service = service };
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
