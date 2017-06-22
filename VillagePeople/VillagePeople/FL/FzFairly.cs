using System;

namespace VillagePeople.FL {
    internal class FzFairly : IFuzzyTerm {
        private FuzzySet _set;

        public FzFairly(FzSet fuzzySet) {
            _set = fuzzySet.Set;
        }

        public FzFairly(FzFairly clone) {
            _set = clone._set;
        }

        public void ClearDom() {
            _set.ClearDom();
        }

        public IFuzzyTerm Clone() => new FzFairly(this);

        public double GetDom() => Math.Sqrt(_set.GetDom());

        public void OrWithDom(double value) {
            _set.OrWithDom(Math.Sqrt(value));
        }
    }
}