using System;
using System.Collections.Generic;
using System.Text;

namespace kul.forbes.contracts.configs
{
    public interface IConfigLipschitzEstimator
    {
        double LipschitzSafetyValue { get;  } 

        double Delta { get; }
    }
}
