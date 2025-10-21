// =================================================================
// CatBreeding.Core/Analysis/FitnessConverter.cs
// =================================================================
namespace CatBreeding.Core.Analysis
{
    /// <summary>
    /// Provides a method to convert raw fitness scores into a standardized quotient,
    /// analogous to an IQ score. This conversion is based on a theoretical
    /// global cat population where the average fitness is 0 and the standard deviation is 1.
    /// </summary>
    public static class FitnessConverter
    {
        private const double GLOBAL_POPULATION_MEAN = 0.0;
        private const double GLOBAL_POPULATION_STD_DEV = 1.0;
        private const double IQ_MEAN = 100.0;
        private const double IQ_STD_DEV = 15.0;

        /// <summary>
        /// Converts a raw fitness score to a standardized quotient.
        /// </summary>
        /// <param name="rawFitness">The raw fitness score, which is the average of trait values.</param>
        /// <returns>A standardized quotient (e.g., a score of 115 for a fitness of 1.0).</returns>
        public static double ConvertToQuotient(double rawFitness)
        {
            // The formula is based on the Z-score calculation, where the reference
            // population's parameters are constants.
            double zScore = (rawFitness - GLOBAL_POPULATION_MEAN) / GLOBAL_POPULATION_STD_DEV;
            return (zScore * IQ_STD_DEV) + IQ_MEAN;
        }
    }
}