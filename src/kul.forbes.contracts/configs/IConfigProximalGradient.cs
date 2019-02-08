using System;
using System.Collections.Generic;
using System.Text;

namespace kul.forbes.contracts.configs
{
    public interface IConfigProximalGradient : IConfigLipschitzEstimator
    {
        double SafetyValueLineSearch { get; }
    }
}
