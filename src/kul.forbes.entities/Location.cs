using MathNet.Numerics.LinearAlgebra;

namespace kul.forbes.entities
{
    public class Location
    {
        public (double Value, Vector<double> Gradient) Cost { get; }

        public Vector<double> Position { get;  }

        public Location(
            (double Value, Vector<double> Gradient) cost,
            Vector<double> position)
        {
            Cost = cost;
            Position = position;
        }
    }
}
