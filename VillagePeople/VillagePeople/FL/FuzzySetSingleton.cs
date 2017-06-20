namespace SteeringCS.fuzzylogic {
    internal class FuzzySetSingleton : FuzzySet {
        private double _leftOffset;
        private double _midPoint;
        private double _rightOffset;

        public FuzzySetSingleton(double mid, double left, double right)
            : base(mid) {
            _midPoint = mid;
            _leftOffset = left;
            _rightOffset = right;
        }

        public override double CalculateDom(double value) {
            if (value >= _midPoint - _leftOffset && value <= _midPoint + _rightOffset) return 1.0;
            return 0.0;
        }
    }
}