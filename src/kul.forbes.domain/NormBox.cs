using kul.forbes.contracts;
using MathNet.Numerics.LinearAlgebra;
using static System.Math;

namespace kul.forbes.domain
{
    public class NormBox : IProx    
    {
        private readonly int dimension;
        private readonly double penality;
        private readonly double offSet;

        public NormBox(
            int dimension,
            double penality,
            double offSet)
        {
            this.dimension = dimension;
            this.penality = penality;
            this.offSet = offSet;
        }

        private double Cost(Vector<double> x)
        {
            var potentialX = x.L1Norm() - offSet;
            return (potentialX > 0) ? potentialX : 0;
        }

        public (double cost, Vector<double> location) Prox(Vector<double> x)
        {
            var gradient = x.L1Norm() switch
            {
                // |x|<w -> x
                _ when x.L1Norm() < offSet => x,
                // |x|>2w -> sign(x)*(|x|-w)
                _ when x.L1Norm() > 2*offSet => x.Map(e => Sign(e) * (Abs(e) - offSet)),
                // w<|x|<2w -> sign(x)*w 
                _ => x.Map(e => Sign(e) * offSet)
            };

            return (Cost(x), gradient);
        }
    }
}
