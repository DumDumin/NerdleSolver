using System;
using System.Collections.Generic;
using System.Linq;
using static Solver.EquationComponent;

namespace Solver
{
    public class Equation
    {
        private List<long> numbers;
        private List<Operator> operators;
        private readonly IList<EquationComponent> components;

        public Equation(IList<EquationComponent> components)
        {
            this.components = components;

            this.numbers = new List<long>();
            this.operators = new List<Operator>();

            TranslateComponents();
        }

        private void TranslateComponents()
        {
            var componentsCopy = components.ToList();

            while(componentsCopy.Count > 0)
            {
                var digits = componentsCopy.TakeWhile(c => !IsOperator(c)).ToList();
                var number = GetNumberFromDigits(digits);
                numbers.Add(number);
                componentsCopy.RemoveRange(0, digits.Count);

                if(componentsCopy.Count > 0)
                {
                    var opComp = componentsCopy[0];
                    var op = GetOperatorFromEquationComponent(opComp);
                    operators.Add(op);
                    componentsCopy.RemoveAt(0);
                }
            }
        }

        private bool IsOperator(EquationComponent component)
        {
            if (component == Equal || component == Add || component == Substract || component == Multiply || component == Divide)
                return true;
            else
                return false;
        }

        private long GetNumberFromDigits(List<EquationComponent> digits)
        {
            long result = 0;
            for (int i = 0; i < digits.Count; i++)
            {
                result += (long)digits[i] * (long)Math.Pow(10, digits.Count - 1 - i);
            }
            return result;
        }

        private Operator GetOperatorFromEquationComponent(EquationComponent comp)
        {
            if(comp == Equal) return Operator.Equal;
            if(comp == Add) return Operator.Add;
            if(comp == Substract) return Operator.Substract;
            if(comp == Multiply) return Operator.Multiply;
            if(comp == Divide) return Operator.Divide;
            else throw new NotImplementedException($"{comp} is not an operator");
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Equation e = (Equation)obj;

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

        public EquationComparison Compare(Equation equation)
        {
            var results = new List<ComparisonStatus>(components.Count);
            var backup = new List<EquationComponent>();
            for (int i = 0; i < equation.components.Count; i++)
            {
                if (equation.components[i] == components[i])
                {
                    results.Add(ComparisonStatus.Correct);
                }
                else
                {
                    results.Add(ComparisonStatus.False);
                    backup.Add(components[i]);
                }
            }
            for (int i = 0; i < results.Count; i++)
            {
                if (results[i] == ComparisonStatus.False && backup.Contains(equation.components[i]))
                {
                    results[i] = ComparisonStatus.WrongPlace;
                    backup.Remove(equation.components[i]);
                }
            }

            return new EquationComparison(equation, results);
        }
    }
}