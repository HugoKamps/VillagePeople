namespace SteeringCS.fuzzylogic {
    internal class FuzzySetTriangle : FuzzySet {
        private double _leftOffset;
        private double _peakPoint;
        private double _rightOffset;

        public FuzzySetTriangle(double mid, double left, double right)
            : base(mid) {
            _peakPoint = mid;
            _leftOffset = left;
            _rightOffset = right;
        }


        public override double CalculateDom(double val) {
            if (Equals(_rightOffset, 0.0) && Equals(_peakPoint, val) ||
                Equals(_leftOffset, 0.0) && Equals(_peakPoint, val))
                return 1.0;
            //find DOM if left of center 
            if (val <= _peakPoint && val >= _peakPoint - _leftOffset) {
                var grad = 1.0 / _leftOffset;
                return grad * (val - (_peakPoint - _leftOffset));
            }
            //find DOM if right of center 
            if (!(val > _peakPoint) || !(val < _peakPoint + _rightOffset)) return 0.0;
            {
                var grad = 1.0 / -_rightOffset;
                return grad * (val - _peakPoint) + 1.0;
            }
            //out of range of this FLV, return zero 
        }
    }
}