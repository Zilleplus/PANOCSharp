using kul.forbes.contracts.configs;

namespace kul.forbes
{
    public class ConfigPanoc : IConfigPanoc
    {
        public ConfigPanoc(int problemDimension)
        {
            ProblemDimension = problemDimension;
        }
        public int CacheSize { get; set; } = 10;

        public int ProblemDimension { get; }

        public double LipschitzSafetyValue { get; set; } = 1e-6;

        public double Delta { get; set; } = 1e-12;

        public double SafetyValueLineSearch { get; set; } = 0.05;

        public double minGammaValue { get; set; } = 1e-15;

        public int FBEMaxIterations { get; set; }=10;
    }
}
