using CatBreeding.Core.Domain;
using CatBreeding.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CatBreeding.Core.Tests.Services
{
    [TestClass]
    public class ElitistBreedingStrategyTests
    {
        private Mock<IRandomProvider> _randomProviderMock;
        private Mock<IFitnessCalculator> _fitnessCalculatorMock;
        private BreedingSimulationOptions _options;
        private ElitistBreedingStrategy _strategy;

        [TestInitialize]
        public void TestInitialize()
        {
            _randomProviderMock = new Mock<IRandomProvider>();
            _fitnessCalculatorMock = new Mock<IFitnessCalculator>();
            _options = new BreedingSimulationOptions
            {
                TotalTraitCount = 10,
                CatFertilityAge = TimeSpan.FromDays(30 * 6),
                BreedingCycleDuration = TimeSpan.FromDays(30 * 10),
                GestationDuration = TimeSpan.FromDays(60),
                LitterSizeMean = 2, // Nastaveno na 2 pro predikovatelné testy
                LitterSizeStdDev = 0, // Nastaveno na 0 pro predikovatelné testy
                InheritanceMutationStdDev = 0.1,
                MaxBreedingFemales = 2
            };

            // Předáváme mock nového, sjednoceného IRandomProvider
            _strategy = new ElitistBreedingStrategy(_options, _fitnessCalculatorMock.Object, _randomProviderMock.Object);
        }


        /// <summary>
        /// OPRAVENÝ TEST: Původní test selhával, protože měl v nastavení `MaxBreedingFemales = 2`,
        /// ale v aserci očekával spáření pouze jedné samice.
        /// Tento test nyní ověřuje původní záměr: pokud je povolena pouze jedna samice,
        /// vybere se skutečně jen ta nejlepší a ostatní jsou sterilizovány.
        /// </summary>
        [TestMethod]
        public void Apply_MaxBreedingFemalesIsOne_SelectsOnlyBestFemale()
        {
            // Arrange
            _options.MaxBreedingFemales = 1; // Změna nastavení specificky pro tento test
            var currentTime = TimeSpan.FromDays(365);
            var lastGenTime = currentTime - _options.BreedingCycleDuration;

            var bestMale = new Cat(Guid.NewGuid(), lastGenTime, Gender.Male, new TraitVector(new double[10]), true, null, null);
            var bestFemale = new Cat(Guid.NewGuid(), lastGenTime, Gender.Female, new TraitVector(new double[10]), true, null, null);
            var worseFemale = new Cat(Guid.NewGuid(), lastGenTime, Gender.Female, new TraitVector(new double[10]), true, null, null);
            var cattery = new Cattery(new List<Cat> { bestMale, bestFemale, worseFemale });

            _fitnessCalculatorMock.Setup(c => c.CalculateFitness(bestMale)).Returns(100);
            _fitnessCalculatorMock.Setup(c => c.CalculateFitness(bestFemale)).Returns(90);
            _fitnessCalculatorMock.Setup(c => c.CalculateFitness(worseFemale)).Returns(80);

            _randomProviderMock.Setup(r => r.NextGaussian()).Returns(0);

            // Act
            var result = _strategy.Apply(cattery, currentTime);

            // Assert
            Assert.AreEqual(2, result.NewCats.Count, "Měl se narodit jeden vrh o 2 koťatech.");
            Assert.AreEqual(1, result.MatedFemales.Count, "Pouze nejlepší samice měla být spářena.");
            Assert.AreEqual(bestFemale.Id, result.MatedFemales.First().Id);
            Assert.AreEqual(1, result.SterilizedCats.Count, "Horší samice měla být určena ke sterilizaci.");
            Assert.AreEqual(worseFemale.Id, result.SterilizedCats.First().Id);
        }

        /// <summary>
        /// NOVÝ TEST: Tento test izoluje a ověřuje chování, které způsobilo selhání původního testu.
        /// Explicitně testujeme scénář, kdy je `MaxBreedingFemales` nastaveno na 2. Očekáváme,
        /// že se vyberou DVA nejlepší jedinci a narodí se DVA vrhy.
        /// </summary>
        [TestMethod]
        public void Apply_MaxBreedingFemalesIsTwo_SelectsTwoBestFemales()
        {
            // Arrange
            _options.MaxBreedingFemales = 2; // Výchozí hodnota, ale pro jistotu explicitně
            var currentTime = TimeSpan.FromDays(365);
            var lastGenTime = currentTime - _options.BreedingCycleDuration;

            var bestMale = new Cat(Guid.NewGuid(), lastGenTime, Gender.Male, new TraitVector(new double[10]), true, null, null);
            var female1 = new Cat(Guid.NewGuid(), lastGenTime, Gender.Female, new TraitVector(new double[10]), true, null, null); // Fitness 90
            var female2 = new Cat(Guid.NewGuid(), lastGenTime, Gender.Female, new TraitVector(new double[10]), true, null, null); // Fitness 80
            var female3 = new Cat(Guid.NewGuid(), lastGenTime, Gender.Female, new TraitVector(new double[10]), true, null, null); // Fitness 70
            var cattery = new Cattery(new List<Cat> { bestMale, female1, female2, female3 });

            _fitnessCalculatorMock.Setup(c => c.CalculateFitness(bestMale)).Returns(100);
            _fitnessCalculatorMock.Setup(c => c.CalculateFitness(female1)).Returns(90);
            _fitnessCalculatorMock.Setup(c => c.CalculateFitness(female2)).Returns(80);
            _fitnessCalculatorMock.Setup(c => c.CalculateFitness(female3)).Returns(70);

            _randomProviderMock.Setup(r => r.NextGaussian()).Returns(0);

            // Act
            var result = _strategy.Apply(cattery, currentTime);

            // Assert
            Assert.AreEqual(4, result.NewCats.Count, "Měly se narodit dva vrhy po 2 koťatech.");
            Assert.AreEqual(2, result.MatedFemales.Count, "Měly být spářeny dvě nejlepší samice.");
            Assert.IsTrue(result.MatedFemales.Any(f => f.Id == female1.Id), "První nejlepší samice měla být spářena.");
            Assert.IsTrue(result.MatedFemales.Any(f => f.Id == female2.Id), "Druhá nejlepší samice měla být spářena.");
            Assert.AreEqual(1, result.SterilizedCats.Count, "Nejhorší samice měla být sterilizována.");
            Assert.AreEqual(female3.Id, result.SterilizedCats.First().Id);
        }

        /// <summary>
        /// NOVÝ TEST: Ověřuje, že je vybrán skutečně nejlepší samec, pokud je jich v populaci více.
        /// Tím izolujeme logiku výběru samců od logiky výběru samic.
        /// </summary>
        [TestMethod]
        public void Apply_MultipleMales_SelectsOnlyBestMale()
        {
            // Arrange
            _options.MaxBreedingFemales = 1;
            var currentTime = TimeSpan.FromDays(365);
            var lastGenTime = currentTime - _options.BreedingCycleDuration;

            var bestMale = new Cat(Guid.NewGuid(), lastGenTime, Gender.Male, new TraitVector(new double[10]), true, null, null); // Fitness 100
            var worseMale = new Cat(Guid.NewGuid(), lastGenTime, Gender.Male, new TraitVector(new double[10]), true, null, null); // Fitness 90
            var female = new Cat(Guid.NewGuid(), lastGenTime, Gender.Female, new TraitVector(new double[10]), true, null, null); // Fitness 95
            var cattery = new Cattery(new List<Cat> { bestMale, worseMale, female });

            _fitnessCalculatorMock.Setup(c => c.CalculateFitness(bestMale)).Returns(100);
            _fitnessCalculatorMock.Setup(c => c.CalculateFitness(worseMale)).Returns(90);
            _fitnessCalculatorMock.Setup(c => c.CalculateFitness(female)).Returns(95);

            _randomProviderMock.Setup(r => r.NextGaussian()).Returns(0);

            // Act
            var result = _strategy.Apply(cattery, currentTime);

            // Assert
            Assert.IsTrue(result.NewCats.Any(), "Měla se narodit koťata.");
            Assert.IsTrue(result.NewCats.All(c => c.FatherId == bestMale.Id), "Otcem všech koťat měl být samec s nejvyšší fitness.");
        }
    }
}

