using SharpService.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Timeout;
using System;
using System.Runtime.Remoting.Messaging;
using SharpService.Components;

namespace SharpService.Requester
{
    public class PollyCircuitBreakingDelegatingHandler : DelegatingHandler
    {
        private readonly ISharpServiceLogger _logger;
        private readonly int _exceptionsAllowedBeforeBreaking;
        private readonly TimeSpan _durationOfBreak;
        private readonly Policy _circuitBreakerPolicy;
        private readonly TimeoutPolicy _timeoutPolicy;

        public PollyCircuitBreakingDelegatingHandler(
            int exceptionsAllowedBeforeBreaking,
            TimeSpan durationOfBreak,
            TimeSpan timeoutValue,
            TimeoutStrategy timeoutStrategy
           )
        {
            _exceptionsAllowedBeforeBreaking = exceptionsAllowedBeforeBreaking;
            _durationOfBreak = durationOfBreak;

            _circuitBreakerPolicy = Policy
                .Handle<RequestServerException>()
                .Or<AggregateException>()
                .Or<TimeoutRejectedException>()
                .CircuitBreaker(
                    exceptionsAllowedBeforeBreaking: exceptionsAllowedBeforeBreaking,
                    durationOfBreak: durationOfBreak,
                    onBreak: (ex, breakDelay) =>
                    {
                        // _logger.LogError(".Breaker logging: Breaking the circuit for " + breakDelay.TotalMilliseconds + "ms!", ex);
#if DEBUG
                        Console.WriteLine($".Breaker logging: Breaking the circuit for{breakDelay.TotalMilliseconds}ms! {ex.Message}{ex.GetType()}");
#endif
                    },
                    onReset: () =>
                    {
                        //_logger.LogDebug(".Breaker logging: Call ok! Closed the circuit again.");
#if DEBUG
                        Console.WriteLine(".Breaker logging: Call ok! Closed the circuit again.");
#endif
                    },
                    onHalfOpen: () =>
                    {
                        //_logger.LogDebug(".Breaker logging: Half-open; next call is a trial.");
#if DEBUG
                        Console.WriteLine(".Breaker logging: Half-open; next call is a trial.");
#endif
                    });
            _timeoutPolicy = Policy.Timeout(timeoutValue, timeoutStrategy);
            _logger = ObjectContainer.Resolve<ISharpServiceLogger>();
        }

        public override IMessage Invoke<Interface>(IMessage msg, string id, bool throwex = true)
        {
            IMethodCallMessage methodCall = (IMethodCallMessage)msg;
            try
            {
                return Policy.Wrap(_circuitBreakerPolicy, _timeoutPolicy).Execute(() =>
                 {
                     return (IMethodReturnMessage)base.Invoke<Interface>(msg, id, true);
                 });
            }
            catch (BrokenCircuitException ex)
            {
                //_logger.LogError($"Reached to allowed number of exceptions. Circuit is open. AllowedExceptionCount: {_exceptionsAllowedBeforeBreaking}, DurationOfBreak: {_durationOfBreak}", ex);
#if DEBUG
                Console.WriteLine($"Reached to allowed number of exceptions. Circuit is open. AllowedExceptionCount: {_exceptionsAllowedBeforeBreaking}, DurationOfBreak: {_durationOfBreak} {ex.Message}{ex.GetType()}");
#endif
                return new ReturnMessage(ex, methodCall);
            }
            catch (TimeoutRejectedException ex)
            {
#if DEBUG
                Console.WriteLine($"The delegate executed through TimeoutPolicy did not complete within the timeout.  {ex.Message}{ex.GetType()}");
#endif
                return new ReturnMessage(ex, methodCall); ;
            }
            catch (RequestServerException ex)
            {
                return new ReturnMessage(ex, methodCall); ;
            }
            catch (AggregateException ex)
            {
                return new ReturnMessage(ex, methodCall); ;
            }        
            catch (Exception ex)
            {
                return new ReturnMessage(ex, methodCall); ;
            }
        }
    }
}
