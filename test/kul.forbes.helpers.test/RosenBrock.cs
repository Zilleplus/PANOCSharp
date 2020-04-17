using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Text;

namespace kul.forbes.helpers.test
{
    public static class RosenBrock
    {
        public static Vector<double> RosenBrockGradient(Vector<double> location)
        {
            double a = 1;
            double b = 100;

            var gradient = Vector<double>.Build.Dense(location.Count);

            // Matlab: df = @(x) [-2*(a-(b+1)*x(1)+b*x(2)); 2*b*(x(2)-x(1)) ]; 
            gradient[0] = -2 * (a - (b + 1) * location[0] + b * location[1]);
            gradient[1] = 2 * b * (location[1] - location[0]);

            return gradient;
        }

        public static double RosenBrockCost(Vector<double> location)
        {
            double a = 1;
            double b = 100;

            // Matlab: f =@(x) (a-x(1))^2 + b*(x(2)-x(1))^2; 
            return Math.Pow((a - location[0]), 2) + b * Math.Pow((location[1] - location[0]), 2);
        }
    }
}
