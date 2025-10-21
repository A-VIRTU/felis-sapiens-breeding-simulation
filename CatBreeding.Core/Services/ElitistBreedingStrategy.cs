using CatBreeding.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CatBreeding.Core.Services
{
    public class ElitistBreedingStrategy : IBreedingStrategy
    {
        private readonly BreedingSimulationOptions _options;
        private readonly IFitnessCalculator _fitnessCalculator;
        private readonly IRandomProvider _randomProvider;
        private const double MALE_PROBABILITY = 100.0 / (100.0 + 92.0);

        public ElitistBreedingStrategy(
            BreedingSimulationOptions options,
            IFitnessCalculator fitnessCalculator,
            IRandomProvider randomProvider) // ZMĚNA: Přijímáme nové rozhraní.
        {
            _options = options;
            _fitnessCalculator = fitnessCalculator;
            _randomProvider = randomProvider;
        }

        // ZMĚNA: Upravená signatura metody podle nového rozhraní.
        public BreedingResult Apply(Cattery cattery, TimeSpan currentTime)
        {
            var allCats = cattery.AllCats;
            var newKittens = new List<Cat>();
            var sterilizationTargets = new List<Cat>();
            var matedFemales = new List<Cat>();

            var fertileFemales = allCats.Where(c => c.IsFertile && c.Gender == Gender.Female && (currentTime - c.BirthTime) >= _options.CatFertilityAge).ToList();
            var fertileMales = allCats.Where(c => c.IsFertile && c.Gender == Gender.Male && (currentTime - c.BirthTime) >= _options.CatFertilityAge).ToList();

            var topFemales = fertileFemales.OrderByDescending(_fitnessCalculator.CalculateFitness).Take(_options.MaxBreedingFemales).ToList();
            var topMale = fertileMales.OrderByDescending(_fitnessCalculator.CalculateFitness).FirstOrDefault();

            if (topMale == null)
            {
                topMale = CreateRandomCat(Gender.Male, currentTime - _options.CatFertilityAge * 1.2);
                fertileMales.Add(topMale);
            }

            if (!topFemales.Any())
            {
                var newFemale = CreateRandomCat(Gender.Female, currentTime - _options.CatFertilityAge * 1.2);
                topFemales.Add(newFemale);
                fertileFemales.Add(newFemale);
            }

            matedFemales.AddRange(topFemales);
            foreach (var female in topFemales)
            {
                newKittens.AddRange(CreateLitter(female, topMale, currentTime + _options.GestationDuration));
            }

            sterilizationTargets.AddRange(fertileMales);
            sterilizationTargets.AddRange(fertileFemales);

            return new BreedingResult(newKittens, matedFemales, sterilizationTargets.Distinct());
        }

        private IEnumerable<Cat> CreateLitter(Cat mother, Cat father, TimeSpan birthTime)
        {
            double gaussianLitterSize = (_randomProvider.NextGaussian() * _options.LitterSizeStdDev) + _options.LitterSizeMean;
            int litterSize = (int)Math.Round(gaussianLitterSize);
            litterSize = Math.Max(0, Math.Min(8, litterSize));

            if (litterSize == 0) return Enumerable.Empty<Cat>();

            var litter = new List<Cat>(litterSize);
            var inheritanceMutationAdjustedDev = Math.Sqrt(_options.FitnessTraitIndices.Count) * _options.InheritanceMutationStdDev;
            for (int i = 0; i < litterSize; i++)
            {
                // ZMĚNA: Používáme _randomProvider místo System.Random
                var gender = _randomProvider.NextDouble() < MALE_PROBABILITY ? Gender.Male : Gender.Female;
                var traits = new double[_options.TotalTraitCount];
                for (int t = 0; t < _options.TotalTraitCount; t++)
                {
                    // ZMĚNA: Používáme _randomProvider místo System.Random
                    double parentTrait = _randomProvider.Next(2) == 0 ? mother.TraitVector[t] : father.TraitVector[t];
                    double mutation = _randomProvider.NextGaussian() * inheritanceMutationAdjustedDev;
                    traits[t] = parentTrait + mutation;
                }
                litter.Add(new Cat(Guid.NewGuid(), birthTime, gender, new TraitVector(traits), true, mother.Id, father.Id));
            }
            return litter;
        }

        // Ostatní pomocné metody (CreateRandomCat, CreateBestOfN) zůstávají beze změny,
        // protože již správně používaly IGaussianRandomProvider (nyní IRandomProvider).
        private Cat CreateRandomCat(Gender gender, TimeSpan birthTime)
        {
            var traits = new double[_options.TotalTraitCount];
            var adjustedStdDev = Math.Sqrt(_options.FitnessTraitIndices.Count);
            for (int i = 0; i < _options.TotalTraitCount; i++)
            {
                traits[i] = _randomProvider.NextGaussian() * adjustedStdDev;
            }
            return new Cat(Guid.NewGuid(), birthTime, gender, new TraitVector(traits), true, null, null);
        }

        private Cat CreateBestOfN(Gender gender, TimeSpan birthTime, int n)
        {
            return Enumerable.Range(0, n)
              .Select(_ => CreateRandomCat(gender, birthTime))
              .OrderByDescending(_fitnessCalculator.CalculateFitness)
              .First();
        }
    }
}

