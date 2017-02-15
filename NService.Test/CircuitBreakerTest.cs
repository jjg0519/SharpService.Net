using NUnit.Framework;
using Polly;
using Polly.CircuitBreaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NService.Test
{
    public class CircuitBreakerTest
    {
        [Test]
        public void QuickstartTest()
        {
            CircuitBreakerPolicy breaker = Policy
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

            System.Threading.Thread.Sleep(TimeSpan.FromMinutes(1));
            try
            {
                breaker.Execute(() => { });
            }
            catch (BrokenCircuitException ex)
            {
                Console.WriteLine($"Reached to allowed number of exceptions. Circuit is open.{ex.Message}");
            }
        }
    }
}
