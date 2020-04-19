using kul.forbes.contracts.configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace kul.forbes.contracts.configs
{
    public interface IConfigPanoc : IConfigLBFGS, IConfigProximalGradient, IConfigForwardBackwardEnvelop
    {
        /// <summary>
        /// The number of times the linesearch should backtrack.
        /// </summary>
        int FBEMaxIterations { get; }
    }
}
