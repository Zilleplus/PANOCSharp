using kul.forbes.contracts;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Text;

namespace kul.forbes.testTools
{
    public class VectorBuilder : IBuilder<Vector<double>>
    {
        private List<double> vector = new List<double>();

        public VectorBuilder WithElements(params double[] elements)
        {
            vector.AddRange(elements);

            return this;
        }

        public Vector<double> Build()
        {
            return Vector<double>.Build.DenseOfArray(vector.ToArray());
        }
    }
}
