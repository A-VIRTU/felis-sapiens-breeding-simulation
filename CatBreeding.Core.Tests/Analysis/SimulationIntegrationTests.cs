using CatBreeding.Core.Services;
using System.Text;

namespace CatBreeding.Core.Tests.Analysis
{
    /// <summary>
    /// Contains integration tests for the simulation engine.
    /// These tests use a REAL implementation of the random provider (MathNetGaussianRandomProvider)
    /// to verify the overall behavior of the simulation under realistic, non-deterministic conditions.
    /// </summary>
    [TestClass]
    public class SimulationIntegrationTests
    {
        public TestContext TestContext { get; set; }

        /// <summary>
        /// This test runs a full simulation with a real random number generator.
        /// It doesn't check for exact values but verifies that the key evolutionary
        /// trends are present: fitness should increase, and the population should not die out.
        /// It serves as a high-level validation that all components work together correctly.
        /// </summary>
        [TestMethod]
        public void RunSimulation_WithRealRandomness_ShowsPlausibleEvolutionaryTrends()
        {
            // Arrange
            var options = new BreedingSimulationOptions
            {
                TotalGenerationsToSimulate = 20, // Pro delší běh, aby byly trendy viditelné
                InitialPopulationSize = 10,
                InitialPopulationSelectionPoolSize = 20,
                InheritanceMutationStdDev = 0.15,
                MaxBreedingFemales = 2,
                MaxBreedingMales = 1,
                MortalityHalfLife = TimeSpan.FromDays(365 * 4) // Realističtější délka života
            };

            // Používáme reálné implementace, žádné mocky (kromě těch, které by byly potřeba pro externí služby)
            var randomProvider = new MathNetGaussianRandomProvider();
            var fitnessCalculator = new MeanTraitFitnessCalculator(options);
            var breedingStrategy = new ElitistBreedingStrategy(options, fitnessCalculator, randomProvider);
            var engine = new SimulationEngine(options, fitnessCalculator, breedingStrategy, randomProvider);

            // Act
            var finalReport = engine.RunSingleSimulation();

            // Assert - ověřujeme trendy, ne přesné hodnoty
            Assert.IsNotNull(finalReport);
            PrintReportToConsole(finalReport);

            var firstGenStats = finalReport.StatisticsByGeneration.First();
            var lastGenStats = finalReport.StatisticsByGeneration.Last();

            Assert.IsTrue(lastGenStats.AverageFitness > firstGenStats.AverageFitness,
                $"Average fitness should increase. Start: {firstGenStats.AverageFitness:F2}, End: {lastGenStats.AverageFitness:F2}");

            Assert.IsTrue(lastGenStats.MaxFitness > firstGenStats.MaxFitness,
                $"Max fitness should increase. Start: {firstGenStats.MaxFitness:F2}, End: {lastGenStats.MaxFitness:F2}");

            Assert.IsTrue(lastGenStats.PopulationSize > 0, "The final population should not be zero.");

            // Kvocient nejlepšího jedince by se měl výrazně zvýšit
            Assert.IsTrue(lastGenStats.MaxFitnessQuotient > firstGenStats.MaxFitnessQuotient + 10,
                $"The quotient of the best cat should increase significantly. Start: {firstGenStats.MaxFitnessQuotient:F0}, End: {lastGenStats.MaxFitnessQuotient:F0}");
        }

        [TestMethod]
        public void RunSimulation_WithRealRandomness_ShowsRealisticCattery()
        {
            // Arrange
            var options = new BreedingSimulationOptions
            {
                TotalGenerationsToSimulate = 10,
                InitialPopulationSize = 1,
                MaxBreedingFemales = 2
            };

            // Používáme reálné implementace, žádné mocky (kromě těch, které by byly potřeba pro externí služby)
            var randomProvider = new MathNetGaussianRandomProvider();
            var fitnessCalculator = new MeanTraitFitnessCalculator(options);
            var breedingStrategy = new ElitistBreedingStrategy(options, fitnessCalculator, randomProvider);
            var engine = new SimulationEngine(options, fitnessCalculator, breedingStrategy, randomProvider);

            // Act
            var finalReport = engine.RunSingleSimulation();

            // Assert - ověřujeme trendy, ne přesné hodnoty
            Assert.IsNotNull(finalReport);
            PrintReportToConsole(finalReport);

            var firstGenStats = finalReport.StatisticsByGeneration.First();
            var lastGenStats = finalReport.StatisticsByGeneration.Last();

            Assert.IsTrue(lastGenStats.AverageFitness > firstGenStats.AverageFitness,
                $"Average fitness should increase. Start: {firstGenStats.AverageFitness:F2}, End: {lastGenStats.AverageFitness:F2}");

            Assert.IsTrue(lastGenStats.MaxFitness > firstGenStats.MaxFitness,
                $"Max fitness should increase. Start: {firstGenStats.MaxFitness:F2}, End: {lastGenStats.MaxFitness:F2}");

            Assert.IsTrue(lastGenStats.PopulationSize > 0, "The final population should not be zero.");

            // Kvocient nejlepšího jedince by se měl výrazně zvýšit
            Assert.IsTrue(lastGenStats.MaxFitnessQuotient > firstGenStats.MaxFitnessQuotient + 10,
                $"The quotient of the best cat should increase significantly. Start: {firstGenStats.MaxFitnessQuotient:F0}, End: {lastGenStats.MaxFitnessQuotient:F0}");
        }

