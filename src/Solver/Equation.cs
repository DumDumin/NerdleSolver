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
    }
}