using CatBreeding.Core.Analysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CatBreeding.Core.Tests.Analysis
{
    /// <summary>
    /// Obsahuje sadu unit testů pro statickou třídu FitnessConverter.
    /// Cílem je ověřit správnost matematické transformace hrubé "fitness"
    /// na standardizovaný kvocient podobný IQ.
    /// </summary>
    [TestClass]
    public class FitnessConverterTests
    {
        private const double Tolerance = 1e-9;

        /// <summary>
        /// Testuje základní případ, kdy je fitness jedince přesně na průměru populace.
        /// Očekávaný výsledek je kvocient 100.
        /// </summary>
        [TestMethod]
        public void ConvertToQuotient_FitnessIsAtMean_Returns100()
        {
            // Act
            double result = FitnessConverter.ConvertToQuotient(0.0);

            // Assert
            Assert.AreEqual(100.0, result, Tolerance, "Jedinec s průměrnou fitness by měl mít kvocient 100.");
        }

        /// <summary>
        /// Testuje případ, kdy je fitness jedince jednu směrodatnou odchylku nad průměrem.
        /// Očekávaný výsledek je kvocient 115 (100 + 1 * 15).
        /// </summary>
        [TestMethod]
        public void ConvertToQuotient_FitnessIsOneStdDevAboveMean_Returns115()
        {
            // Act
            double result = FitnessConverter.ConvertToQuotient(1.0);

            // Assert
            Assert.AreEqual(115.0, result, Tolerance, "Jedinec 1 směrodatnou odchylku nad průměrem by měl mít kvocient 115.");
        }

        /// <summary>
        /// Testuje případ, kdy je fitness jedince dvě směrodatné odchylky pod průměrem.
        /// Očekávaný výsledek je kvocient 70 (100 - 2 * 15).
        /// </summary>
        [TestMethod]
        public void ConvertToQuotient_FitnessIsTwoStdDevsBelowMean_Returns70()
        {
            // Act
            double result = FitnessConverter.ConvertToQuotient(-2.0);

            // Assert
            Assert.AreEqual(70.0, result, Tolerance, "Jedinec 2 směrodatné odchylky pod průměrem by měl mít kvocient 70.");
        }
    }
}
