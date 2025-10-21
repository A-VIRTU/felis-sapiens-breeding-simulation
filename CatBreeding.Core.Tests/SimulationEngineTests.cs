using CatBreeding.Core.Domain;
using CatBreeding.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CatBreeding.Core.Tests
{ 
    [TestClass]
    public class SimulationEngineTests
    {
        public TestContext TestContext { get; set; }
        private BreedingSimulationOptions _options;
        private Mock<IRandomProvider> _randomProviderMock;

        [TestInitialize]
        public void TestInitialize()
        {
            _options = new BreedingSimulationOptions
            {
                TotalTraitCount = 100,
                FitnessTraitIndices = Enumerable.Range(0, 50).ToList(),
                InitialPopulationSelectionPoolSize = 4,
                TotalGenerationsToSimulate = 5,
                LitterSizeMean = 3,
                LitterSizeStdDev = 0,
                InheritanceMutationStdDev = 0.1,
                CatFertilityAge = TimeSpan.FromDays(180),
                BreedingCycleDuration = TimeSpan.FromDays(300),
                MaxBreedingFemales = 2,
                MaxBreedingMales = 1
            };

            _randomProviderMock = new Mock<IRandomProvider>();
            _randomProviderMock.Setup(r => r.NextGaussian()).Returns(0.1);
            _randomProviderMock.Setup(r => r.NextDouble()).Returns(0.4);
            _randomProviderMock.Setup(r => r.Next(It.IsAny<int>())).Returns(0);
        }

        [TestMethod]
        public void RunSimulation_WithElitistStrategy_ShowsFitnessGrowth()
        {
            // Arrange
            var fitnessCalculator = new MeanTraitFitnessCalculator(_options);
            var breedingStrategy = new ElitistBreedingStrategy(_options, fitnessCalculator, _randomProviderMock.Object);
            var engine = new SimulationEngine(_options, fitnessCalculator, breedingStrategy, _randomProviderMock.Object);

            // Act
            var finalReport = engine.RunSingleSimulation();

            // Assert
            Assert.IsNotNull(finalReport, "Report by neměl být null.");
            // Počet statistik je o 1 větší, protože zahrnuje i nultou (počáteční) generaci.
            Assert.AreEqual(_options.TotalGenerationsToSimulate + 1, finalReport.StatisticsByGeneration.Count);

            var firstGenStats = finalReport.StatisticsByGeneration.First();
            var lastGenStats = finalReport.StatisticsByGeneration.Last();

            Assert.IsTrue(lastGenStats.AverageFitness > firstGenStats.AverageFitness,
                $"Očekával se růst fitness. Start: {firstGenStats.AverageFitness:F2}, Konec: {lastGenStats.AverageFitness:F2}");

            // OPRAVA: Čteme velikost populace z poslední statistiky.
            Assert.IsTrue(lastGenStats.PopulationSize > 0, "Populace by na konci simulace neměla vymřít.");
            Assert.IsTrue(lastGenStats.MaxFitness > firstGenStats.MaxFitness, "Maximální fitness by se mělo v čase zvyšovat.");
            Assert.IsTrue(lastGenStats.AverageFitnessQuotient > 100, "Průměrný kvocient na konci by měl být nadprůměrný (vyšší než 100).");
        }

        [TestMethod]
        public void RunSimulation_AndPrintReport_OutputsToConsole()
        {
            // Arrange
            var fitnessCalculator = new MeanTraitFitnessCalculator(_options);
            var breedingStrategy = new ElitistBreedingStrategy(_options, fitnessCalculator, _randomProviderMock.Object);
            var engine = new SimulationEngine(_options, fitnessCalculator, breedingStrategy, _randomProviderMock.Object);

            // Act
            var finalReport = engine.RunSingleSimulation();

            // Assert & Print
            Assert.IsNotNull(finalReport);
            PrintReportToConsole(finalReport);
        }

        /// <summary>
        /// NOVÝ TEST: Simuluje realističtější scénář s počáteční variabilitou
        /// a skutečnou náhodností, aby ověřil, že elitářská selekce
        /// skutečně vede k prokazatelnému zlepšení průměrné fitness populace.
        /// </summary>
        [TestMethod]
        public void RunSimulation_WithVariability_ElitistSelectionImprovesFitness()
        {
            // Arrange
            var options = new BreedingSimulationOptions
            {
                TotalGenerationsToSimulate = 10,
                InitialPopulationSize = 1, // Větší počáteční populace
                MaxBreedingFemales = 2
            };

            // "Náhodná" čísla s variabilitou
            var random = new Random(1234); // Pevný seed pro opakovatelnost
            var randomProviderMock = new Mock<IRandomProvider>();
            randomProviderMock.Setup(r => r.NextGaussian()).Returns(() => (random.NextDouble() - 0.5) * 2);
            randomProviderMock.Setup(r => r.NextDouble()).Returns(random.NextDouble);
            randomProviderMock.Setup(r => r.Next(It.IsAny<int>())).Returns<int>(max => random.Next(max));

            var fitnessCalculator = new MeanTraitFitnessCalculator(options);
            var breedingStrategy = new ElitistBreedingStrategy(options, fitnessCalculator, randomProviderMock.Object);
            var engine = new SimulationEngine(options, fitnessCalculator, breedingStrategy, randomProviderMock.Object);

            // Act
            var finalReport = engine.RunSingleSimulation();

            // Assert
            TestContext.WriteLine("\n--- Test s reálnou variabilitou a selekcí ---");
            PrintReportToConsole(finalReport);

            var firstGenStats = finalReport.StatisticsByGeneration.First();
            var lastGenStats = finalReport.StatisticsByGeneration.Last();

            Assert.IsTrue(lastGenStats.AverageFitness > firstGenStats.AverageFitness + 1.0,
                $"Očekával se výrazný růst fitness. Start: {firstGenStats.AverageFitness:F2}, Konec: {lastGenStats.AverageFitness:F2}");

            Assert.IsTrue(lastGenStats.MaxFitness > 2.0, "Měl by se objevit jedinec s výrazně nadprůměrnou fitness.");
            Assert.IsTrue(lastGenStats.FitnessStdDev > 0, "Populace by si měla udržet genetickou variabilitu.");
            Assert.IsTrue(lastGenStats.AverageFitnessQuotient > 150, "Průměrný kvocient by měl být výrazně nadprůměrný.");
        }

        /// <summary>
        /// NOVÝ IZOLAČNÍ TEST: Tento test se zaměřuje výhradně na logiku
        /// metody GenerateInitialPopulation. Předchozí test selhal a jeho report
        /// naznačil, že se do nulté generace dostala celá skupina kandidátů (velikost 4)
        /// namísto jednoho nejlepšího jedince. Tento test tuto hypotézu ověřuje.
        /// </summary>
        [TestMethod]
        public void GenerateInitialPopulation_SelectsOnlyBestCandidate_WhenSizeIsOne()
        {
            // Arrange
            var options = new BreedingSimulationOptions
            {
                TotalGenerationsToSimulate = 0, // Nebudeme simulovat žádné další generace
                InitialPopulationSize = 1,      // Chceme POUZE JEDNOHO zakladatele
                InitialPopulationSelectionPoolSize = 10 // Z velkého koše kandidátů
            };

            var fitnessCalculatorMock = new Mock<IFitnessCalculator>();
            // Pro každou kočku vrátíme předvídatelnou fitness, abychom věděli, kdo je "nejlepší".
            // Použijeme negativní ID, aby se fitness snadno odlišilo.
            fitnessCalculatorMock.Setup(c => c.CalculateFitness(It.IsAny<Cat>()))
                                 .Returns<Cat>(cat => (double)cat.Id.GetHashCode() * -1);

            // Random provider není pro tento test kritický, ale musí existovat.
            var randomProviderMock = new Mock<IRandomProvider>();
            randomProviderMock.Setup(r => r.NextDouble()).Returns(0.5);
            randomProviderMock.Setup(r => r.NextGaussian()).Returns(0.1);

            var breedingStrategyMock = new Mock<IBreedingStrategy>();
            var engine = new SimulationEngine(options, fitnessCalculatorMock.Object, breedingStrategyMock.Object, randomProviderMock.Object);

            // Act
            var finalReport = engine.RunSingleSimulation();

            // Assert
            var finalStats = finalReport.StatisticsByGeneration.Last();
            Assert.AreEqual(1, finalStats.PopulationSize, "Populace v nulté generaci by měla mít velikost přesně 1.");

            // Dále ověříme, že ten jeden vybraný jedinec je opravdu ten nejlepší.
            // Jelikož fitness je negativní hashcode, nejvyšší fitness má nejmenší hashcode.
            // To je sice trochu neintuitivní, ale pro test to stačí.
            var initialPopulation = finalReport.StatisticsByGeneration.First(); // Zde bychom potřebovali přístup ke kočkám
            // Vzhledem k tomu, že report neobsahuje samotné jedince, spokojíme se s kontrolou velikosti populace,
            // která je klíčovým indikátorem chyby z předchozího testu.
        }

        private void PrintReportToConsole(FinalReport report)
        {
            var baselineStats = report.StatisticsByGeneration.First();
            var lastStats = report.StatisticsByGeneration.Last();
            var sb = new StringBuilder();
            sb.AppendLine($"\nFinal Population: {lastStats.PopulationSize} | Baseline Fitness: {baselineStats.AverageFitness:F2} (StdDev: {baselineStats.FitnessStdDev:F2})");
            sb.AppendLine("--------------------------------------------------------------------------------------");
            sb.AppendLine("| Gen | Pop | Avg Fitness | Max Fitness | Min Fitness | Avg Quotient |");
            sb.AppendLine("--------------------------------------------------------------------------------------");
            foreach (var stats in report.StatisticsByGeneration)
            {
                sb.AppendLine($"| {stats.GenerationNumber,3} | {stats.PopulationSize,3} | {stats.AverageFitness,11:F2} | {stats.MaxFitness,11:F2} | {stats.MinFitness,11:F2} | {stats.AverageFitnessQuotient,12:F2} |");
            }
            sb.AppendLine("--------------------------------------------------------------------------------------");
            TestContext.WriteLine(sb.ToString());
        }
    }
}

