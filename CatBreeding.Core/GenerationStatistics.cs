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

        /// <summary>
        /// The standardized quotient of the weakest individual in this generation.
        /// </summary>
        public double MinFitnessQuotient { get; set; }

        /// <summary>
        /// The standardized quotient of the strongest individual in this generation.
        /// </summary>
        public double MaxFitnessQuotient { get; set; }

        public int PopulationSize { get; set; }
    }
}