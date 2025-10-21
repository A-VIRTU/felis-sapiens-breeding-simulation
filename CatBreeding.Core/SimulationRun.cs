// =================================================================
// CatBreeding.Core/SimulationRun.cs
// =================================================================
using CatBreeding.Core.Domain;

namespace CatBreeding.Core
{
    public class SimulationRun
    {
        public BreedingSimulationOptions Options { get; }
        public Cattery Cattery { get; set; }
        public List<GenerationStatistics> GenerationalStats { get; } = new List<GenerationStatistics>();
        public double BaselineFitnessMean { get; set; }
        public double BaselineFitnessStdDev { get; set; }

        public SimulationRun(BreedingSimulationOptions options, IEnumerable<Cat> initialPopulation)
        {
            Options = options;
            Cattery = new Cattery(initialPopulation);
        }
    }
}