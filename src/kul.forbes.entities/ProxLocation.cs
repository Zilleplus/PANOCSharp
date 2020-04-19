using MathNet.Numerics.LinearAlgebra;


namespace kul.forbes.entities
{
    public class ProxLocation : Location
    {
        public ProxLocation(
            Vector<double> position,
            (double Value, Vector<double> Gradient) evaluated,
            double gamma, 
            (double Cost, Vector<double> Value) constraint) : base(position,evaluated)
        {
            Gamma = gamma;
            Constraint = constraint;
        }

        public ProxLocation(
            Location location,
            double gamma, 
            (double Cost, Vector<double> Value) constraint) : base(location)
        {
            Gamma = gamma;
            Constraint = constraint;
        }

        public double Gamma { get; }

        public (double Cost, Vector<double> Value) Constraint { get; }
    }
}
