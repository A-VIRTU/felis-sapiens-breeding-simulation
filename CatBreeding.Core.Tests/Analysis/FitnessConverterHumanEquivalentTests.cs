using CatBreeding.Core.Analysis;

namespace CatBreeding.Core.Tests.Analysis
{
    [TestClass]
    public class FitnessConverterHumanEquivalentTests
    {
        private const double DELTA = 1e-9;

        /// <summary>
        /// Tests the anchor point of the model. An average cat (fitness = 0.0)
        /// should map to the cognitive level of a 2-year-old, which we've estimated at IQ 25.
        /// </summary>
        [TestMethod]
        public void ConvertToHumanEquivalentIQ_AverageCat_ReturnsAnchorIq()
        {
            // Arrange
            double averageCatFitness = 0.0;
            double expectedHumanEquivalentIq = 25.0;

            // Act
            double actualIq = FitnessConverter.ConvertToHumanEquivalentIQ(averageCatFitness);

            // Assert
            Assert.AreEqual(expectedHumanEquivalentIq, actualIq, DELTA);
        }

        /// <summary>
        /// Tests a "gifted" cat, two standard deviations above the cat mean.
        /// The result should be significantly higher than the anchor point.
        /// </summary>
        [TestMethod]
        public void ConvertToHumanEquivalentIQ_GiftedCat_ReturnsCorrectlyScaledIq()
        {
            // Arrange
            double giftedCatFitness = 2.0; // 2 sigma
            double expectedHumanEquivalentIq = 25.0 + 2.0 * 15.0; // 55

            // Act
            double actualIq = FitnessConverter.ConvertToHumanEquivalentIQ(giftedCatFitness);

            // Assert
            Assert.AreEqual(expectedHumanEquivalentIq, actualIq, DELTA);
        }

        /// <summary>
        /// Tests a "less gifted" cat, one standard deviation below the cat mean.
        /// </summary>
        [TestMethod]
        public void ConvertToHumanEquivalentIQ_LessGiftedCat_ReturnsCorrectlyScaledIq()
        {
            // Arrange
            double lessGiftedCatFitness = -1.0; // -1 sigma
            double expectedHumanEquivalentIq = 25.0 + -1.0 * 15.0; // 10

            // Act
            double actualIq = FitnessConverter.ConvertToHumanEquivalentIQ(lessGiftedCatFitness);

            // Assert
            Assert.AreEqual(expectedHumanEquivalentIq, actualIq, DELTA);
        }
    }
}

