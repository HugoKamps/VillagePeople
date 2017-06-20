using System.Collections.Generic;

namespace SteeringCS.fuzzylogic {
    internal class FuzzyVariable {
        private double _maxRange;
        private Dictionary<string, FuzzySet> _memberSets;
        private double _minRange;

        public FuzzyVariable() {
            _minRange = 0.0;
            _maxRange = 0.0;
            _memberSets = new Dictionary<string, FuzzySet>();
        }

        public void AdjustRangeTotFit(double min, double max) {
            if (min < _minRange)
                _minRange = min;
            if (max > _maxRange)
                _maxRange = max;
        }

        public FzSet AddTriangularSet(string name, double minBound, double peak, double maxBound) {
            var fst = new FuzzySetTriangle(peak, peak - minBound, maxBound - peak);

            AdjustRangeTotFit(minBound, maxBound);

            _memberSets.Add(name, fst);
            return new FzSet(fst);
        }

        public FzSet AddRightShoulderSet(string name, double minBound, double peak, double maxBound) {
            var fsrs = new FuzzySetRightShoulder(peak, peak - minBound, maxBound - peak);

            AdjustRangeTotFit(minBound, maxBound);

            _memberSets.Add(name, fsrs);
            return new FzSet(fsrs);
        }

        public FzSet AddLeftShoulderSet(string name, double minBound, double peak, double maxBound) {
            var fsrs = new FuzzySetLeftShoulder(peak, peak - minBound, maxBound - peak);

            AdjustRangeTotFit(minBound, maxBound);

            _memberSets.Add(name, fsrs);
            return new FzSet(fsrs);
        }

        public FzSet AddSingletonSet(string name, double minBound, double peak, double maxBound) {
            var fss = new FuzzySetSingleton(peak, minBound, maxBound);

            _memberSets.Add(name, fss);
            return new FzSet(fss);
        }

        public void Fuzzify(double val) {
            foreach (var f in _memberSets.Values)
                f.SetDom(f.CalculateDom(val));
        }

        public double DeFuzzifyMaxAv() {
            var bottom = 0.0;
            var top = 0.0;

            foreach (var f in _memberSets.Values) {
                bottom += f.GetDom();
                top += f.GetRepresentativeValeu() * f.GetDom();
            }

            if (Equals(0, bottom))
                return 0.0;

            return top / bottom;
        }
    }
}