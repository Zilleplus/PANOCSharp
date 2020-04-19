using kul.forbes.contracts;
using kul.forbes.contracts.configs;
using kul.forbes.entities;
using kul.forbes.helpers;
using kul.forbes.helpers.contracts;
using MathNet.Numerics.LinearAlgebra;
using System;

namespace kul.forbes
{
    public class Panoc 
    {
        private readonly IAccelerator accelerator;
        private readonly IConfigPanoc config;

        public Panoc(
            IAccelerator accelerator,
            IConfigPanoc config) 
        {
            this.accelerator = accelerator;
            this.config = config;
        }

        public Vector<double> Solve(
            Vector<double> initLocation,
            int maxIterations, 
            double minResidual,
            IFunction function,
            IProx proxFunction)
        {
            var residual = double.MaxValue;
            var prox = ProximalGradientStep.Calculate(
                new Location(initLocation,function.Evaluate(initLocation)),
                config,
                function,
                proxFunction);
            var fbe = ForwardBackwardEnvelop.Calculate(prox);

            for (int i = 0; i < maxIterations && residual>minResidual; i++)
            {
                var oldLocation = prox.Location;
                if (accelerator.HasCache) // there is accelstep then we can improve stuff
                {
                    (residual, prox, fbe) = Search(prox, fbe,function,proxFunction);
                }
                else 
                {
                    prox = ProximalGradientStep.Calculate(prox.ProxLocation,config,function,proxFunction);
                }
                accelerator.Update(oldLocation: oldLocation, newLocation: prox.Location);
            }

            return prox.Location.Position;
        }

        private static Vector<double> Residual(ProximalGradient prox)
            => ((prox.Location.Position - prox.ProxLocation.Position) / prox.ProxLocation.Gamma);

        private (double residual ,ProximalGradient prox,double fbe) 
            Search(ProximalGradient prox, double fbe,IFunction function,IProx proxFunc)
        {
            Func<int, double> tau = i => Math.Pow(2.0, i);
            var accelerationStep = accelerator.GetStep(prox.Location);
            for (int i = 0; i < 10; i++) // FBE_LINESEARCH_MAX_ITERATIONS=10
            {
                var step = prox.Location.Position
                    + (prox.ProxLocation.Position - prox.Location.Position) * (1-tau(i))
                    + accelerationStep * tau(i);

                var newProx = ProximalGradientStep.Calculate(
                    new Location(step,function.Evaluate(step)),
                    config,
                    function,
                    proxFunc);
                var newFbe = ForwardBackwardEnvelop.Calculate(newProx);

                if (newFbe< fbe)
                {
                    return ((Residual(newProx).InfinityNorm(),newProx,newFbe));
                }
            }
            // use only proximal gradient, no accelerator
            var pureProx = ProximalGradientStep.Calculate(prox.ProxLocation,config,function,proxFunc);
            return (Residual(pureProx).InfinityNorm(),pureProx,ForwardBackwardEnvelop.Calculate(pureProx));
        }
    }
}
