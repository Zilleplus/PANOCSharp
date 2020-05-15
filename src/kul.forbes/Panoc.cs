using kul.forbes.contracts;
using kul.forbes.contracts.configs;
using kul.forbes.entities;
using kul.forbes.helpers;
using kul.forbes.helpers.Accelerators;
using kul.forbes.helpers.contracts;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;

namespace kul.forbes
{
    public class PanocDiagnostics
    {
        public PanocDiagnostics(double tau,ProximalGradient proxOld,ProximalGradient proxNew)
        {
            Tau = tau;
            ProxOld = proxOld;
            ProxNew = proxNew;
        }

        public double Tau { get; }
        public ProximalGradient ProxOld { get; }
        public ProximalGradient ProxNew { get; }
    }

    public class Panoc 
    {
        private readonly IConfigPanoc config;
        public IEnumerable<PanocDiagnostics> diagnostics { get; } = Enumerable.Empty<PanocDiagnostics>();

        public Panoc( IConfigPanoc config) 
        {
            this.config = config;
        }

        public Vector<double> Solve(
            Vector<double> initLocation,
            int maxIterations, 
            double minResidual,
            IFunction function,
            IProx proxFunction,
            bool diagnosticsEnabled = false)
        {
            var diagnostics = new List<PanocDiagnostics>();
            var residual = double.MaxValue;
            var prox = ProximalGradientStep.Calculate(
                new Location(initLocation,function.Evaluate(initLocation)),
                config,
                function,
                proxFunction);
            var fbe = ForwardBackwardEnvelop.Calculate(prox);
            var accelerator = new LBFGS(config);
            double tau = 0;
            for (int i = 0; i < maxIterations && residual>minResidual; i++)
            {
                var oldProx = prox;

                var oldGamma = prox.ProxLocation.Gamma;
                if (accelerator.HasCache) // If there is accelstep(which needs previous runs) then we can improve stuff
                {
                    (residual, prox, fbe,tau) = Search(prox, fbe,function,proxFunction,config,accelerator.GetStep(prox.Location));
                }
                else 
                {
                    prox = ProximalGradientStep.Calculate(prox.ProxLocation,config,function,proxFunction);
                    fbe = ForwardBackwardEnvelop.Calculate(prox);
                }
                // This update doesn't always mean that the cache will be updated,
                // the lbfgs does a carefull update and will refuse some updates due to beein badly conditioned
                if (oldGamma != prox.ProxLocation.Gamma) { accelerator.Reset(); }
                var cacheUpdated = accelerator.Update(oldLocation: oldProx.Location, newLocation: prox.Location);

                if (diagnosticsEnabled) { diagnostics.Add(new PanocDiagnostics(tau: tau, prox,oldProx)); }
            }

            return prox.Location.Position;
        }

        private static Vector<double> Residual(ProximalGradient prox)
            => ((prox.Location.Position - prox.ProxLocation.Position) / prox.ProxLocation.Gamma);

        private static (double residual ,ProximalGradient prox,double fbe,double tau) 
            Search(
            ProximalGradient prox,
            double fbe,
            IFunction function,
            IProx proxFunc,
            IConfigPanoc config,
            Vector<double> accelerationStep)
        {
            Func<int, double> tau = i => Math.Pow(2.0, i);
            for (int i = 0; i < config.FBEMaxIterations; i++) 
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
                    return ((Residual(newProx).InfinityNorm(),newProx,newFbe,tau(i)));
                }
            }
            // use only proximal gradient, no accelerator
            var pureProx = ProximalGradientStep.Calculate(prox.ProxLocation,config,function,proxFunc);
            return (Residual(pureProx).InfinityNorm(),pureProx,ForwardBackwardEnvelop.Calculate(pureProx),tau:0);
        }
    }
}
