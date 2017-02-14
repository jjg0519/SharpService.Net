using Polly;
using Polly.Timeout;
using System;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.ServiceModel;
using NService.Core.Client;

namespace NService.Core.Requester
{
    public class RequestRealProxy<IObjcet> : RealProxy
    {
        private string id;

        private readonly Policy _circuitBreakerPolicy;
        private readonly TimeoutPolicy _timeoutPolicy;

        public RequestRealProxy(string id) : base(typeof(IObjcet))
        {
            _circuitBreakerPolicy = Policy
                .Handle<Exception>()
                .Or<TimeoutRejectedException>()
                .Or<TimeoutException>()
                .CircuitBreakerAsync(
                    exceptionsAllowedBeforeBreaking: 1,
                    durationOfBreak: new TimeSpan(0, 0, 1),
                    onBreak: (ex, breakDelay) =>
                    {
                        // _logger.LogError(".Breaker logging: Breaking the circuit for " + breakDelay.TotalMilliseconds + "ms!", ex);
                    },
                    onReset: () => { },//_logger.LogDebug(".Breaker logging: Call ok! Closed the circuit again."),
                    onHalfOpen: () => { }//_logger.LogDebug(".Breaker logging: Half-open; next call is a trial.")
                    );
            _timeoutPolicy = Policy.TimeoutAsync(1, TimeoutStrategy.Optimistic);

            this.id = id;
        }

        public override IMessage Invoke(IMessage msg)
        {
            return Policy.WrapAsync(_circuitBreakerPolicy, _timeoutPolicy).Execute(() =>
           {
               IMethodReturnMessage methodReturn = null;
               IMethodCallMessage methodCall = (IMethodCallMessage)msg;
               var factory = ClientFactory.CreateChannelFactory<IObjcet>(id);
               var channel = factory.CreateChannel();
               try
               {
                   object[] copiedArgs = Array.CreateInstance(typeof(object), methodCall.Args.Length) as object[];
                   methodCall.Args.CopyTo(copiedArgs, 0);

                   object returnValue = methodCall.MethodBase.Invoke(channel, copiedArgs);

                   methodReturn = new ReturnMessage(returnValue, copiedArgs, copiedArgs.Length, methodCall.LogicalCallContext, methodCall);
                   //TODO:Write log
               }
               catch(Exception ex)
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
           });
        }
    }
}
