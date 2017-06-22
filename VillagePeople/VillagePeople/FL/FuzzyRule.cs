namespace VillagePeople.FL {
    internal class FuzzyRule {
        private IFuzzyTerm _antecedent;
        private IFuzzyTerm _consequence;

        public FuzzyRule(IFuzzyTerm ant, IFuzzyTerm con) {
            _antecedent = ant;
            _consequence = con;
        }

        public void SetConfidenceOfConsquentToZero() {
            _consequence.ClearDom();
        }

        public void Calculate() {
            _consequence.OrWithDom(_antecedent.GetDom());
        }
    }
}