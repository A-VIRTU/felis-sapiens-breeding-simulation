// =================================================================
// CatBreeding.Core/Services/IGaussianRandomProvider.cs
// =================================================================
namespace CatBreeding.Core.Services
{
    /// <summary>
    /// Sjednocuje rozhraní pro poskytování různých typů náhodných čísel
    /// potřebných v simulaci. Cílem je abstrahovat od konkrétní implementace
    /// (např. System.Random) a umožnit tak snadné a spolehlivé testování
    /// komponent, které na náhodnosti závisí.
    /// </summary>
    public interface IRandomProvider : IGaussianRandomProvider
    {
        /// <summary>
        /// Vrací náhodné číslo s plovoucí desetinnou čárkou, které je větší nebo rovno 0.0 a menší než 1.0.
        /// </summary>
        double NextDouble();

        /// <summary>
        /// Vrací nezáporné náhodné celé číslo, které je menší než zadané maximum.
        /// </summary>
        int Next(int maxValue);
    }
}