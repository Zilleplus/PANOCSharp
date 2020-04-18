using kul.forbes.contracts;
using kul.forbes.entities;

namespace kul.forbes.helpers.domain
{
    public static class ProxLocationBuilder 
    {
        public static ProxLocation Build(
            Location location,
            double gamma,
            IProx prox,
            IFunction function)
        {
            var newLocation = location.Position - gamma * location.Cost.Gradient;
            return new ProxLocation(
                cost: function.Evaluate(newLocation),
                position: newLocation,
                constraint: prox.Prox(newLocation),
                gamma: gamma);
        }
    }
}
