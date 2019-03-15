using kul.forbes.contracts;
using kul.forbes.entities;
using MathNet.Numerics.LinearAlgebra;

namespace kul.forbes.helpers.domain
{
    public class ResidualCalculator : ICalculator<ProximalGradient, Vector<double>>
    {
        public Vector<double> Calculate(ProximalGradient proximalGradient)
        => (1 / proximalGradient.ProxLocation.Gamma) * 
            (proximalGradient.ProxLocation.Position - proximalGradient.Location.Position);
    }
}
