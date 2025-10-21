using CatBreeding.Core.Analysis;
using CatBreeding.Core.Domain;
using CatBreeding.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CatBreeding.Core
{
    public class SimulationEngine
    {
        private const double TARGET_INITIAL_FITNESS_STD_DEV = 1.0;

        private readonly BreedingSimulationOptions _options;
        private readonly IFitnessCalculator _fitnessCalculator;
        private readonly IBreedingStrategy _breedingStrategy;
        private readonly IRandomProvider _randomProvider;
        private readonly double _adjustedInitialSubTraitStdDev;

        public SimulationEngine(
            BreedingSimulationOptions options,
            IFitnessCalculator fitnessCalculator,
            IBreedingStrategy breedingStrategy,
            IRandomProvider randomProvider)
        {
            _options = options;
            _fitnessCalculator = fitnessCalculator;
            _breedingStrategy = breedingStrategy;
            _randomProvider = randomProvider;

            int n = _options.FitnessTraitIndices.Count;
            _adjustedInitialSubTraitStdDev = TARGET_INITIAL_FITNESS_STD_DEV * Math.Sqrt(n);
        }

        private Cat CreateRandomCat(Gender gender, TimeSpan birthTime)
        {
            var traits = new double[_options.TotalTraitCount];
            var fitnessIndicesSet = new HashSet<int>(_options.FitnessTraitIndices);

            for (int i = 0; i < _options.TotalTraitCount; i++)
            {
                traits[i] = _randomProvider.NextGaussian() * _adjustedInitialSubTraitStdDev;
            }
            return new Cat(Guid.NewGuid(), birthTime, gender, new TraitVector(traits), true, null, null);
        }

        public FinalReport RunSingleSimulation()
        {
            var initialPopulation = GenerateInitialPopulation();
            var run = new SimulationRun(_options, initialPopulation);
            var currentTime = TimeSpan.Zero;

            // Vypočítáme "baseline" fitness z nulté generace pro pozdější výpočet kvocientu.
            CalculateAndStoreStats(run, 0, TimeSpan.Zero);
            var baselineStats = run.GenerationalStats.First();
            run.BaselineFitnessMean = baselineStats.AverageFitness;
            run.BaselineFitnessStdDev = baselineStats.FitnessStdDev;

            for (int i = 1; i <= _options.TotalGenerationsToSimulate; i++)
            {
                currentTime += _options.BreedingCycleDuration;

                SimulateMortality(run.Cattery);

                if (!run.Cattery.AllCats.Any())
                {
                    break; // Populace vymřela
                }

                var breedingResult = _breedingStrategy.Apply(run.Cattery, currentTime);

                // Aplikujeme výsledky šlechtění
                var sterilizedIds = breedingResult.SterilizedCats.Select(c => c.Id).ToHashSet();
                foreach (var cat in run.Cattery.AllCats.Where(c => sterilizedIds.Contains(c.Id)))
                {
                    cat.IsFertile = false;
                }
                run.Cattery.Add(breedingResult.NewCats);

                // Pro další generace předáme čas narození nových koťat
                // Čas narození je currentTime + gestation
                var litterBirthTime = currentTime + _options.GestationDuration;
                CalculateAndStoreStats(run, i, litterBirthTime);
            }

            // OPRAVA: Vracíme původní, čistý report bez nadbytečných vlastností.
            return new FinalReport
            {
                SimulationOptions = _options,
                StatisticsByGeneration = run.GenerationalStats
            };
        }

        private List<Cat> GenerateInitialPopulation()
        {
            return Enumerable.Range(0, _options.InitialPopulationSelectionPoolSize)
                .Select(_ => CreateRandomCat(
                    _randomProvider.NextDouble() < 0.5 ? Gender.Male : Gender.Female,
                    TimeSpan.Zero))
                .OrderByDescending(_fitnessCalculator.CalculateFitness)
                .Take(_options.InitialPopulationSize)
                .ToList();
        }

        private void SimulateMortality(Cattery cattery)
        {
            if (_options.MortalityHalfLife.TotalDays <= 0) return;

            double t = _options.BreedingCycleDuration.TotalDays;
            double T_half = _options.MortalityHalfLife.TotalDays;
            double deathProbability = 1 - Math.Pow(2, -t / T_half);

            var survivors = cattery.AllCats
                .Where(cat => _randomProvider.NextDouble() >= deathProbability)
                .ToList();

            cattery.Clear();
            cattery.Add(survivors);
        }

        /// <summary>
        /// OPRAVA: Metoda nyní počítá statistiky pouze z relevantní skupiny koček pro danou generaci.
        /// </summary>
        /// <param name="run">Aktuální běh simulace.</param>
        /// <param name="generationNumber">Číslo generace.</param>
        /// <param name="birthTimeOfGeneration">Čas narození koček, které patří do této generace.</param>
        private void CalculateAndStoreStats(SimulationRun run, int generationNumber, TimeSpan birthTimeOfGeneration)
        {
            // Pro nultou generaci bereme všechny (zakladatele).
            // Pro ostatní generace filtrujeme POUZE nově narozená koťata.
            var relevantPopulation = run.Cattery.AllCats
                .Where(c => c.BirthTime == birthTimeOfGeneration)
                .ToList();

            if (!relevantPopulation.Any())
            {
                run.GenerationalStats.Add(new GenerationStatistics { GenerationNumber = generationNumber, PopulationSize = 0 });
                return;
            }

            var fitnessValues = relevantPopulation.Select(_fitnessCalculator.CalculateFitness).ToList();
            double avgFitness = fitnessValues.Average();
            double sumOfSquares = fitnessValues.Select(f => (f - avgFitness) * (f - avgFitness)).Sum();
            double stdDev = fitnessValues.Count > 1 ? Math.Sqrt(sumOfSquares / (fitnessValues.Count - 1)) : 0;

            run.GenerationalStats.Add(new GenerationStatistics
            {
                GenerationNumber = generationNumber,
                AverageFitness = avgFitness,
                FitnessStdDev = stdDev,
                MinFitness = fitnessValues.Min(),
                MaxFitness = fitnessValues.Max(),
                PopulationSize = relevantPopulation.Count, // Velikost vrhu, ne celého chovu
                AverageFitnessQuotient = FitnessConverter.ConvertToQuotient(avgFitness)
            });
        }
    }
}

