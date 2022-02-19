using System;
using System.Collections.Generic;
using System.Linq;
using static Solver.EquationComponent;

namespace Solver
{
    public class Equation
    {
        private readonly EquationComponent[] components;

        public Equation(EquationComponent[] components)
        {
            if (!ValidateSyntax(components))
            {
                throw new ArgumentException();
            }

            this.components = components;
        }

        private static EquationComponent GetNextOperator(EquationComponent[] components, ref int index)
        {
            EquationComponent firstOp = Equal;
            for (int i = index; index < components.Length; index++)
            {
                if (IsOperator(components[index]))
                {
                    if (firstOp == Equal)
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
                    return firstOp;
                }
            }

            throw new ArgumentException("Could not find an operator with a following digit");
        }

        public static List<EquationComponent[]> Filter(List<EquationComponent[]> list, EquationComparison comparison)
        {
            List<EquationComponent[]> result = new List<EquationComponent[]>();
            foreach (var eq in list)
            {
                List<EquationComponent> buffer = eq.ToList();
                // check every position of both equations
                int counter = 0;
                for (int i = 0; i < comparison.Comparison.Count; i++)
                {
                    if (comparison.Comparison[i] == ComparisonStatus.Correct)
                    {
                        if (eq[i] != comparison.Equation.components[i])
                        {
                            break;
                        }
                        else
                        {
                            // Remove the correct positioned component from the buffer
                            // it cannot be used for wrong positioned components anymore
                            buffer.Remove(eq[i]);
                        }
                    }
                    else if (comparison.Comparison[i] == ComparisonStatus.WrongPlace)
                    {
                        if (eq[i] == comparison.Equation.components[i])
                        {
                            break;
                        }
                        else if (!buffer.Contains(comparison.Equation.components[i]))
                        {
                            break;
                        }
                        else
                        {
                            // Remove the wrong positioned component from the buffer
                            // it cannot be used for other wrong positioned components anymore
                            buffer.Remove(comparison.Equation.components[i]);
                        }
                    }
                    counter++;
                }
                // False positions must be checked after all wrongplaces are checked
                for (int i = 0; i < comparison.Comparison.Count; i++)
                {
                    if (comparison.Comparison[i] == ComparisonStatus.False && buffer.Contains(comparison.Equation.components[i]))
                    {
                        counter--;
                        break;
                    }
                }
                if (counter == comparison.Comparison.Count)
                {
                    result.Add(eq);
                }
            }
            return result;
        }

        private static EquationComponent CombineOperators(EquationComponent first, EquationComponent second)
        {
            if (first == Add && second == Substract)
                return Substract;
            else if (first == Substract && second == Add)
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

        public static bool Validate(EquationComponent[] components)
        {
            int equalIndex = GetEqualIndex(components);

            long leftside = Calculate(components, 0, equalIndex, out bool validLeft);
            long rightside = Calculate(components, equalIndex + 1, components.Length, out bool validRight);

            if (leftside == rightside && validRight && validLeft)
            {
                return true;
            }

            return false;
        }

        private static int GetEqualIndex(EquationComponent[] components)
        {
            int equalIndex = 0;
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] == Equal)
                {
                    equalIndex = i;
                    break;
                }
            }

            return equalIndex;
        }

        private static long Calculate(EquationComponent[] components, int startIndex, int endIndex, out bool valid)
        {
            if (components.Length > startIndex && startIndex < endIndex)
            {
                valid = true;
                if (IsOperator(components[startIndex]))
                {
                    EquationComponent op = GetNextOperator(components, ref startIndex);
                    long result = GetNextNumber(components, ref startIndex);
                    if (op == Substract)
                        result = -result;
                    result = Calculate(components, result, startIndex, endIndex, out valid);
                    return result;
                }
                else
                {
                    long result = GetNextNumber(components, ref startIndex);
                    result = Calculate(components, result, startIndex, endIndex, out valid);
                    return result;
                }
            }
            else
            {
                valid = false;
                throw new NotImplementedException();
            }
        }

        private static long Calculate(EquationComponent[] components, long result, int startIndex, int endIndex, out bool valid)
        {
            valid = true;
            if (startIndex == endIndex) return result;

            var op = GetNextOperator(components, ref startIndex);
            if (op == Add)
            {
                result += Calculate(components, startIndex, endIndex, out valid);
            }
            else if (op == Substract)
            {
                result -= Calculate(components, startIndex, endIndex, out valid);
            }
            else if (op == Multiply)
            {
                result *= GetNextNumber(components, ref startIndex);
                result = Calculate(components, result, startIndex, endIndex, out valid);
            }
            else if (op == Divide)
            {
                var number = GetNextNumber(components, ref startIndex);
                var rest = result % number;
                if (rest != 0)
                {
                    // End calculation of division is not valid
                    valid = false;
                    return long.MaxValue;
                }
                result /= number;
                result = Calculate(components, result, startIndex, endIndex, out valid);
            }
            else
            {
                throw new NotImplementedException();
            }

            return result;
        }

        public EquationComparison Compare(Equation equationToCheck) => Compare(this, equationToCheck);
        public static EquationComparison Compare(Equation equation, Equation equationToCheck)
        {
            var results = new List<ComparisonStatus>(equation.components.Length);
            var backup = new List<EquationComponent>();
            for (int i = 0; i < equationToCheck.components.Length; i++)
            {
                if (equationToCheck.components[i] == equation.components[i])
                {
                    results.Add(ComparisonStatus.Correct);
                }
                else
                {
                    results.Add(ComparisonStatus.False);
                    backup.Add(equation.components[i]);
                }
            }
            for (int i = 0; i < results.Count; i++)
            {
                if (results[i] == ComparisonStatus.False && backup.Contains(equationToCheck.components[i]))
                {
                    results[i] = ComparisonStatus.WrongPlace;
                    backup.Remove(equationToCheck.components[i]);
                }
            }

            return new EquationComparison(equationToCheck, results);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Equation e = (Equation)obj;

            return e.components.SequenceEqual(components);
        }

        public override int GetHashCode()
        {
            return components.GetHashCode();
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