// =================================================================
// CatBreeding.Core/Services/MathNetGaussianRandomProvider.cs
// =================================================================
using MathNet.Numerics.Distributions;

namespace CatBreeding.Core.Services
{
    public class MathNetGaussianRandomProvider : IRandomProvider
    {

        private static Normal _distribution;
        private readonly Random _random = new Random();

        private static Normal Distribution => _distribution ??= new Normal(0, 1);

        public int Next(int maxValue)
        {
            return _random.Next(maxValue);
        }

        public double NextDouble()
        {
            return _random.NextDouble();
        }

        public double NextGaussian()
        {
            return Distribution.Sample();
        }
    }
}