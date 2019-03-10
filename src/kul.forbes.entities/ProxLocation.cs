using MathNet.Numerics.LinearAlgebra;

namespace kul.forbes.entities
{
    public class ProxLocation : Location
    {
        public ProxLocation(
            (double Value, Vector<double> Gradient) cost,
            Vector<double> position,
            double gamma, 
            (double Cost, Vector<double> Value) constraint) : base(cost,position)
        {
            Gamma = gamma;
            Constraint = constraint;
        }

        public double Gamma { get; }

        public (double Cost, Vector<double> Value) Constraint { get; }
    }
}
