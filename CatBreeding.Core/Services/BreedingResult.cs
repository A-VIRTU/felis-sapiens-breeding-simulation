// =================================================================
// CatBreeding.Core/Services/IBreedingStrategy.cs
// =================================================================
using CatBreeding.Core.Domain;

namespace CatBreeding.Core.Services
{
    /// <summary>
    /// Represents the outcome of a single breeding cycle from a breeding strategy.
    /// It encapsulates all changes to the cattery, such as new kittens,
    /// which females were mated, and which cats were sterilized.
    /// </summary>
    public class BreedingResult
    {
        public IReadOnlyList<Cat> NewCats { get; }
        public IReadOnlyList<Cat> MatedFemales { get; }
        public IReadOnlyList<Cat> SterilizedCats { get; }

        public BreedingResult(
            IEnumerable<Cat> newCats,
            IEnumerable<Cat> matedFemales,
            IEnumerable<Cat> sterilizedCats)
        {
            NewCats = new List<Cat>(newCats);
            MatedFemales = new List<Cat>(matedFemales);
            SterilizedCats = new List<Cat>(sterilizedCats);
        }
    }
}