using MathNet.Numerics.LinearAlgebra;

namespace kul.forbes.helpers.contracts
{
    public interface IProximalGradient
    {
        double DoProximalStep(Vector<double> location);
        Vector<double> GetStep(Vector<double> location);
    }
}