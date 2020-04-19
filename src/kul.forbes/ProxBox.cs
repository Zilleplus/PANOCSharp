using kul.forbes.contracts;
using MathNet.Numerics.LinearAlgebra;

namespace kul.forbes
{
    public class ProxBox : IProx
    {
        private readonly double size;
        private readonly double penalty;
        private readonly int dimension;

        /// <summary>
        /// Put the value back within the borders of the box
        /// </summary>
        /// <param name="size">size of the border, values higher then this are pushed back inside</param>
        /// <param name="penalty">cost of a value outside the box</param>
        /// <param name="dimension">the dimension of the box</param>
        public ProxBox(
            double size,
            double penalty,
            int dimension)
        {
            this.size = size;
            this.penalty = penalty;
            this.dimension = dimension;
        }

        public (double,Vector<double>) Prox(Vector<double> vector)
        {
            var prox = Vector<double>.Build.Dense(vector.Count);
            vector.CopyTo(prox);
            double cost = 0;
            for (int i = 0; i < vector.Count; i++)
            {
                if (prox[i] > size)
                {
                    cost = penalty;
                    prox[i] = size;
                }
                if (prox[i] < -size)
                {
                    cost = penalty;
                    prox[i] = -size;
                }
            }
            return (cost,prox);
        }
    }
}
