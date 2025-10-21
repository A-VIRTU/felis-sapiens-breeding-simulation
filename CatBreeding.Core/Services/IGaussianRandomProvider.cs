// =================================================================
// CatBreeding.Core/Services/IGaussianRandomProvider.cs
// =================================================================
namespace CatBreeding.Core.Services
{
    public interface IGaussianRandomProvider
    {
        /// <summary>
        /// Vrací náhodné číslo s normálním (Gaussovým) rozdělením se střední hodnotou 0 a směrodatnou odchylkou 1.
        /// </summary>
        double NextGaussian();
    }
}