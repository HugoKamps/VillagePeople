using System.Collections.Generic;

namespace VillagePeople.FL {
    internal class FuzzyModule {
        private List<FuzzyRule> _rules;

        public FuzzyModule() {
            VarMap = new Dictionary<string, FuzzyVariable>();
            _rules = new List<FuzzyRule>();
        }

        private Dictionary<string, FuzzyVariable> VarMap { get; }

        public FuzzyVariable CreateFlv(string varName) {
            var fv = new FuzzyVariable();

            VarMap.Add(varName, fv);
            return fv;
        }

        public void AddRule(IFuzzyTerm antecedent, IFuzzyTerm consquence) {
            _rules.Add(new FuzzyRule(antecedent, consquence));
        }

        public void Fuzzify(string variableName, double value) {
            VarMap[variableName].Fuzzify(value);
        }

        public double DeFuzzify(string key) {
            SetConfidencesOfConsequentsToZero();

            foreach (var fr in _rules)
                fr.Calculate();

            return VarMap[key].DeFuzzifyMaxAv();
        }

        public void SetConfidencesOfConsequentsToZero() {
            foreach (var fr in _rules)
                fr.SetConfidenceOfConsquentToZero();
        }
    }
}