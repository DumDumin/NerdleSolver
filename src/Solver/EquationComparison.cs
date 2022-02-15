using System.Collections.Generic;

namespace Solver
{
    public class EquationComparison
    {
        public Equation Equation { get; }
        public List<ComparisonStatus> Comparison { get; }
        public EquationComparison(Equation equation, List<ComparisonStatus> comparison)
        {
            Equation = equation;
            Comparison = comparison;
        }
    }
}