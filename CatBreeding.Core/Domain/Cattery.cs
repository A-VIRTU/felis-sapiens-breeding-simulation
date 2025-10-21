// =================================================================
// CatBreeding.Core/Domain/Cattery.cs
// =================================================================
namespace CatBreeding.Core.Domain
{
    public class Cattery
    {
        private readonly Dictionary<Guid, Cat> _cats = new Dictionary<Guid, Cat>();

        public IReadOnlyCollection<Cat> AllCats => _cats.Values;

        public Cattery(IEnumerable<Cat> initialPopulation)
        {
            foreach (var cat in initialPopulation)
            {
                Add(cat);
            }
        }

        public void Add(Cat cat)
        {
            if (cat == null) throw new ArgumentNullException(nameof(cat));
            _cats[cat.Id] = cat;
        }

        public void Add(IEnumerable<Cat> cats)
        {
            foreach (var cat in cats)
            {
                Add(cat);
            }
        }

        public void AddLitter(IEnumerable<Cat> cats)
        {
            foreach (var cat in cats)
            {
                Add(cat);
            }
        }

        public Cat FindById(Guid id)
        {
            _cats.TryGetValue(id, out var cat);
            return cat;
        }

        public void Sterilize(IReadOnlyCollection<Guid> catIdsToSterilize)
        {
            if (catIdsToSterilize == null) return;
            foreach (var catId in catIdsToSterilize)
            {
                if (_cats.TryGetValue(catId, out var cat))
                {
                    cat.IsFertile = false;
                }
            }
        }

        public void RemoveDeceased(IReadOnlyCollection<Guid> catIdsToRemove)
        {
            if (catIdsToRemove == null) return;
            foreach (var catId in catIdsToRemove)
            {
                _cats.Remove(catId);
            }
        }

        internal void Clear()
        {
            _cats.Clear();
        }
    }
}