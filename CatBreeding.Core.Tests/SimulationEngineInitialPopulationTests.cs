using Microsoft.VisualStudio.TestTools.UnitTesting;
using CatBreeding.Core.Services;
using CatBreeding.Core.Domain;
using System.Collections.Generic;
using System.Linq;
using System;
using Moq;

namespace CatBreeding.Core.Tests
{
    [TestClass]
    public class SimulationEngineInitialPopulationTests
    {
        /// <summary>
        /// Tento test ověřuje, že dynamická adjustace směrodatné odchylky sub-znaků
        /// skutečně vede k tomu, že výsledná fitness v počáteční populaci má
        /// směrodatnou odchylku blízko cílové hodnoty 1.0.
        /// </summary>
        [TestMethod]
        public void InitialPopulation_HasCorrectFitnessStandardDeviation_AfterDynamicAdjustment()
        {
            // Arrange
            var options = new BreedingSimulationOptions
            {
                FitnessTraitIndices = Enumerable.Range(0, 100).ToList(),
                InitialPopulationSize = 1000,
                InitialPopulationSelectionPoolSize = 1000,
                // OPRAVA 1: Explicitně nastavíme, že nechceme simulovat žádné další generace.
                // Tím se test zaměří POUZE na statistiky nulté generace.
                TotalGenerationsToSimulate = 0
            };

            var random = new Random();
            var randomProvider = new GaussianRandomProvider(random);
            var fitnessCalculator = new MeanTraitFitnessCalculator(options);
            var breedingStrategyMock = new Mock<IBreedingStrategy>();

            // OPRAVA 2: I když by se strategie neměla volat, pro robustnost testu
            // ji nastavíme tak, aby vracela prázdný, ale validní výsledek.
            // Tím předejdeme NullReferenceException, pokud by se logika enginu změnila.
            breedingStrategyMock
                .Setup(s => s.Apply(It.IsAny<Cattery>(), It.IsAny<TimeSpan>()))
                .Returns(new BreedingResult(Array.Empty<Cat>(), Array.Empty<Cat>(), Array.Empty<Cat>())); // Vracíme prázdný výsledek

            var engine = new SimulationEngine(options, fitnessCalculator, breedingStrategyMock.Object, randomProvider);

            // Act
            var report = engine.RunSingleSimulation();
            var initialStats = report.StatisticsByGeneration.First();
            double actualStdDev = initialStats.FitnessStdDev;

            // Assert
            Assert.AreEqual(1, report.StatisticsByGeneration.Count, "Report by měl obsahovat statistiky pouze pro nultou generaci.");
            Assert.AreEqual(1.0, actualStdDev, 0.1,
                $"Směrodatná odchylka fitness by se měla blížit cílové hodnotě 1.0, ale byla {actualStdDev:F4}.");
        }
    }

    // Pomocná reálná implementace IRandomProvider pro tento test
    public class GaussianRandomProvider : IRandomProvider
    {
        private readonly Random _random;
        public GaussianRandomProvider(Random random) { _random = random; }
        public double NextDouble() => _random.NextDouble();
        public int Next(int max) => _random.Next(max);
        public double NextGaussian()
        {
            double u1 = 1.0 - _random.NextDouble();
            double u2 = 1.0 - _random.NextDouble();
            return Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
        }
    }
}

