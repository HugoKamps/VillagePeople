using System.Collections.Generic;
using System.Linq;

namespace VillagePeople.FL {
    internal class FzOr : IFuzzyTerm {
        private List<IFuzzyTerm> _terms;

        public FzOr() {
            _terms = new List<IFuzzyTerm>();
        }

        public FzOr(FzOr clone) {
            _terms = clone._terms;
        }

        public FzOr(IFuzzyTerm op1, IFuzzyTerm op2) {
            _terms = new List<IFuzzyTerm> {op1.Clone(), op2.Clone()};
        }

        public FzOr(IFuzzyTerm op1, IFuzzyTerm op2, IFuzzyTerm op3) {
            _terms = new List<IFuzzyTerm> {op1.Clone(), op2.Clone(), op3.Clone()};
        }

        public FzOr(IFuzzyTerm op1, IFuzzyTerm op2, IFuzzyTerm op3, IFuzzyTerm op4) {
            _terms = new List<IFuzzyTerm> {op1.Clone(), op2.Clone(), op3.Clone(), op4.Clone()};
        }

        public void ClearDom() {
            foreach (var t in _terms)
                t.ClearDom();
        }

        public IFuzzyTerm Clone() => new FzOr(this);

        public double GetDom() => _terms.Select(t => t.GetDom()).Concat(new double[] {0}).Max();

        public void OrWithDom(double value) {
            foreach (var t in _terms)
                t.OrWithDom(value);
        }
    }
}