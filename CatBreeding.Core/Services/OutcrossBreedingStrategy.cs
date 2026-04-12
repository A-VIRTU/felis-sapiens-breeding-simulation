using CatBreeding.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CatBreeding.Core.Services
{
    public class OutcrossBreedingStrategy : IBreedingStrategy
    {
        private readonly BreedingSimulationOptions _options;
        private readonly IFitnessCalculator _fitnessCalculator;
        private readonly IRandomProvider _randomProvider;
        private const double MALE_PROBABILITY = 100.0 / (100.0 + 92.0);

        public OutcrossBreedingStrategy(
            BreedingSimulationOptions options,
            IFitnessCalculator fitnessCalculator,
            IRandomProvider randomProvider)
        {
            _options = options;
            _fitnessCalculator = fitnessCalculator;
            _randomProvider = randomProvider;
        }

        public BreedingResult Apply(Cattery cattery, TimeSpan currentTime)
        {
            var allCats = cattery.AllCats;
            var newKittens = new List<Cat>();
            var sterilizationTargets = new List<Cat>();
            var matedFemales = new List<Cat>();

            var fertileFemales = allCats.Where(c => c.IsFertile && c.Gender == Gender.Female && (currentTime - c.BirthTime) >= _options.CatFertilityAge).ToList();
            var fertileMales = allCats.Where(c => c.IsFertile && c.Gender == Gender.Male && (currentTime - c.BirthTime) >= _options.CatFertilityAge).ToList();

            var topFemales = fertileFemales.OrderByDescending(_fitnessCalculator.CalculateFitness).Take(_options.MaxBreedingFemales).ToList();

            if (!topFemales.Any())
            {
                var newFemale = CreateRandomCat(Gender.Female, currentTime - _options.CatFertilityAge * 1.2);
                topFemales.Add(newFemale);
                fertileFemales.Add(newFemale);
            }

            matedFemales.AddRange(topFemales);
            foreach (var female in topFemales)
            {
                // Vždy se spáří s "náhodným samcem mimo chov, průměrná populace"
                Cat outsideMale = CreateRandomCat(Gender.Male, currentTime - _options.CatFertilityAge * 1.2);

                double gaussianLitterSize = (_randomProvider.NextGaussian() * _options.LitterSizeStdDev) + _options.LitterSizeMean;
                int litterSize = (int)Math.Round(gaussianLitterSize);
                litterSize = Math.Max(0, Math.Min(8, litterSize));

                if (litterSize == 0) continue;

                var inheritanceMutationAdjustedDev = Math.Sqrt(_options.FitnessTraitIndices.Count) * _options.InheritanceMutationStdDev;
                var birthTime = currentTime + _options.GestationDuration;

                for (int i = 0; i < litterSize; i++)
                {
                    Cat father = outsideMale;

                    var gender = _randomProvider.NextDouble() < MALE_PROBABILITY ? Gender.Male : Gender.Female;
                    var traits = new double[_options.TotalTraitCount];
                    for (int t = 0; t < _options.TotalTraitCount; t++)
                    {
                        double parentTrait = _randomProvider.Next(2) == 0 ? female.TraitVector[t] : father.TraitVector[t];
                        double mutation = _randomProvider.NextGaussian() * inheritanceMutationAdjustedDev;
                        traits[t] = parentTrait + mutation;
                    }
                    newKittens.Add(new Cat(Guid.NewGuid(), birthTime, gender, new TraitVector(traits), true, female.Id, father.Id));
                }
            }

            // Sterilizujeme plodnou generaci.
            sterilizationTargets.AddRange(fertileMales);
            sterilizationTargets.AddRange(fertileFemales);

            return new BreedingResult(newKittens, matedFemales, sterilizationTargets.Distinct());
        }

        private Cat CreateRandomCat(Gender gender, TimeSpan birthTime)
        {
            var traits = new double[_options.TotalTraitCount];
            var adjustedStdDev = Math.Sqrt(_options.FitnessTraitIndices.Count);
            for (int i = 0; i < _options.TotalTraitCount; i++)
            {
                // Zcela náhodné generování z průměrné populace
                traits[i] = _randomProvider.NextGaussian() * adjustedStdDev;
            }
            return new Cat(Guid.NewGuid(), birthTime, gender, new TraitVector(traits), true, null, null);
        }
    }
}
