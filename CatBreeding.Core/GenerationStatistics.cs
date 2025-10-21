// =================================================================
// CatBreeding.Core/GenerationStatistics.cs
// =================================================================
namespace CatBreeding.Core
{
    public class GenerationStatistics
    {
        public int GenerationNumber { get; set; }
        public double AverageFitness { get; set; }
        public double FitnessStdDev { get; set; }
        public double MinFitness { get; set; }
        public double MaxFitness { get; set; }
        public double AverageFitnessQuotient { get; set; }
        public Dictionary<int, double> FitnessPercentiles { get; set; } = new Dictionary<int, double>();
        public int PopulationSize { get; set; }
    }
}