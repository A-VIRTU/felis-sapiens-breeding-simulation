// =================================================================
// CatBreeding.Core/Services/IBreedingStrategy.cs
// =================================================================
using CatBreeding.Core.Domain;

namespace CatBreeding.Core.Services
{
    /// <summary>
    /// Definuje kontrakt pro všechny šlechtitelské strategie. Použití rozhraní
    /// odděluje hlavní simulační engine od konkrétních pravidel šlechtění,
    /// což umožňuje snadnou záměnu strategií (Strategy Pattern).
    /// </summary>
    public interface IBreedingStrategy
    {
        /// <summary>
        /// Aplikuje šlechtitelskou logiku na aktuální stav chovné stanice.
        /// </summary>
        /// <param name="cattery">Aktuální kolekce všech koček v simulaci.</param>
        /// <param name="currentTime">Aktuální čas v simulaci.</param>
        /// <returns>Objekt BreedingResult s detailem všech změn.</returns>
        BreedingResult Apply(Cattery cattery, TimeSpan currentTime);
    }
}