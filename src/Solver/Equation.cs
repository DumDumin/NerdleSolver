using System;
using System.Collections.Generic;
using System.Linq;

namespace Solver
{
    public class Equation
    {
        private IList<long> numbers;
        private IList<Operator> operators;

        public Equation(IList<long> numbers, IList<Operator> operators)
        {
            this.numbers = numbers;
            this.operators = operators;
        }

        public override bool Equals(object obj)
        {            
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            
            Equation e = (Equation) obj;

            return e.operators.SequenceEqual(operators) && e.numbers.SequenceEqual(numbers);
        }
        
        public override int GetHashCode()
        {
            return operators.GetHashCode() + numbers.GetHashCode();
        }

        public bool Validate()
        {
            if (operators.Count(o => o == Operator.Equal) == 1)
            {
                int equalIndex = operators.IndexOf(Operator.Equal);
                long leftside = Calculate(0, equalIndex);
                long rightside = Calculate(equalIndex + 1, numbers.Count - 1);

                if (leftside == rightside)
                {
                    return true;
                }
            }
        
            return false;
        }

        private long Calculate(int startIndex, int endIndex)
        {
            long result = numbers[startIndex];

            if (operators.Count > startIndex && startIndex < endIndex)
            {
                result = Calculate(result, startIndex, endIndex);
            }

            return result;
        }

        private long Calculate(long result, int startIndex, int endIndex)
        {
            if (operators[startIndex] == Operator.Add)
            {
                result += Calculate(startIndex + 1, endIndex);
            }
            else if (operators[startIndex] == Operator.Substract)
            {
                result -= Calculate(startIndex + 1, endIndex);
            }
            else if (operators[startIndex] == Operator.Multiply)
            {
                result *= numbers[startIndex + 1];
                result = Calculate(result, startIndex + 1, endIndex);
            }
            else if (operators[startIndex] == Operator.Divide)
            {
                result /= numbers[startIndex + 1];
                result = Calculate(result, startIndex + 1, endIndex);
            }

            return result;
        }

        public EquationComparison Compare(Equation equationTwo)
        {
            List<ComparisonStatus> numberResults = CompareNumbers(equationTwo);
            List<ComparisonStatus> operatorResults = CompareOperators(equationTwo);

            return new EquationComparison(numberResults, operatorResults);
        }

        private List<ComparisonStatus> CompareOperators(Equation equationTwo)
        {
            var operatorResults = new List<ComparisonStatus>(operators.Count);
            var backupOperators = new List<Operator>();
            for (int i = 0; i < equationTwo.operators.Count; i++)
            {
                if (equationTwo.operators[i] == operators[i])
                {
                    operatorResults.Add(ComparisonStatus.Correct);
                }
                else
                {
                    operatorResults.Add(ComparisonStatus.False);
                    backupOperators.Add(operators[i]);
                }
            }
            for (int i = 0; i < operatorResults.Count; i++)
            {
                if (operatorResults[i] == ComparisonStatus.False && backupOperators.Contains(equationTwo.operators[i]))
                {
                    operatorResults[i] = ComparisonStatus.WrongPlace;
                    backupOperators.Remove(equationTwo.operators[i]);
                }
            }

            return operatorResults;
        }

        private List<ComparisonStatus> CompareNumbers(Equation equation)
        {
            var numberResults = new List<ComparisonStatus>(numbers.Count);
            var backupNumbers = new List<long>();
            for (int i = 0; i < equation.numbers.Count; i++)
            {
                if (equation.numbers[i] == numbers[i])
                {
                    numberResults.Add(ComparisonStatus.Correct);
                }
                else
                {
                    numberResults.Add(ComparisonStatus.False);
                    backupNumbers.Add(numbers[i]);
                }
            }
            for (int i = 0; i < numberResults.Count; i++)
            {
                if (numberResults[i] == ComparisonStatus.False && backupNumbers.Contains(equation.numbers[i]))
                {
                    numberResults[i] = ComparisonStatus.WrongPlace;
                    backupNumbers.Remove(equation.numbers[i]);
                }
            }

            return numberResults;
        }
    }
}