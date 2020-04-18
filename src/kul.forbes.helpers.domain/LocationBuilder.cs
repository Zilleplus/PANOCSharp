using kul.forbes.contracts;
using kul.forbes.entities;
using MathNet.Numerics.LinearAlgebra;

namespace kul.forbes.helpers.domain
{
    public class LocationBuilder
    {
        public static Location Build(IFunction function,Vector<double> input)
            => new Location(function.Evaluate(input), input);

        public static Location Build(IFunction function,params double[] input)
            => Build(function,Vector<double>.Build.DenseOfArray(input));
    }
}
