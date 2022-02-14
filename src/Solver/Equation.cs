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
            var numberResults = new List<ComparisonStatus>(numbers.Count);
            for (int i = 0; i < equationTwo.numbers.Count; i++)
            {
                if(equationTwo.numbers[i] == numbers[i])
                {
                    numberResults.Add(ComparisonStatus.Correct);
                }
                else
                {
                    numberResults.Add(ComparisonStatus.False);
                }
            }

            var operatorResults = new List<ComparisonStatus>(operators.Count);
            for (int i = 0; i < equationTwo.operators.Count; i++)
            {
                if(equationTwo.operators[i] == operators[i])
                {
                    operatorResults.Add(ComparisonStatus.Correct);
                }
                else
                {
                    operatorResults.Add(ComparisonStatus.False);
                }
            }

            return new EquationComparison(numberResults, operatorResults);
        }
    }
}