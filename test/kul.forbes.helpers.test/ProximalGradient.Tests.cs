﻿using kul.forbes.contracts.configs;
using kul.forbes.entities;
using MathNet.Numerics.LinearAlgebra;
using System.Linq;
using Xunit;

namespace kul.forbes.helpers.test
{
    public class ProximalGradientTests
    {
        class ProxConfig : IConfigProximalGradient
        {
            public double SafetyValueLineSearch => 0.05;

            public double LipschitzSafetyValue => 1e-6;

            public double MinimumDelta => 1e-12;

            public double minGammaValue => 10;
        }

        [Fact]
        public void Given_Polynomial_2th_degree_Solve()
        {
            var numberOfIterations = 99;
            var degree = 5;
            var poly = new VectorFunction( (x) => (x.PointwisePower(degree).Sum(), degree * x.PointwisePower(degree - 1)));
            var proximalOperator = new NormBox(dimension: 2, offSet: 2);

            var initPosition = new double[] { 0.5, 0.5 };
            var init = new Location(
                position:Vector<double>.Build.DenseOfArray(initPosition),
                evaluated: poly.Evaluate( Vector<double>.Build.DenseOfArray(initPosition)));
            var location = ProximalGradientStep.Calculate(init,new ProxConfig(),poly,proximalOperator).ProxLocation;
            var loops = Enumerable.Range(0, numberOfIterations)
                .Select(i=> 
                {
                    location = ProximalGradientStep.Calculate(location,new ProxConfig(),poly,proximalOperator).ProxLocation;
                    return location.Position.ToArray();
                })
                .ToList();

            var expected = Vector<double>.Build.Dense(new[] { 0.118816, 0.118816 }); // answers taken from the C-code

            Assert.Equal(expected: expected[0], actual: loops.Last()[0], precision: 6);
            Assert.Equal(expected: expected[1], actual: loops.Last()[1], precision: 6);
        }
    }
}
