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
        private readonly EquationComponent[] components;

        public Equation(EquationComponent[] components)
        {
            if (!ValidateSyntax(components))
            {
                throw new ArgumentException();
            }

            this.components = components;

            this.numbers = new List<long>();
            this.operators = new List<Operator>();

            TranslateComponents();
        }

        // Optimized for performance
        private void TranslateComponents()
        {
            int index = 0;
            while (index < components.Length)
            {
                if (IsOperator(components[index]))
                {
                    Operator op = GetNextOperator(ref index);
                    int number = GetNextNumber(components, ref index);
                    if (op == Operator.Substract)
                        numbers.Add(-number);
                    else
                        numbers.Add(number);
                }
                else
                {
                    int number = GetNextNumber(components, ref index);
                    numbers.Add(number);
                }


                if (index < components.Length)
                {
                    Operator op = GetNextOperator(ref index);
                    operators.Add(op);
                }
            }
        }

        private Operator GetNextOperator(ref int index)
        {
            EquationComponent firstOp = Equal;
            for (int i = index; index < components.Length; index++)
            {
                if (IsOperator(components[index]))
                {
                    if(firstOp == Equal)
                    {
                        firstOp = components[index];
                    }
                    else
                    {
                        firstOp = CombineOperators(firstOp, components[index]);
                    }
                    if (firstOp == Equal)
                    {
                        // Equal sign should not be combined with other operators
                        index++;
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            var op = GetOperatorFromEquationComponent(firstOp);
            return op;
        }

        private EquationComponent CombineOperators(EquationComponent first, EquationComponent second)
        {
            if(first == Add && second == Substract)
                return Substract;
            else if(first == Substract && second == Add)
                return Substract;
            else
                return Add;
        }

        private static int GetNextNumber(EquationComponent[] components, ref int index)
        {
            int number = 0;
            for (int i = index; index < components.Length; index++)
            {
                if (!IsOperator(components[index]))
                {
                    number = number * 10 + (int)components[index];
                }
                else
                {
                    break;
                }
            }
            return number;
        }

        private static bool IsOperator(EquationComponent component)
        {
            if (component == Equal || component == Add || component == Substract || component == Multiply || component == Divide)
                return true;
            else
                return false;
        }

        private Operator GetOperatorFromEquationComponent(EquationComponent comp)
        {
            if (comp == Equal) return Operator.Equal;
            if (comp == Add) return Operator.Add;
            if (comp == Substract) return Operator.Substract;
            if (comp == Multiply) return Operator.Multiply;
            if (comp == Divide) return Operator.Divide;
            else throw new NotImplementedException($"{comp} is not an operator");
        }

        public static bool ValidateSyntax(EquationComponent[] components)
        {
            return
                ContainsExcatlyOneEqualSign(components) &&
                StartAndEndSignsAreAllowed(components) &&
                ConsecutiveOperatorsAreAllowed(components) &&
                NoDivisionByZero(components);
        }

        // Optimized for performance
        private static bool NoDivisionByZero(EquationComponent[] components, int skip = 0)
        {
            // TODO create find operator method
            int index = -1;
            for (int i = skip; i < components.Length; i++)
            {
                if (components[i] == Divide)
                {
                    // index point in digit behind divide sign
                    index = i + 1;
                    break;
                }
            }

            if (index != -1)
            {
                long number = GetNextNumber(components, ref index);
                if (number == 0)
                {
                    return false;
                }
                else
                {
                    // as long we found a division, we try to find another one
                    return NoDivisionByZero(components, index);
                }
            }

            return true;
        }

        // Optimized for performance
        // The following LINQ statement is too slow
        // return components.Count(o => o == Equal) == 1;
        private static bool ContainsExcatlyOneEqualSign(EquationComponent[] components)
        {
            int counter = 0;
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] == Equal) counter++;
                if (counter == 2) return false;
            }
            if (counter == 1) return true;
            else return false;
        }

        private static bool StartAndEndSignsAreAllowed(EquationComponent[] components)
        {
            return
                components[0] != Multiply &&
                components[0] != Divide &&
                components[0] != Equal &&
                !IsOperator(components[components.Length - 1]);
        }

        private static bool ConsecutiveOperatorsAreAllowed(EquationComponent[] components)
        {
            for (int i = 0; i < components.Length - 1; i++)
            {
                if (IsOperator(components[i]) && IsOperator(components[i + 1]))
                {
                    if (
                        // no operator is allowed in front of the equal sign
                        components[i + 1] == EquationComponent.Equal ||
                        components[i] == EquationComponent.Equal && components[i + 1] == EquationComponent.Multiply ||
                        components[i] == EquationComponent.Equal && components[i + 1] == EquationComponent.Divide ||

                        components[i] == EquationComponent.Add && components[i + 1] == EquationComponent.Multiply ||
                        components[i] == EquationComponent.Multiply && components[i + 1] == EquationComponent.Add ||
                        components[i] == EquationComponent.Substract && components[i + 1] == EquationComponent.Multiply ||
                        components[i] == EquationComponent.Multiply && components[i + 1] == EquationComponent.Substract ||

                        components[i] == EquationComponent.Add && components[i + 1] == EquationComponent.Divide ||
                        components[i] == EquationComponent.Divide && components[i + 1] == EquationComponent.Add ||
                        components[i] == EquationComponent.Substract && components[i + 1] == EquationComponent.Divide ||
                        components[i] == EquationComponent.Divide && components[i + 1] == EquationComponent.Substract ||

                        components[i] == EquationComponent.Multiply && components[i + 1] == EquationComponent.Divide ||
                        components[i] == EquationComponent.Divide && components[i + 1] == EquationComponent.Multiply ||
                        components[i] == EquationComponent.Multiply && components[i + 1] == EquationComponent.Multiply ||
                        components[i] == EquationComponent.Divide && components[i + 1] == EquationComponent.Divide
                    )
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool Validate()
        {
            int equalIndex = operators.IndexOf(Operator.Equal);
            long leftside = Calculate(0, equalIndex);
            long rightside = Calculate(equalIndex + 1, numbers.Count - 1);

            if (leftside == rightside)
            {
                return true;
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
            if (startIndex == endIndex) return result;

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
            var results = new List<ComparisonStatus>(components.Length);
            var backup = new List<EquationComponent>();
            for (int i = 0; i < equation.components.Length; i++)
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
                if (IsOperator(component))
                {
                    if (component == Equal) result += "=";
                    else if (component == Add) result += "+";
                    else if (component == Substract) result += "-";
                    else if (component == Multiply) result += "*";
                    else if (component == Divide) result += "/";
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