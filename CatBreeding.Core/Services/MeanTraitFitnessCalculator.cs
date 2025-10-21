// =================================================================
// CatBreeding.Core/Services/MeanTraitFitnessCalculator.cs
// =================================================================
using CatBreeding.Core.Domain;

namespace CatBreeding.Core.Services
{
    public class MeanTraitFitnessCalculator : IFitnessCalculator
    {
        private readonly BreedingSimulationOptions _options;

        public MeanTraitFitnessCalculator(BreedingSimulationOptions options)
        {
            _options = options;
        }

        public double CalculateFitness(Cat cat)
        {
            if (_options.FitnessTraitIndices == null || !_options.FitnessTraitIndices.Any())
            {
                return 0.0;
            }

            double sum = _options.FitnessTraitIndices.Sum(index => cat.TraitVector[index]);
            return sum / _options.FitnessTraitIndices.Count;
        }
    }
}