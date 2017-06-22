namespace VillagePeople.FL {
    internal class FzSet : IFuzzyTerm {
        public FuzzySet Set;

        public FzSet(FuzzySet fuzzySet) {
            Set = fuzzySet;
        }

        public FzSet(FzSet clone) {
            Set = clone.Set;
        }

        public void ClearDom() {
            Set.ClearDom();
        }

        public IFuzzyTerm Clone() => new FzSet(this);

        public double GetDom() => Set.GetDom();

        public void OrWithDom(double value) {
            Set.OrWithDom(value);
        }
    }
}