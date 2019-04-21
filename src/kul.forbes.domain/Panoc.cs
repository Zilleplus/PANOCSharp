using kul.forbes.contracts;
using kul.forbes.contracts.configs;
using kul.forbes.entities;
using kul.forbes.helpers.contracts;
using MathNet.Numerics.LinearAlgebra;
using System;

namespace kul.forbes.domain
{
    public class Panoc : IPanoc
    {
        private readonly IBuilder<Vector<double>, Location> locationBuilder;
        private readonly IBuilder<Vector<double>, ProxLocation> proxLocationBuilder;
        private readonly ICalculator<Location, ProximalGradient> proxCalculator;
        private readonly IAccelerator accelerator;
        private readonly IConfigPanoc config;
        private readonly ILogger logger;

        public Panoc(
            IBuilder<Vector<double>,Location> locationBuilder,
            IBuilder<Vector<double>,ProxLocation> proxLocationBuilder,
            ICalculator<Location, ProximalGradient> proxCalculator,
            IAccelerator accelerator,
            IConfigPanoc config,
            ILogger logger)
        {
            this.locationBuilder = locationBuilder;
            this.proxLocationBuilder = proxLocationBuilder;
            this.proxCalculator = proxCalculator;
            this.accelerator = accelerator;
            this.config = config;
            this.logger = logger;
        }

        public Vector<double> Solve(Vector<double> initLocation)
        {
            var location = locationBuilder.Build(initLocation);

            var maxIter = 10;
            for (int i = 0; i < maxIter ; i++)
            {
                Search(location);
            }

            throw new NotImplementedException();
        }

        private Location Search(Location location)
        {
            var prox = proxCalculator.Calculate(location);
            Func<int, double> tau = i => Math.Pow(2.0, i);
            for (int i = 0; i < 4; i++)
            {
                var accelerationStep = accelerator.GetStep(prox.Location);
                var step = prox.Location.Position
                    + (prox.ProxLocation.Position - prox.Location.Position) * (1-tau(i))
                    + accelerationStep * tau(i);

                if (LineSearchCondition(prox))
                    return proxLocationBuilder.Build(step + location.Position);
            }

            return prox.ProxLocation;
        }

        private bool LineSearchCondition(ProximalGradient proximalGradient)
        {
            throw new NotImplementedException();
        }
    }
}
