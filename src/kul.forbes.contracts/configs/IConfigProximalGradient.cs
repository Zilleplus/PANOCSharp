using System;
using System.Collections.Generic;
using System.Text;

namespace kul.forbes.contracts.configs
{
    public interface IConfigProximalGradient : IConfigLipschitzEstimator
    {
        /// <summary>
        /// The linesearch condition should be :
        /// fNew > f - df.DotProduct(df) + (1 ) / (2 * newLocation.Gamma) * (direction.DotProduct(direction))
        ///
        /// In reality it is:
        /// fNew > f - df.DotProduct(df)
        ///     + (1 - safetyValueLineSearch) / (2 * newLocation.Gamma) * directionSquaredNorm
        ///     + 1e-6 * f;
        /// </summary>
        double SafetyValueLineSearch { get; }

        /// <summary>
        /// The linesearch parameter gamma can never be smaller then this.
        /// </summary>
        double minGammaValue { get; }
    }
}
