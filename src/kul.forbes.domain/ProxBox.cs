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

        public double Prox(Vector<double> vector)
        {
            double cost = 0;
            for (int i = 0; i < vector.Count; i++)
            {
                if (vector[i] > size)
                {
                    cost = penalty;
                    vector[i] = size;
                }
                if (vector[i] < -size)
                {
                    cost = penalty;
                    vector[i] = -size;
                }
            }
            return cost;
        }
    }
}
