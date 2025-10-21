using CatBreeding.Core.Domain;
using CatBreeding.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatBreeding.Core.Tests.Services
{
    [TestClass]
    public class MeanFitnessCalculatorTests
    {
        /// <summary>
        /// Ověřuje, že kalkulátor fitness správně vypočítá průměrnou hodnotu
        /// pouze z těch znaků, které jsou specifikovány v nastavení simulace.
        /// Motivací je zajistit, že fitness - klíčová metrika pro šlechtění -
        /// je počítána korektně a že jsou ignorovány znaky, které pro fitness nemají být relevantní.
        /// </summary>
        [TestMethod]
        public void CalculateFitness_ShouldReturnMeanOfSpecifiedTraits()
        {
            // Arrange
            var options = new BreedingSimulationOptions
            {
                TotalTraitCount = 4,
                FitnessTraitIndices = Enumerable.Range(0, 2).ToList() // Pouze první 2 znaky se počítají do fitness
            };
            var calculator = new MeanTraitFitnessCalculator(options);
            var catTraits = new TraitVector(new double[] { 10, 20, 99, -50 }); // Průměr prvních dvou je (10+20)/2 = 15
            var cat = new Cat(System.Guid.NewGuid(), System.TimeSpan.Zero, Gender.Female, catTraits, true, null, null);

            var expectedFitness = 15.0;

            // Act
            var actualFitness = calculator.CalculateFitness(cat);

            // Assert
            Assert.AreEqual(expectedFitness, actualFitness, "Fitness should be the average of the first two traits only.");
        }
    }
}
