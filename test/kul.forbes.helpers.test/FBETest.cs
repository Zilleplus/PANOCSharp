using Xunit;
using kul.forbes.contracts.configs;
using kul.forbes.entities;
using MathNet.Numerics.LinearAlgebra;

namespace kul.forbes.helpers.test
{
    public class FBETest
    {
        [Fact]
        public static void TestFBEOnPoly()
        {
            var defaultConfig = new ConfigPanoc(problemDimension: 2);
            IConfigProximalGradient config = defaultConfig;
            var function = new VectorFunction(x => (3 * x.PointwisePower(2).Sum(), 6 * x));
            var prox = new ProxBox(size: 7, penalty: 1, dimension: 2);

            var position = Vector<double>.Build.DenseOfArray(new double[] { 6, 10 });
            var location = new Location(position, function.Evaluate(position));
            var proxGrad = ProximalGradientStep.Calculate(location, config, function, prox);
            double fbe = ForwardBackwardEnvelop.Calculate(proxGrad);
            var direction = proxGrad.Location.Position - proxGrad.ProxLocation.Position;

            Assert.Equal(expected: 20.408, actual: fbe,precision: 1);

            // Manual calculation of this fbe:
            // f(x) = 3x^2
            // df(x) = 6x
            // gx(x) = box{7}
            //
            // -----
            // start position: [6;10]
            // cost at this position = 36*3+100*3=408
            // df at this position = [6*6;6*10] = [36;60]

            // prox step taken to
            // -----
            // position: [0.299999;0.499999]
            // cost at this position: 1.019999
            // df at this position: [1.79999;2.99999]
            // gamma: 0.15833
            // constraint cost: 0

            // some intermediate values
            // direction = x_old - x_new = [6;10] -[0.299999;0.499999] = [5.70000;9.50000]
            // direction* direction = 122.74000
            // df'*direction = 775.20000

            // so the fbe becomes:
            // fbe = fx + gx_new - df'*direction + (1/(gamma*2))*(direction'*direction)
            //     = (408 + 0 - 775.20000 + (1/(0.15833*2))*(122.74000))
            //     = 20.408
        }
    }
}