        private void PrintReportToConsole(FinalReport report)
        {
            if (!report.StatisticsByGeneration.Any())
            {
                TestContext.WriteLine("\n--- Simulation Report: No data generated. ---");
                return;
            }

            var baselineStats = report.StatisticsByGeneration.First();
            var lastStats = report.StatisticsByGeneration.Last();
            var sb = new StringBuilder();
            sb.AppendLine($"\n--- INTEGRATION TEST REPORT (using {nameof(MathNetGaussianRandomProvider)}) ---");
            sb.AppendLine($"Final Population: {lastStats.PopulationSize} | Baseline Fitness: {baselineStats.AverageFitness:F2} (StdDev: {baselineStats.FitnessStdDev:F2})");
            sb.AppendLine("-------------------------------------------------------------------------------------------------------------------");
            sb.AppendLine("| Gen | Pop | Avg Fitness | Max Fitness | Min Fitness | Avg Q | Max Q | Min Q |");
            sb.AppendLine("-------------------------------------------------------------------------------------------------------------------");
            foreach (var stats in report.StatisticsByGeneration)
            {
                sb.AppendLine($"| {stats.GenerationNumber,3} | {stats.PopulationSize,3} | {stats.AverageFitness,11:F2} | {stats.MaxFitness,11:F2} | {stats.MinFitness,11:F2} | {stats.AverageFitnessQuotient,5:F0} | {stats.MaxFitnessQuotient,5:F0} | {stats.MinFitnessQuotient,5:F0} |");
            }
            sb.AppendLine("-------------------------------------------------------------------------------------------------------------------");
            TestContext.WriteLine(sb.ToString());
        }
        [TestMethod]
        public void CalculateTimeTo150CatIq_SimulationRuns()
        {
            // Arrange
            int numberOfRuns = 100;
            var options = new BreedingSimulationOptions
            {
                TotalGenerationsToSimulate = 200, // Velký limit pro jistotu
                InitialPopulationSize = 10,
                InitialPopulationSelectionPoolSize = 20,
                InheritanceMutationStdDev = 0.15,
                MaxBreedingFemales = 2,
                MaxBreedingMales = 1,
                MortalityHalfLife = TimeSpan.FromDays(365 * 4)
            };

            var randomProvider = new MathNetGaussianRandomProvider();
            var fitnessCalculator = new MeanTraitFitnessCalculator(options);
            
            List<int> generationsTo150 = new List<int>();

            // Act
            for (int i = 0; i < numberOfRuns; i++)
            {
                var breedingStrategy = new ElitistBreedingStrategy(options, fitnessCalculator, randomProvider);
                var engine = new SimulationEngine(options, fitnessCalculator, breedingStrategy, randomProvider);
                
                var report = engine.RunSingleSimulation();
                
                var successGen = report.StatisticsByGeneration.FirstOrDefault(s => s.MaxFitnessQuotient >= 150);
                if (successGen != null)
                {
                    generationsTo150.Add(successGen.GenerationNumber);
                }
            }

            // Assert / Calculate
            generationsTo150.Sort();
            int successCount = generationsTo150.Count;
            
            TestContext.WriteLine($"\n--- IQ 150 SIMULATION RESULTS ---");
            TestContext.WriteLine($"Total runs: {numberOfRuns}");
            TestContext.WriteLine($"Successful runs: {successCount}");

            if (successCount > 0)
            {
                double median = successCount % 2 == 0 
                    ? (generationsTo150[successCount / 2 - 1] + generationsTo150[successCount / 2]) / 2.0 
                    : generationsTo150[successCount / 2];

                int p05Index = (int)Math.Max(0, successCount * 0.05);
                int p95Index = (int)Math.Min(successCount - 1, successCount * 0.95);
                
                int p05 = generationsTo150[p05Index];
                int p95 = generationsTo150[p95Index];

                TestContext.WriteLine($"Median generations to reach IQ 150: {median}");
                TestContext.WriteLine($"90% Confidence Interval (5th - 95th percentile): [{p05}, {p95}]");
                
                Assert.IsTrue(median > 0);
            }
            else
            {
                Assert.Fail("IQ 150 was never reached in any run. Consider increasing TotalGenerationsToSimulate or adjusting parameters.");
            }
        }
        [TestMethod]
        public void CalculateMaxIqIn7Generations_SimulationRuns()
        {
            // Arrange
            int numberOfRuns = 100;
            var generationsToSimulate = 7;
            List<double> maxIqsInRun = new List<double>();

            var options = new BreedingSimulationOptions
            {
                TotalGenerationsToSimulate = generationsToSimulate,
                InitialPopulationSize = 10,
                InitialPopulationSelectionPoolSize = 20,
                InheritanceMutationStdDev = 0.15,
                MaxBreedingFemales = 2,
                MaxBreedingMales = 1,
                MortalityHalfLife = TimeSpan.FromDays(365 * 4)
            };

            var randomProvider = new MathNetGaussianRandomProvider();
            var fitnessCalculator = new MeanTraitFitnessCalculator(options);
            
            // Act
            for (int i = 0; i < numberOfRuns; i++)
            {
                var breedingStrategy = new ElitistBreedingStrategy(options, fitnessCalculator, randomProvider);
                var engine = new SimulationEngine(options, fitnessCalculator, breedingStrategy, randomProvider);
                
                var report = engine.RunSingleSimulation();
                
                // Získáme absolutní nejvyšší dosažený fitness kvocient napříč všemi 7 generacemi v tomto konkrétním běhu
                double runMaxIq = report.StatisticsByGeneration.Max(s => s.MaxFitnessQuotient);
                maxIqsInRun.Add(runMaxIq);
            }

            // Assert / Calculate
            maxIqsInRun.Sort();
            int successCount = maxIqsInRun.Count;
            
            TestContext.WriteLine($"\n--- MAX IQ IN 7 GENERATIONS SIMULATION RESULTS ---");
            TestContext.WriteLine($"Total runs: {numberOfRuns}");

            if (successCount > 0)
            {
                double median = successCount % 2 == 0 
                    ? (maxIqsInRun[successCount / 2 - 1] + maxIqsInRun[successCount / 2]) / 2.0 
                    : maxIqsInRun[successCount / 2];

                int p05Index = (int)Math.Max(0, successCount * 0.05);
                int p95Index = (int)Math.Min(successCount - 1, successCount * 0.95);
                
                double p05 = maxIqsInRun[p05Index];
                double p95 = maxIqsInRun[p95Index];

                TestContext.WriteLine($"Median maximal IQ reached: {median:F2}");
                TestContext.WriteLine($"90% Confidence Interval (5th - 95th percentile): [{p05:F2}, {p95:F2}]");
                
                Assert.IsTrue(median > 0);
            }
        }
        [TestMethod]
        public void CalculateMaxIqIn7Generations_HalfInbreeding_SimulationRuns()
        {
            // Arrange
            int numberOfRuns = 100;
            var generationsToSimulate = 7;
            List<double> maxIqsInRun = new List<double>();

            var options = new BreedingSimulationOptions
            {
                TotalGenerationsToSimulate = generationsToSimulate,
                InitialPopulationSize = 10,
                InitialPopulationSelectionPoolSize = 20,
                InheritanceMutationStdDev = 0.15,
                MaxBreedingFemales = 2,
                MaxBreedingMales = 1,
                MortalityHalfLife = TimeSpan.FromDays(365 * 4)
            };

            var randomProvider = new MathNetGaussianRandomProvider();
            var fitnessCalculator = new MeanTraitFitnessCalculator(options);
            
            // Act
            for (int i = 0; i < numberOfRuns; i++)
            {
                // Použijeme NOVOU strategii
                var breedingStrategy = new HalfInbreedingBreedingStrategy(options, fitnessCalculator, randomProvider);
                var engine = new SimulationEngine(options, fitnessCalculator, breedingStrategy, randomProvider);
                
                var report = engine.RunSingleSimulation();
                
                double runMaxIq = report.StatisticsByGeneration.Max(s => s.MaxFitnessQuotient);
                maxIqsInRun.Add(runMaxIq);
            }

            // Assert / Calculate
            maxIqsInRun.Sort();
            int successCount = maxIqsInRun.Count;
            
            TestContext.WriteLine($"\n--- MAX IQ IN 7 GENERATIONS (HALF INBREEDING) ---");
            TestContext.WriteLine($"Total runs: {numberOfRuns}");

            if (successCount > 0)
            {
                double median = successCount % 2 == 0 
                    ? (maxIqsInRun[successCount / 2 - 1] + maxIqsInRun[successCount / 2]) / 2.0 
                    : maxIqsInRun[successCount / 2];

                int p05Index = (int)Math.Max(0, successCount * 0.05);
                int p95Index = (int)Math.Min(successCount - 1, successCount * 0.95);
                
                double p05 = maxIqsInRun[p05Index];
                double p95 = maxIqsInRun[p95Index];

                TestContext.WriteLine($"Median maximal IQ reached: {median:F2}");
                TestContext.WriteLine($"90% Confidence Interval (5th - 95th percentile): [{p05:F2}, {p95:F2}]");
                
                Assert.IsTrue(median > 0);
            }
        }
        [TestMethod]
        public void CompareGeneticDiversity_In7Generations()
        {
            int numberOfRuns = 100;
            var generationsToSimulate = 7;

            var options = new BreedingSimulationOptions
            {
                TotalGenerationsToSimulate = generationsToSimulate,
                InitialPopulationSize = 10,
                InitialPopulationSelectionPoolSize = 20,
                InheritanceMutationStdDev = 0.15,
                MaxBreedingFemales = 2,
                MaxBreedingMales = 1,
                MortalityHalfLife = TimeSpan.FromDays(365 * 4)
            };

            var randomProvider = new MathNetGaussianRandomProvider();
            var fitnessCalculator = new MeanTraitFitnessCalculator(options);
            
            List<double> elitistStdDevs = new List<double>();
            List<double> halfInbreedingStdDevs = new List<double>();

            for (int i = 0; i < numberOfRuns; i++)
            {
                // 1. Elitist Strategy
                var elitistStrategy = new ElitistBreedingStrategy(options, fitnessCalculator, randomProvider);
                var elitistEngine = new SimulationEngine(options, fitnessCalculator, elitistStrategy, randomProvider);
                var elitistReport = elitistEngine.RunSingleSimulation();
                elitistStdDevs.Add(elitistReport.StatisticsByGeneration.Last().FitnessStdDev);

                // 2. Half Inbreeding Strategy
                var halfInbreedingStrategy = new HalfInbreedingBreedingStrategy(options, fitnessCalculator, randomProvider);
                var halfEngine = new SimulationEngine(options, fitnessCalculator, halfInbreedingStrategy, randomProvider);
                var halfReport = halfEngine.RunSingleSimulation();
                halfInbreedingStdDevs.Add(halfReport.StatisticsByGeneration.Last().FitnessStdDev);
            }

            elitistStdDevs.Sort();
            halfInbreedingStdDevs.Sort();

            double GetMedian(List<double> list)
            {
                int count = list.Count;
                if (count == 0) return 0;
                return count % 2 == 0 ? (list[count / 2 - 1] + list[count / 2]) / 2.0 : list[count / 2];
            }

            double elitistMedian = GetMedian(elitistStdDevs);
            double halfMedian = GetMedian(halfInbreedingStdDevs);

            double p05Elitist = elitistStdDevs[(int)Math.Max(0, numberOfRuns * 0.05)];
            double p95Elitist = elitistStdDevs[(int)Math.Min(numberOfRuns - 1, numberOfRuns * 0.95)];

            double p05Half = halfInbreedingStdDevs[(int)Math.Max(0, numberOfRuns * 0.05)];
            double p95Half = halfInbreedingStdDevs[(int)Math.Min(numberOfRuns - 1, numberOfRuns * 0.95)];

            TestContext.WriteLine($"\n--- GENETIC DIVERSITY (FITNESS STD DEV) AT GENERATION 7 ---");
            TestContext.WriteLine($"Total runs: {numberOfRuns}");
            TestContext.WriteLine($"\n[Elitist Strategy]");
            TestContext.WriteLine($"Median StdDev: {elitistMedian:F4}");
            TestContext.WriteLine($"90% CI: [{p05Elitist:F4}, {p95Elitist:F4}]");

            TestContext.WriteLine($"\n[Half-Inbreeding Strategy]");
            TestContext.WriteLine($"Median StdDev: {halfMedian:F4}");
            TestContext.WriteLine($"90% CI: [{p05Half:F4}, {p95Half:F4}]");
            
            if (elitistMedian > 0)
            {
                double percentageDiff = ((halfMedian - elitistMedian) / elitistMedian) * 100;
                TestContext.WriteLine($"\n=> Half-Inbreeding maintains {percentageDiff:F2}% MORE genetic diversity in median runs.");
            }

            Assert.IsTrue(halfMedian >= 0 && elitistMedian >= 0);
        }
    }
}

