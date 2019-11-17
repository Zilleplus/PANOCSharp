using kul.forbes.contracts.configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace kul.forbes.domain
{
    public class ConfigPanoc : IConfigPanoc
    {
        public ConfigPanoc(int problemDimension)
        {
            ProblemDimension = problemDimension;
        }
        public int CacheSize { get; set; } = 10;

        public int ProblemDimension { get; }

        public double LipschitzSafetyValue { get; set; } = 0;

        public double Delta { get; set; } = 0;

        public double SafetyValueLineSearch { get; set; } = 0;

        public bool EnableLogging { get; set; } = false;
    }
}
