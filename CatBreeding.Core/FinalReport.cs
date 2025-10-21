using System.Collections.Generic;

namespace CatBreeding.Core
{
    public class FinalReport
    {
        public BreedingSimulationOptions SimulationOptions { get; set; }
        public IReadOnlyList<GenerationStatistics> StatisticsByGeneration { get; set; }
    }
}
