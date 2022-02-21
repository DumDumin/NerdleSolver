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

        public static EquationComparison FromString(string input, Equation eq)
        {
            List<ComparisonStatus> comparison = input.Select(c => {
                if(c == '0')
                    return ComparisonStatus.False;
                else if(c == '1')
                    return ComparisonStatus.Correct;
                else if(c == '2')
                    return ComparisonStatus.WrongPlace;
                else
                    throw new ArgumentException($"{c} cannot be parsed to {nameof(ComparisonStatus)}");
            }).ToList();

            return new EquationComparison(eq, comparison);
        }
    }
}