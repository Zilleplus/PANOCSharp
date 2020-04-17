using kul.forbes.contracts;
using kul.forbes.contracts.configs;
using kul.forbes.entities;
using kul.forbes.helpers.contracts;
using kul.forbes.helpers.domain;
using MathNet.Numerics.LinearAlgebra;
using System;

namespace kul.forbes.domain
{
    public class Panoc : IPanoc
    {
        private readonly LocationBuilder locationBuilder;
        private readonly ProximalGradientCalculator proxCalculator;
        private readonly IAccelerator accelerator;

        public Panoc(
            LocationBuilder locationBuilder,
            ProximalGradientCalculator proxCalculator,
            IAccelerator accelerator,
            ILogger logger)
        {
            this.locationBuilder = locationBuilder;
            this.proxCalculator = proxCalculator;
            this.accelerator = accelerator;
        }

        public Vector<double> Solve(
            Vector<double> initLocation,
            int maxIterations, 
            double minResidual)
        {
            var residual = double.MaxValue;
            var prox = proxCalculator.Calculate(locationBuilder.Build(initLocation));
            var fbe = ForwardBackwardEnvelop.Calculate(prox);

            for (int i = 0; i < maxIterations && residual>minResidual; i++)
            {
                var oldLocation = prox.Location;
                if (accelerator.HasCache) // there is accelstep then we can improve stuff
                {
                    (residual, prox, fbe) = Search(prox, fbe);
                }
                else 
                {
                    prox = proxCalculator.Calculate(prox.ProxLocation);
                }
                accelerator.Update(oldLocation: oldLocation, newLocation: prox.Location);
            }

            return prox.Location.Position;
        }

        private static Vector<double> Residual(ProximalGradient prox)
            => ((prox.Location.Position - prox.ProxLocation.Position) / prox.ProxLocation.Gamma);

        private (double residual ,ProximalGradient prox,double fbe) 
            Search(ProximalGradient prox, double fbe)
        {
            Func<int, double> tau = i => Math.Pow(2.0, i);
            var accelerationStep = accelerator.GetStep(prox.Location);
            for (int i = 0; i < 10; i++) // FBE_LINESEARCH_MAX_ITERATIONS=10
            {
                var step = prox.Location.Position
                    + (prox.ProxLocation.Position - prox.Location.Position) * (1-tau(i))
                    + accelerationStep * tau(i);

                var newProx = proxCalculator.Calculate(locationBuilder.Build(step));
                var newFbe = ForwardBackwardEnvelop.Calculate(newProx);

                if (newFbe< fbe)
                {
                    return ((Residual(newProx).InfinityNorm(),newProx,newFbe));
                }
            }
            // use only proximal gradient, no accelerator
            var pureProx = proxCalculator.Calculate(prox.ProxLocation);
            return (Residual(pureProx).InfinityNorm(),pureProx,ForwardBackwardEnvelop.Calculate(pureProx));
        }
    }
}
