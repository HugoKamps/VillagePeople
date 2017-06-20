namespace SteeringCS.fuzzylogic {
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
            //test for the case where the left or right offsets are zero
            //(to prevent divide by zero errors below)
            if (Equals(_rightOffset, 0.0) && Equals(_peakPoint, value) ||
                Equals(_leftOffset, 0.0) && Equals(_peakPoint, value))
                return 1.0;

            //find DOM if left of center
            if (value <= _peakPoint && value > _peakPoint - _leftOffset) {
                var grad = 1.0 / _leftOffset;

                return grad * (value - (_peakPoint - _leftOffset));
            }
            //find DOM if right of center and less than center + right offset
            if (value > _peakPoint && value <= _peakPoint + _rightOffset)
                return 1.0;

            return 0;
        }
    }
}