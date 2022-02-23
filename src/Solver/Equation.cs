using static Solver.EquationComponent;

namespace Solver;

public static class Equation
{
    public static EquationComparison Compare(this EquationComponent[] equation, EquationComponent[] equationToCheck)
    {
        var results = new List<ComparisonStatus>(equation.Length);
        var backup = new List<EquationComponent>();
        for (int i = 0; i < equationToCheck.Length; i++)
        {
            if (equationToCheck[i] == equation[i])
            {
                results.Add(ComparisonStatus.Correct);
            }
            else
            {
                results.Add(ComparisonStatus.False);
                backup.Add(equation[i]);
            }
        }
        for (int i = 0; i < results.Count; i++)
        {
            if (results[i] == ComparisonStatus.False && backup.Contains(equationToCheck[i]))
            {
                results[i] = ComparisonStatus.WrongPlace;
                backup.Remove(equationToCheck[i]);
            }
        }

        return new EquationComparison(equationToCheck, results);
    }

    public static string FormatAsString(this EquationComponent[] components)
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

    public static EquationComponent[] FromString(string eq)
    {
        EquationComponent[] components = new EquationComponent[eq.Length];
        for (int i = 0; i < eq.Length; i++)
        {
            if (eq[i] == '=') components[i] = Equal;

            else if (eq[i] == '+') components[i] = Add;
            else if (eq[i] == '-') components[i] = Substract;
            else if (eq[i] == '*') components[i] = Multiply;
            else if (eq[i] == '/') components[i] = Divide;

            else if (eq[i] == '0') components[i] = Zero;
            else if (eq[i] == '1') components[i] = One;
            else if (eq[i] == '2') components[i] = Two;
            else if (eq[i] == '3') components[i] = Three;
            else if (eq[i] == '4') components[i] = Four;
            else if (eq[i] == '5') components[i] = Five;
            else if (eq[i] == '6') components[i] = Six;
            else if (eq[i] == '7') components[i] = Seven;
            else if (eq[i] == '8') components[i] = Eight;
            else if (eq[i] == '9') components[i] = Nine;

            else throw new NotImplementedException($"{eq[i]} cannot be parsed");
        }
        return components;
    }

    // TODO return equal if next component is a number
    internal static EquationComponent GetNextOperator(this EquationComponent[] components, ref int index)
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

    private static EquationComponent CombineOperators(EquationComponent first, EquationComponent second)
    {
        if (first == Add && second == Substract)
            return Substract;
        else if (first == Substract && second == Add)
            return Substract;
        else
            return Add;
    }
    
    internal static bool IsOperator(this EquationComponent component)
    {
        if (component == Equal || component == Add || component == Substract || component == Multiply || component == Divide)
            return true;
        else
            return false;
    }

    internal static int GetNextNumber(this EquationComponent[] components, ref int index)
    {
        int number = 0;
        for (int i = index; index < components.Length; index++)
        {
            if (!components[index].IsOperator())
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

    internal static EquationComponent[] FromNumber(int number)
    {
        string digits = "";
        while (true)
        {
            digits = (number % 10).ToString() + digits;
            number = number / 10;
            if (number == 0)
                break;
        }
        return FromString(digits);
    }

    internal static int GetComponentIndex(this EquationComponent[] components, params EquationComponent[] targets)
    {
        int equalIndex = -1;
        for (int i = 0; i < components.Length; i++)
        {
            if (targets.Contains(components[i]))
            {
                equalIndex = i;
                break;
            }
        }

        return equalIndex;
    }

    internal static int GetPreviousNumber(this EquationComponent[] components, ref int index)
    {
        int number = 0;
        int idx = index;
        for (int i = 0; i <= idx; i++)
        {
            if (!IsOperator(components[idx - i]))
            {
                number = number + (int)components[idx - i] * (int)Math.Pow(10, i);
                index--;
            }
            else
            {
                break;
            }
        }
        return number;
    }
}