using kul.forbes.contracts;
using kul.forbes.entities;

namespace kul.forbes.helpers.domain
{
    public class ProxLocationBuilder : IBuilder<Location,double,ProxLocation>
    {
        private readonly IFunction function;
        private readonly IProx prox;

        public ProxLocationBuilder(IFunction function,IProx prox)
        {
            this.function = function;
            this.prox = prox;
        }

        public ProxLocation Build(Location location,double gamma)
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
