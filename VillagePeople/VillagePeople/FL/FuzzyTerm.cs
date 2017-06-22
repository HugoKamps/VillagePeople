namespace VillagePeople.FL {
    internal interface IFuzzyTerm {
        IFuzzyTerm Clone();

        double GetDom();

        void ClearDom();

        void OrWithDom(double value);
    }
}