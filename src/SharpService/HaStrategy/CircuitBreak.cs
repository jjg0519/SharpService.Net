using Polly;
using SharpService.Components;
using SharpService.Configuration;
using System;
using System.Collections.Generic;

namespace SharpService.HaStrategy
{
    public class CircuitBreak
    {
        private readonly Logging.ILogger logger;
        public readonly Policy circuitBreakerPolicy;
        public readonly Policy retryPolicy;

        public CircuitBreak(int exceptionsAllowedBeforeBreaking, TimeSpan durationOfBreak, int reTries, Func<Exception, bool> exceptionPredicate)
        {
            logger = ObjectContainer.Resolve<Logging.ILogger>();

            circuitBreakerPolicy = Policy
                .Handle(exceptionPredicate)
                .CircuitBreaker(
                    exceptionsAllowedBeforeBreaking: exceptionsAllowedBeforeBreaking,
                    durationOfBreak: durationOfBreak,
                    onBreak: (ex, breakDelay) =>
                    {
                        logger.LogError(".Breaker logging: Breaking the circuit for " + breakDelay.TotalMilliseconds + "ms!", ex);
                    },
                    onReset: () =>
                    {
                        logger.LogDebug(".Breaker logging: Call ok! Closed the circuit again.");
                    },
                    onHalfOpen: () =>
                    {
                        logger.LogDebug(".Breaker logging: Half-open; next call is a trial.");
                    });


            retryPolicy = Policy
             .Handle(exceptionPredicate)
             .Retry(reTries);

        }

        private static Dictionary<string, CircuitBreak> circuitBreakers = new Dictionary<string, CircuitBreak>();

        public static CircuitBreak GetCircuitBreak(ProtocolConfiguration protocolConfiguration, Func<Exception, bool> exceptionPredicate)
        {
            string key = $"{protocolConfiguration.Protocol}_{protocolConfiguration.Name}_{protocolConfiguration.Defalut}";
            if (!circuitBreakers.ContainsKey(key))
            {
                circuitBreakers.Add(key, new CircuitBreak(
                                                               protocolConfiguration.ExceptionsAllowedBeforeBreaking,
                                                               TimeSpan.FromSeconds(protocolConfiguration.DurationOfBreak),
                                                               protocolConfiguration.ReTries,
                                                               exceptionPredicate));
            }
            return circuitBreakers[key];
        }
    }
}
