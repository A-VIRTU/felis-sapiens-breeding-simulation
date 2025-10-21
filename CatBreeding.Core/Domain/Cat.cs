// =================================================================
// CatBreeding.Core/Domain/Cat.cs
// =================================================================
namespace CatBreeding.Core.Domain
{
    public class Cat
    {
        public Guid Id { get; }
        public TimeSpan BirthTime { get; }
        public Gender Gender { get; }
        public TraitVector TraitVector { get; }
        public bool IsFertile { get; set; }
        public Guid? MotherId { get; }
        public Guid? FatherId { get; }

        public Cat(Guid id, TimeSpan birthTime, Gender gender, TraitVector traitVector, bool isFertile, Guid? motherId, Guid? fatherId)
        {
            Id = id;
            BirthTime = birthTime;
            Gender = gender;
            TraitVector = traitVector ?? throw new ArgumentNullException(nameof(traitVector));
            IsFertile = isFertile;
            MotherId = motherId;
            FatherId = fatherId;
        }
    }
}