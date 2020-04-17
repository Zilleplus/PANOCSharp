using kul.forbes.API;
using kul.forbes.domain;
using kul.forbes.testTools;
using MathNet.Numerics.LinearAlgebra;
using Xunit;

namespace kul.forbes.helpers.test
{
    public class PANOC_Tests
    {
        [Fact]
        public void Given_Polynomial_2th_degree_Solve()
        {
            var numberOfIterations =5;
            var degree = 6;

            var costFunction = new MockedFunctionBuilder()
                .WithCostGradient(
                    (x) => x.PointwisePower(degree).Sum(),
                    (x) => degree * x.PointwisePower(degree - 1))
                .Build()
                .Object;
            var constraint = new NormBox(dimension: 2, offSet: 2);

            var defaultConfig = new ConfigPanoc(problemDimension: 2);

            var solver = new PANOCSolver(costFunction,constraint,defaultConfig);
            var solution = solver.Solve(new double[] { 0.5,0.5 },maxIterations:numberOfIterations,minResidual:1e-15);

            var expected = Vector<double>.Build.Dense(new[] { 0.221282, 0.221282 }); // answers taken from the C-code
            Assert.Equal(expected: expected[0], actual: solution[0], precision: 6);
            Assert.Equal(expected: expected[1], actual: solution[1], precision: 6);
        }

        [Fact]
        public void Given_Polynomial_2th_degree_Solve_2th_Test()
        {
            var numberOfIterations =10;
            var degree = 6;

            var costFunction = new MockedFunctionBuilder()
                .WithCostGradient(
                    (x) => x.PointwisePower(degree).Sum(),
                    (x) => degree * x.PointwisePower(degree - 1))
                .Build()
                .Object;
            var constraint = new NormBox(dimension: 2, offSet: 2);

            var defaultConfig = new ConfigPanoc(problemDimension: 2);

            var solver = new PANOCSolver(costFunction,constraint,defaultConfig);
            var solution = solver.Solve(new double[] { 1,1 },maxIterations:numberOfIterations,minResidual:1e-15);

            var expected = Vector<double>.Build.Dense(new[] { 0.204269, 0.204269}); // answers taken from the C-code
            Assert.Equal(expected: expected[0], actual: solution[0], precision: 6);
            Assert.Equal(expected: expected[1], actual: solution[1], precision: 6);
        }
    }
}
