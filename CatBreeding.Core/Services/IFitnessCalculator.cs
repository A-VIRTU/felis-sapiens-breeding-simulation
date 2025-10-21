// =================================================================
// CatBreeding.Core/Services/IFitnessCalculator.cs
// =================================================================
using CatBreeding.Core.Domain;

namespace CatBreeding.Core.Services
{
    public interface IFitnessCalculator
    {
        double CalculateFitness(Cat cat);
    }
}