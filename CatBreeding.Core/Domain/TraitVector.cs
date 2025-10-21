using System;
using System.Collections.Generic;
using System.Linq;

namespace CatBreeding.Core.Domain
{
    /// <summary>
    /// Represents an immutable vector of genetic traits for an individual.
    /// This class is introduced as a value object to provide strong typing and encapsulation
    /// for the concept of a cat's genetic makeup. Using a dedicated class instead of a raw
    /// array (e.g., double[]) prevents a wide range of potential errors, enforces immutability,
    /// and provides value-based equality semantics, which is crucial for comparisons and tracking.
    /// It ensures that a vector of traits is always treated as a single, atomic value.
    /// </summary>
    public sealed class TraitVector : IEquatable<TraitVector>
    {
        private readonly double[] _traits;

        /// <summary>
        /// Gets the number of traits in the vector.
        /// </summary>
        public int Length => _traits.Length;

        /// <summary>
        /// Gets the trait value at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the trait to get.</param>
        /// <returns>The trait value at the specified index.</returns>
        public double this[int index] => _traits[index];

        /// <summary>
        /// Initializes a new instance of the <see cref="TraitVector"/> class from a sequence of trait values.
        /// </summary>
        /// <param name="traits">The sequence of trait values. Must not be null.</param>
        /// <exception cref="ArgumentNullException">Thrown if traits is null.</exception>
        public TraitVector(IEnumerable<double> traits)
        {
            // The ToArray() call creates a defensive copy, ensuring the internal state cannot be mutated from the outside.
            _traits = traits?.ToArray() ?? throw new ArgumentNullException(nameof(traits));
        }

        /// <summary>
        /// Returns an enumerator that iterates through the trait values.
        /// </summary>
        /// <returns>An enumerator for the trait vector.</returns>
        public IEnumerable<double> AsEnumerable() => _traits;

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// Equality is determined by comparing the sequence of trait values.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
        public bool Equals(TraitVector other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            // SequenceEqual provides an efficient, value-based comparison of the two vectors.
            return _traits.SequenceEqual(other._traits);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as TraitVector);
        }

        /// <summary>
        /// Serves as the default hash function. A correct implementation is essential for
        /// using this object in hash-based collections like dictionaries or hash sets.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            // This implementation combines the hash codes of individual elements
            // to ensure that equal TraitVector instances produce the same hash code.
            return (_traits ?? Array.Empty<double>()).Aggregate(17, (current, value) => current * 23 + value.GetHashCode());
        }
    }
}
