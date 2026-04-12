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

        // --- Human Equivalent Model Anchor Points ---
        /// <summary>
        /// The anchor point on the human IQ scale that corresponds to an average cat's intelligence.
        /// Based on the cognitive abilities of a 2-year-old human, estimated to be
        /// 5 standard deviations below the adult human mean (100 - 5 * 15 = 25).
        /// </summary>
        private const double CAT_IQ_ANCHOR_ON_HUMAN_SCALE = 25.0;

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

        /// <summary>
        /// Converts a cat's raw fitness score to a human-equivalent IQ.
        /// This provides a relatable context for the simulation's output.
        /// The model assumes an average cat (fitness = 0.0) has the cognitive ability
        /// of a 2-year-old human, which is anchored at an estimated IQ of 25 on the adult scale.
        /// </summary>
        /// <param name="catRawFitness">The raw fitness score from the simulation.</param>
        /// <returns>An estimated human-equivalent IQ score.</returns>
        public static double ConvertToHumanEquivalentIQ(double catRawFitness)
        {
            // Each standard deviation in cat fitness is mapped to a standard deviation in human IQ (15 points).
            return CAT_IQ_ANCHOR_ON_HUMAN_SCALE + (catRawFitness * IQ_STD_DEV);
        }
    }
}