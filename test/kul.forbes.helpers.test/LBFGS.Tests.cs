using kul.forbes.contracts;
using kul.forbes.contracts.configs;
using kul.forbes.entities;
using kul.forbes.helpers.domain;
using kul.forbes.helpers.domain.Accelerators;
using kul.forbes.testTools;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xunit;

namespace kul.forbes.helpers.test
{
    public class LBFGS_Tests
    {
        Vector<double> RosenBrockGradient(Vector<double> location)
        {
            double a = 1;
            double b = 100;

            var gradient = Vector<double>.Build.Dense(location.Count);

            // Matlab: df = @(x) [-2*(a-(b+1)*x(1)+b*x(2)); 2*b*(x(2)-x(1)) ]; 
            gradient[0] = -2 * (a - (b + 1) * location[0] + b * location[1]);
            gradient[1] = 2 * b * (location[1] - location[0]);

            return gradient;
        }

        class RosenConfig : IConfigLBFGS
        {
            public int CacheSize => 20;

            public int ProblemDimension => 2;
        }

        [Fact]
        public void Given_RosenBrock_Solve_With_LBFGS()
        {
            var rosen = new MockedFunctionBuilder()
                .WithCostGradient(
                    (a)=>0,
                    (x)=> RosenBrockGradient(x))
                .Build()
                .Object;

            var sut = new LBFGS( new RosenConfig() );

            var startLocationVector =  new VectorBuilder()
                .WithElements(-1.2, 1)
                .Build();

            var location = new Location(startLocationVector,rosen.Evaluate(startLocationVector));
            var res = Enumerable
                .Range(0, 4)
                .Select(i =>
                {
                    var newLocation = new Location(
                        location.Position + sut.GetStep(location),
                        rosen.Evaluate(location.Position + sut.GetStep(location)));
                    sut.Update(location,newLocation);

                    location = newLocation;
                    return newLocation.Position.ToArray();
                })
                .ToList();

            res.ForEach(r => Debug.Print(
                "current location ->"
                + String.Join("," , r.Select(vector=> vector.ToString()))
                + "\n" ));

            var precision = 1e-6;
            var expectedLocations = new List<double[]>
            {
                new double[]{ 443.2 , -439 },
                new double[]{-0.089055 , -0.094527 },
                new double[]{ -0.086346 , -0.091805 },
                new double[]{ 1 , 1 },
            };
            var isCorrectEnough = res.Zip(expectedLocations, (x, y)
                 => Math.Abs(x[0] - y[0]) < precision
                 && Math.Abs(x[1] - y[1]) < precision)
               .Aggregate(true,(x,y)=>x & y);
            Assert.True(isCorrectEnough);
        }
    }
}
