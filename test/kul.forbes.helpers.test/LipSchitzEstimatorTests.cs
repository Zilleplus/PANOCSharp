using kul.forbes.contracts.configs;
using kul.forbes.entities;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace kul.forbes.helpers.test
{
    public class LipSchitzEstimatorTests
    {
        class TestConfig : IConfigLipschitzEstimator
        {
            public double LipschitzSafetyValue { get;} = 1e-6; 
            public double MinimumDelta { get; } = 1e-12;
        }

        [Fact]
        public static void Give_Poly_Estimate_LipSchitz_At_Certain_Position()
        { 
            var degree = 2;
            var poly = new VectorFunction( (x) => (x.PointwisePower(degree).Sum(), degree * x.PointwisePower(degree - 1)));

            var initPosition = Vector<double>.Build.DenseOfArray(new double[] { 6, 10 });
            var location = new Location(initPosition, poly.Evaluate(initPosition));

            var res = LipschitzEstimator.Estimate(location, new TestConfig(), poly);

            Assert.Equal(expected: 2, actual: res,precision: 4);
        }
    }
}
