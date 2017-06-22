namespace VillagePeople.FL {
    internal abstract class FuzzySet {
        protected double MDDom;
        protected double MDRepresentativeValue;

        public FuzzySet(double repVal) {
            MDDom = 0.0;
            MDRepresentativeValue = repVal;
        }

        public abstract double CalculateDom(double value);

        public double GetRepresentativeValeu() => MDRepresentativeValue;

        public void OrWithDom(double val) {
            if (val > MDDom)
                MDDom = val;
        }

        public void ClearDom() {
            MDDom = 0.0;
        }

        public double GetDom() {
            return MDDom;
        }

        public void SetDom(double value) {
            if (value >= 0 && value <= 1)
                MDDom = value;
        }
    }
}