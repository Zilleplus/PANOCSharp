using kul.forbes.contracts;
using kul.forbes.entities;
using MathNet.Numerics.LinearAlgebra;

namespace kul.forbes.helpers.domain
{
    public class LocationBuilder
    {
        private readonly IFunction function;

        public LocationBuilder(
            IFunction function)
        {
            this.function = function;
        }

        public Location Build(Vector<double> input)
        {
            return new Location(function.Evaluate(input), input);
        }

        public Location Build(params double[] input)
            => Build(Vector<double>.Build.DenseOfArray(input));
    }
}
