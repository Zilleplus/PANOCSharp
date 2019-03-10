using MathNet.Numerics.LinearAlgebra;

namespace kul.forbes.entities
{
    public class ProximalGradient
    {
        public Location Location { get; }

        public ProxLocation ProxLocation { get; }

        public ProximalGradient(
            Location location,
            ProxLocation proxLocation)
        {
            Location = location;
            ProxLocation = proxLocation;
        }
    }
}
