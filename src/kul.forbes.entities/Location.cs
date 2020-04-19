using MathNet.Numerics.LinearAlgebra;

namespace kul.forbes.entities
{
    public class Location
    {
        public Location(
            Vector<double> position,
            (double cost, Vector<double> Gradient) evaluated)
        {
            Position = position;
            Evaluated = evaluated;
        }

        public Location(Location loc)
        {
            Position = loc.Position;
            Evaluated = loc.Evaluated;
        }

        public Vector<double> Position { get; }
        public (double cost, Vector<double> Gradient) Evaluated { get; }
    }
}
