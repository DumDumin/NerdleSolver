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
                    var opsComp = componentsCopy.TakeWhile(c => IsOperator(c)).ToList();
                    var opComp = CombineOperators(opsComp);
                    var op = GetOperatorFromEquationComponent(opComp);
                    operators.Add(op);
                    componentsCopy.RemoveRange(0, opsComp.Count);
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

        private EquationComponent CombineOperators(List<EquationComponent> operators)
        {
            // This method can only be called from a validated Equation
            if(operators[0] == Multiply) return Multiply;
            if(operators[0] == Divide) return Divide;
            if(operators[0] == Equal) return Equal;

            if(operators.All(o => o == Add)) return Add;
            if(operators.Count(o => o == Substract) % 2 == 1) return Substract;
            else return Add;
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

        public bool Validate()
        {
            if (ValidateSyntax())
            {
                try
                {  
                    int equalIndex = operators.IndexOf(Operator.Equal);
                    long leftside = Calculate(0, equalIndex);
                    long rightside = Calculate(equalIndex + 1, numbers.Count - 1);

                    if (leftside == rightside)
                    {
                        return true;
                    }
                }
                catch (System.Exception)
                {
                    // TODO probably of "divided by zero"
                    return false;
                }
            }

            return false;
        }

        private bool ValidateSyntax()
        {
            if (ContainsExcatlyOneEqualSign())
            {
                if (StartAndEndSignsAreAllowed())
                {
                    if(ConsecutiveOperatorsAreAllowed())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool ContainsExcatlyOneEqualSign()
        {
            return components.Count(o => o == Equal) == 1;
        }

        private bool StartAndEndSignsAreAllowed()
        {
            return
                components[0] != Multiply &&
                components[0] != Divide &&
                components[0] != Equal &&
                !IsOperator(components[components.Count-1]);
        }

        private bool ConsecutiveOperatorsAreAllowed()
        {
            // no operator is allowed in front of the equal sign
            int equalIndex = components.IndexOf(EquationComponent.Equal);
            if (IsOperator(components[equalIndex - 1]))
            {
                return false;
            }

            for (int i = 0; i < components.Count-1; i++)
            {
                if(IsOperator(components[i]) && IsOperator(components[i+1]))
                {
                    if(
                        components[i] == EquationComponent.Add && components[i+1] == EquationComponent.Multiply ||
                        components[i] == EquationComponent.Multiply && components[i+1] == EquationComponent.Add ||
                        components[i] == EquationComponent.Substract && components[i+1] == EquationComponent.Multiply ||
                        components[i] == EquationComponent.Multiply && components[i+1] == EquationComponent.Substract ||

                        components[i] == EquationComponent.Add && components[i+1] == EquationComponent.Divide ||
                        components[i] == EquationComponent.Divide && components[i+1] == EquationComponent.Add ||
                        components[i] == EquationComponent.Substract && components[i+1] == EquationComponent.Divide ||
                        components[i] == EquationComponent.Divide && components[i+1] == EquationComponent.Substract
                    )
                    {
                        return false;
                    }
                }
            }
            return true;
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

        public override string ToString()
        {
            string result = "";
            foreach (var component in components)
            {
                if(IsOperator(component))
                {
                    if(component == Equal) result += "=";
                    else if(component == Add) result += "+";
                    else if(component == Substract) result += "-";
                    else if(component == Multiply) result += "*";
                    else if(component == Divide) result += "/";
                    else throw new NotImplementedException($"{component} is not an operator");
                }
                else
                {
                    result += ((int)component).ToString();
                }
            }
            return result;
        }
    }
}