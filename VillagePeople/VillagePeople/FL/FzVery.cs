namespace VillagePeople.FL {
    internal class FzVery : IFuzzyTerm {
        private FuzzySet _set;

        public FzVery(FzSet fuzzySet) {
            _set = fuzzySet.Set;
        }

        public FzVery(FuzzySet fuzzySet) {
            _set = fuzzySet;
        }

        public FzVery(FzVery clone) {
            _set = clone._set;
        }

        public void ClearDom() {
            _set.ClearDom();
        }

        public IFuzzyTerm Clone() => new FzVery(this);

        public double GetDom() => _set.GetDom() * _set.GetDom();

        public void OrWithDom(double value) {
            _set.OrWithDom(value * value);
        }
    }
}