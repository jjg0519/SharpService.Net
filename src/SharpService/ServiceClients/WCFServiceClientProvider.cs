using System;
using SharpService.ServiceDiscovery;
using System.ServiceModel;
using SharpService.WCF;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using SharpService.Configuration;
using SharpService.HaStrategy;
using Polly;
using Polly.CircuitBreaker;

namespace SharpService.ServiceClients
{
    public class WCFServiceClientProvider : IServiceClientProvider
    {
        public IService GetServiceClient<IService>( RegistryInformation registryInformation,  ProtocolConfiguration protocolConfiguration)
        {                
            return (IService)new WCFRealProxy<IService>(registryInformation, protocolConfiguration).GetTransparentProxy();
        }
    }

    public class WCFRealProxy<IService> : RealProxy
    {
        private RegistryInformation registryInformation { get; set; }

        private ProtocolConfiguration protocolConfiguration { get; set; }

        public WCFRealProxy(RegistryInformation registryInformation, ProtocolConfiguration protocolConfiguration) : base(typeof(IService))
        {
            this.registryInformation = registryInformation;
            this.protocolConfiguration = protocolConfiguration;
        }

        public override IMessage Invoke(IMessage msg)
        {
            if (protocolConfiguration.CircuitBreak)
            {
                var methodCall = (IMethodCallMessage)msg;
                var circuitBreak = CircuitBreak.GetCircuitBreak(protocolConfiguration, ex => { return (ex as FaultException) != null; });
                try
                {
                    return Policy.Wrap(circuitBreak.circuitBreakerPolicy, circuitBreak.retryPolicy).Execute(() =>
                    {
                        var methodReturn = (IMethodReturnMessage)Call(msg);
                        if (methodReturn.Exception != null)
                        {
                            throw methodReturn.Exception;
                        }
                        return methodReturn;
                    });
                }
                catch (BrokenCircuitException ex)
                {
                    return new ReturnMessage(ex, methodCall);
                }
                catch (Exception ex)
                {
                    return new ReturnMessage(ex, methodCall); ;
                }
            }
            else
            {
                return Call(msg);
            }        
        }

        private IMessage Call(IMessage msg)
        {
            IMethodReturnMessage methodReturn = null;
            IMethodCallMessage methodCall = (IMethodCallMessage)msg;
            IService channel = default(IService);
            try
            {
                var factory = WCFHelper.CreateChannelFactory<IService>(registryInformation);
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
