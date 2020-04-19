using kul.forbes.contracts;
using MathNet.Numerics.LinearAlgebra;
using System;

namespace kul.forbes.domain
{
    public class ProxBox : IProx
    {
        private readonly double size;
        private readonly double penalty;
        private readonly int dimension;

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
