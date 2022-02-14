using System.Collections.Generic;

namespace Solver
{
    public class EquationComparison
    {
        private readonly List<ComparisonStatus> numbers;
        private readonly List<ComparisonStatus> operators;

        public List<ComparisonStatus> Operators => operators;

        public List<ComparisonStatus> Numbers => numbers;

        public EquationComparison(List<ComparisonStatus> numbers, List<ComparisonStatus> operators)
        {
            this.numbers = numbers;
            this.operators = operators;
        }
    }
}