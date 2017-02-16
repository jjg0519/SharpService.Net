using NUnit.Framework;
using Polly;
using Polly.CircuitBreaker;
using Polly.Timeout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NService.Test
{
    public class PollyTest
    {
        [Test]
        public void CircuitBreakerTest()
        {
            var breaker = Policy
                   .Handle<Exception>()
                   .CircuitBreaker(2, TimeSpan.FromMinutes(1),
                   onBreak: (ex, breakDelay) =>
                   {
                       Console.WriteLine($".Breaker logging: Breaking the circuit for {breakDelay.TotalMilliseconds}ms! {ex.Message}{ex.GetType()}");
                   },
                    onReset: () =>
                    {
                        Console.WriteLine(".Breaker logging: Call ok! Closed the circuit again.");
                    },
                   onHalfOpen: () =>
                   {
                       Console.WriteLine(".Breaker logging: Half-open; next call is a trial.");
                   });
            try
            {
                breaker.Execute(() =>
                {
                    throw new Exception("error");
                });
            }
            catch (Exception ex) { }

            try
            {
                breaker.Execute(() => { throw new Exception("error"); });
            }
            catch (Exception ex) { }

            try
            {
                breaker.Execute(() => { });
            }
            catch (BrokenCircuitException ex)
            {
                Console.WriteLine($"Reached to allowed number of exceptions. Circuit is open.{ex.Message}");
            }

            Thread.Sleep(TimeSpan.FromMinutes(1));
            try
            {
                breaker.Execute(() => { });
            }
            catch (BrokenCircuitException ex)
            {
                Console.WriteLine($"Reached to allowed number of exceptions. Circuit is open.{ex.Message}");
            }
        }

        [Test]
        public void TimeoutPessimisticTest()
        {
            var timeoutPolicy = Policy.Timeout(
                seconds: 30,
                timeoutStrategy: TimeoutStrategy.Pessimistic,
                onTimeout: (context, timespan, task) =>
                {
                    Console.WriteLine($"Pessimistic");
                });
            try
            {
                timeoutPolicy.Execute(() =>
                  {
                      Thread.Sleep(TimeSpan.FromSeconds(60));
                  });
            }
            catch (TimeoutRejectedException ex)
            {
            }
        }


        [Test]
        public void TimeoutOptimisticTest()
        {
            var timeoutPolicy = Policy.Timeout(
                seconds: 30,
                timeoutStrategy: TimeoutStrategy.Optimistic,
                onTimeout: (context, timespan, task) =>
                {
                    Console.WriteLine($"Optimistic");
                });
            try
            {
                CancellationTokenSource s = new CancellationTokenSource();
                CancellationToken token = s.Token;
                timeoutPolicy.Execute((cancellationToken) =>
                {
                    Thread.Sleep(TimeSpan.FromSeconds(60));
                }, token);
            }
            catch (TimeoutRejectedException ex)
            {
            }
        }
    }
}
