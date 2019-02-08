using kul.forbes.contracts.configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace kul.forbes.domain
{
    public class ConfigPanoc : IConfigPanoc
    {
        public int CacheSize => throw new NotImplementedException();

        public int ProblemDimension => throw new NotImplementedException();

        public double LipschitzSafetyValue => throw new NotImplementedException();

        public double Delta => throw new NotImplementedException();

        public double SafetyValueLineSearch => throw new NotImplementedException();
    }
}
