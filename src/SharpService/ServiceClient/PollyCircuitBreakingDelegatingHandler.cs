using SharpService.Logging;
using Polly;
using Polly.CircuitBreaker;
using System;
using System.Runtime.Remoting.Messaging;
using SharpService.Components;

namespace SharpService.ServiceRequester
{
    public class PollyCircuitBreakingDelegatingHandler : WCFDelegatingHandler
    {
        private readonly ILogger _logger;
        private readonly int _exceptionsAllowedBeforeBreaking;
        private readonly TimeSpan _durationOfBreak;
        private readonly Policy _circuitBreakerPolicy;

        public PollyCircuitBreakingDelegatingHandler(
            int exceptionsAllowedBeforeBreaking,
            TimeSpan durationOfBreak)
        {
            _exceptionsAllowedBeforeBreaking = exceptionsAllowedBeforeBreaking;
            _durationOfBreak = durationOfBreak;

            _circuitBreakerPolicy = Policy
                .Handle<UnableRequestException>()
                .CircuitBreaker(
                    exceptionsAllowedBeforeBreaking: exceptionsAllowedBeforeBreaking,
                    durationOfBreak: durationOfBreak,
                    onBreak: (ex, breakDelay) =>
                    {
                        _logger.LogError(".Breaker logging: Breaking the circuit for " + breakDelay.TotalMilliseconds + "ms!", ex);
                    },
                    onReset: () =>
                    {
                        //_logger.LogDebug(".Breaker logging: Call ok! Closed the circuit again.");
                    },
                    onHalfOpen: () =>
                    {
                        //_logger.LogDebug(".Breaker logging: Half-open; next call is a trial.");
                    });

            ObjectContainer.TryResolve(out _logger);
        }

        public override IMessage Handler<Interface>(IMessage msg, string id, bool throwex = true)
        {
            IMethodCallMessage methodCall = (IMethodCallMessage)msg;
            try
            {
                return _circuitBreakerPolicy.Execute(() =>
                 {
                     return (IMethodReturnMessage)base.Handler<Interface>(msg, id, true);
                 });
            }
            catch (BrokenCircuitException ex)
            {
                return new ReturnMessage(ex, methodCall);
            }
            catch (UnableRequestException ex)
            {
                return new ReturnMessage(ex, methodCall); ;
            }
        }
    }
}
