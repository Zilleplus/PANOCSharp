using kul.forbes.contracts;
using kul.forbes.contracts.configs;
using kul.forbes.entities;
using MathNet.Numerics.LinearAlgebra;

namespace kul.forbes.helpers
{
    public class ForwardBackwardEnvelop
    {
        private static Vector<double> Residual(ProximalGradient prox)  
            => ((prox.Location.Position - prox.ProxLocation.Position) / prox.ProxLocation.Gamma) ;

        /*
         * calculate the forward backward envelop using the internal gamma
         * Matlab cache.FBE = cache.fx + cache.gz - cache.gradfx(:)'*cache.FPR(:)
         * + (0.5/gam)*(cache.normFPR^2);
         */
        public static double Calculate(ProximalGradient proxGradient)
        {
            (var fx, var df) = proxGradient.Location.Evaluated;
            var direction = proxGradient.Location.Position - proxGradient.ProxLocation.Position;

            return fx
                + proxGradient.ProxLocation.Constraint.Cost
                - df.DotProduct(direction)
                + (1 / (proxGradient.ProxLocation.Gamma * 2))*(direction.DotProduct(direction));
        }
    }
}
