using CatBreeding.Core.Domain;
using CatBreeding.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace CatBreeding.Core.Tests.Analysis
{
    [TestClass]
    public class ChartDataExporter
    {
        private class GenerationMetric
        {
            public int Generation { get; set; }
            public double MedianAvgIq { get; set; }
            public double MedianMaxIq { get; set; }
            public double MaxIqP5 { get; set; }
            public double MaxIqP95 { get; set; }
            public double MedianGeneticDiversity { get; set; }
        }

        private class ExportData
        {
            public List<GenerationMetric> ElitistData { get; set; } = new List<GenerationMetric>();
            public List<GenerationMetric> HalfInbreedingData { get; set; } = new List<GenerationMetric>();
            public List<GenerationMetric> OutcrossData { get; set; } = new List<GenerationMetric>();
        }

        [TestMethod]
        public void ExportChartDataToJson()
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

            var exportData = new ExportData();

            exportData.ElitistData = RunStrategy(options, numberOfRuns, generationsToSimulate, "Elitist");
            exportData.HalfInbreedingData = RunStrategy(options, numberOfRuns, generationsToSimulate, "HalfInbreeding");
            exportData.OutcrossData = RunStrategy(options, numberOfRuns, generationsToSimulate, "Outcross");

            string docsDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\docs\vizualizace"));
            Directory.CreateDirectory(docsDir);

            string json = JsonSerializer.Serialize(exportData, new JsonSerializerOptions { WriteIndented = true });
            
            // Pro snazší vložení do html vytvoříme přímo soubor data.js s proměnnou
            string jsContent = $"const simulationData = {json};";
            File.WriteAllText(Path.Combine(docsDir, "data.js"), jsContent);
        }

        private List<GenerationMetric> RunStrategy(BreedingSimulationOptions options, int numberOfRuns, int maxGen, string strategyType)
        {
            var randomProvider = new MathNetGaussianRandomProvider();
            var fitnessCalculator = new MeanTraitFitnessCalculator(options);
            
            // Uložíme si všechny reporty (ze všech běhů)
            var allReports = new List<FinalReport>();

            for (int i = 0; i < numberOfRuns; i++)
            {
                IBreedingStrategy strategy;
                if (strategyType == "Elitist")
                    strategy = new ElitistBreedingStrategy(options, fitnessCalculator, randomProvider);
                else if (strategyType == "Outcross")
                    strategy = new OutcrossBreedingStrategy(options, fitnessCalculator, randomProvider);
                else
                    strategy = new HalfInbreedingBreedingStrategy(options, fitnessCalculator, randomProvider);

                var engine = new SimulationEngine(options, fitnessCalculator, strategy, randomProvider);
                allReports.Add(engine.RunSingleSimulation());
            }

            var results = new List<GenerationMetric>();

            for (int gen = 1; gen <= maxGen; gen++)
            {
                var avgIqs = allReports.Select(r => r.StatisticsByGeneration[gen].AverageFitnessQuotient).OrderBy(x => x).ToList();
                var maxIqs = allReports.Select(r => r.StatisticsByGeneration[gen].MaxFitnessQuotient).OrderBy(x => x).ToList();
                var stdDevs = allReports.Select(r => r.StatisticsByGeneration[gen].FitnessStdDev).OrderBy(x => x).ToList();

                results.Add(new GenerationMetric
                {
                    Generation = gen,
                    MedianAvgIq = GetMedian(avgIqs),
                    MedianMaxIq = GetMedian(maxIqs),
                    MaxIqP5 = maxIqs[(int)Math.Max(0, numberOfRuns * 0.05)],
                    MaxIqP95 = maxIqs[(int)Math.Min(numberOfRuns - 1, numberOfRuns * 0.95)],
                    MedianGeneticDiversity = GetMedian(stdDevs)
                });
            }

            return results;
        }

        private double GetMedian(List<double> sortedList)
        {
            int count = sortedList.Count;
            if (count == 0) return 0;
            return count % 2 == 0 ? (sortedList[count / 2 - 1] + sortedList[count / 2]) / 2.0 : sortedList[count / 2];
        }
    }
}
