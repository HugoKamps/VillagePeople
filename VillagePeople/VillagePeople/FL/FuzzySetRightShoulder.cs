namespace VillagePeople.FL {
    internal class FuzzySetRightShoulder : FuzzySet {
        private double _leftOffset;
        private double _peakPoint;
        private double _rightOffset;

        public FuzzySetRightShoulder(double peak, double left, double right)
            : base((peak + right + peak) / 2) {
            _peakPoint = peak;
            _leftOffset = left;
            _rightOffset = right;
        }

        public override double CalculateDom(double value) {
            if (Equals(_rightOffset, 0.0) && Equals(_peakPoint, value) ||
                Equals(_leftOffset, 0.0) && Equals(_peakPoint, value))
                return 1.0;

            if (value <= _peakPoint && value > _peakPoint - _leftOffset) {
                var grad = 1.0 / _leftOffset;

                return grad * (value - (_peakPoint - _leftOffset));
            }

            if (value > _peakPoint && value <= _peakPoint + _rightOffset)
                return 1.0;

            return 0;
        }
    }
}