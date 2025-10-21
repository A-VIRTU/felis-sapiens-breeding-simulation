// =================================================================
// CatBreeding.Core/BreedingSimulationOptions.cs
// =================================================================
namespace CatBreeding.Core
{
    public class BreedingSimulationOptions
    {
        public int TotalTraitCount { get; set; } = 1280;
        public IReadOnlyList<int> FitnessTraitIndices { get; set; } = Enumerable.Range(0, 640).ToList();
        public double InheritanceMutationStdDev { get; set; } = 0.67;
        public int InitialPopulationSize { get; set; } = 1;
        public int InitialPopulationSelectionPoolSize { get; set; } = 4;
        public TimeSpan BreedingCycleDuration { get; set; } = TimeSpan.FromDays(30 * 10);
        public TimeSpan GestationDuration { get; set; } = TimeSpan.FromDays(60);
        public TimeSpan CatFertilityAge { get; set; } = TimeSpan.FromDays(30 * 6);
        public TimeSpan MortalityHalfLife { get; set; } = TimeSpan.FromDays(30 * 18);
        public int TotalGenerationsToSimulate { get; set; } = 10;
        public int MaxBreedingFemales { get; set; } = 2;
        public int MaxBreedingMales { get; set; } = 1;
        public double LitterSizeMean { get; set; } = 4.5;
        public double LitterSizeStdDev { get; set; } = 1.5;
        public int TotalSimulationRuns { get; set; } = 10000;
    }
}