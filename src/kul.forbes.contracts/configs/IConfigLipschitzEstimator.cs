using System;
using System.Collections.Generic;
using System.Text;

namespace kul.forbes.contracts.configs
{
    public interface IConfigLipschitzEstimator
    {
        /// <summary>
        /// The lipschitz value is estimated over an delta of:
        /// delta= max{MinimumDelta,LipschitzSafetyValue*u_0}
        /// </summary>
        double LipschitzSafetyValue { get;  } 

        /// <summary>
        /// The lipschitz value is estimated over an delta of:
        /// delta= max{MinimumDelta,LipschitzSafetyValue*u_0}
        /// </summary>
        double MinimumDelta { get; }
    }
}
