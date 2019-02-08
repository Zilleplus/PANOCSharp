using System;
using System.Collections.Generic;
using System.Text;

namespace kul.forbes.contracts.configs
{
    public interface IConfigLBFGS
    {
        int CacheSize { get; }
        int ProblemDimension { get; }
    }
}
