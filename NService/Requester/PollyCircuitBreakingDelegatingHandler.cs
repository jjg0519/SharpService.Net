using NService.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Timeout;
using System;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;

namespace NService.Requester
{
    public class PollyCircuitBreakingDelegatingHandler : DelegatingHandler
    {

        private readonly INServiceLogger _logger;
        private readonly int _exceptionsAllowedBeforeBreaking;
        private readonly TimeSpan _durationOfBreak;
        private readonly Policy _circuitBreakerPolicy;
        private readonly TimeoutPolicy _timeoutPolicy;

        public PollyCircuitBreakingDelegatingHandler(int exceptionsAllowedBeforeBreaking, TimeSpan durationOfBreak, TimeSpan timeoutValue
            , TimeoutStrategy timeoutStrategy, INServiceLogger logger = null)
        {
            this._exceptionsAllowedBeforeBreaking = exceptionsAllowedBeforeBreaking;
            this._durationOfBreak = durationOfBreak;

            _circuitBreakerPolicy = Policy
                .Handle<RequestException>()
                .Or<TimeoutRejectedException>()
                .Or<CommunicationException>()
                .Or<TimeoutException>()
                .CircuitBreakerAsync(
                    exceptionsAllowedBeforeBreaking: exceptionsAllowedBeforeBreaking,
                    durationOfBreak: durationOfBreak,
                    onBreak: (ex, breakDelay) =>
                    {
                        // _logger.LogError(".Breaker logging: Breaking the circuit for " + breakDelay.TotalMilliseconds + "ms!", ex);
                    },
                    onReset: () =>
                    {
                        //_logger.LogDebug(".Breaker logging: Call ok! Closed the circuit again."),
                    },
                    onHalfOpen: () =>
                    {
                        //_logger.LogDebug(".Breaker logging: Half-open; next call is a trial.")
                    });
            _timeoutPolicy = Policy.TimeoutAsync(timeoutValue, timeoutStrategy);
            _logger = logger;
        }

        public override IMessage Invoke<Interface>(IMessage msg, string id)
        {
            try
            {
                return Policy.WrapAsync(_circuitBreakerPolicy, _timeoutPolicy).Execute(() =>
                {
                    return base.Invoke<Interface>(msg, id);
                });
            }
            catch (BrokenCircuitException ex)
            {
                //_logger.LogError($"Reached to allowed number of exceptions. Circuit is open. AllowedExceptionCount: {_exceptionsAllowedBeforeBreaking}, DurationOfBreak: {_durationOfBreak}", ex);
                throw;
            }
        }
    }
}
