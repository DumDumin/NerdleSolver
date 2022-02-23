using static Solver.EquationComponent;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Solver.Tests")]
namespace Solver;

public static class EquationValidate
{
    public static bool Validate(this EquationComponent[] components)
    {
        try
        {
            int equalIndex = components.GetComponentIndex(Equal);

            long leftside = components.Take(equalIndex).ToArray().Calculate(out bool validLeft);
            long rightside = components.Skip(equalIndex + 1).Take(components.Length - equalIndex + 1).ToArray().Calculate(out bool validRight);

            if (leftside == rightside && validRight && validLeft)
            {
                return true;
            }

            return false;
        }
        catch (System.Exception e)
        {
            throw new Exception($"Failed on {components.FormatAsString()}", e);
        }
    }

    private static long Calculate(this EquationComponent[] equation, out bool valid)
    {
        int startIndex = 0;
        valid = true;
        if (equation[startIndex].IsOperator())
        {
            EquationComponent op = equation.GetNextOperator(ref startIndex);
            equation = equation.Skip(startIndex).ToArray();
            startIndex = 0;
            return equation.CalculateWithSubstitution(startIndex, out valid, op == Substract);
        }
        else
        {
            return equation.CalculateWithSubstitution(startIndex, out valid);
        }
    }

    private static long CalculateWithSubstitution(this EquationComponent[] equation, int startIndex, out bool valid, bool minus = false)
    {
        equation = equation.Substitute(out valid);
        long result = equation.GetNextNumber(ref startIndex);
        if (!valid)
            return result;
        if (minus)
            result = -result;
        result = equation.CalculateNextSum(result, startIndex, equation.Length - 1, out valid);
        return result;
    }

    private static long CalculateNextSum(this EquationComponent[] equation, long result, int startIndex, int endIndex, out bool valid)
    {
        valid = true;
        if (startIndex >= endIndex) return result;

        var op = equation.GetNextOperator(ref startIndex);
        if (op == Add)
        {
            result += equation.GetNextNumber(ref startIndex);
            result = equation.CalculateNextSum(result, startIndex, endIndex, out valid);
        }
        else if (op == Substract)
        {
            result -= equation.GetNextNumber(ref startIndex);
            result = equation.CalculateNextSum(result, startIndex, endIndex, out valid);
        }
        else
        {
            throw new NotImplementedException($"{op} was unexpected. {equation.FormatAsString()}");
        }

        return result;
    }

    private static int CalculateFactor(EquationComponent op, int first, int second, out bool valid)
    {
        valid = true;
        if (op == Multiply)
        {
            return first * second;
        }
        else
        {
            var rest = first % second;
            if (rest != 0)
            {
                // End calculation of division is not valid
                valid = false;
            }
            return first / second;
        }
    }

    internal static EquationComponent[] Substitute(this EquationComponent[] components, out bool valid)
    {
        valid = true;
        int opIndex = components.GetComponentIndex(Multiply, Divide);

        if (opIndex != -1)
        {
            int prevIndex = opIndex - 1;
            int nextIndex = opIndex + 1;
            var previous = components.GetPreviousNumber(ref prevIndex);
            prevIndex++; // we need to increment it again, to get the index of the previous number starting
            var next = components.GetNextNumber(ref nextIndex);

            int result = CalculateFactor(components[opIndex], previous, next, out valid);
            if (!valid)
                return components;

            EquationComponent[] sub = CreateSubstitutionEquation(components, prevIndex, nextIndex, result);
            if (nextIndex < components.Length && components[nextIndex].IsOperator())
                components = Substitute(sub, out valid);
            else
                components = sub;
        }
        return components;
    }

    private static EquationComponent[] CreateSubstitutionEquation(EquationComponent[] components, int prevIndex, int nextIndex, int substituation)
    {
        EquationComponent[] calc = Equation.FromNumber(substituation);
        EquationComponent[] sub = new EquationComponent[prevIndex + calc.Length + components.Length - nextIndex];
        for (int i = 0; i < prevIndex; i++)
        {
            // move all components, which are in front of the substitution
            sub[i] = components[i];
        }
        for (int i = prevIndex; i < prevIndex + calc.Length; i++)
        {
            // add the substitution
            sub[i] = calc[i - prevIndex];
        }
        for (int i = nextIndex; i < components.Length; i++)
        {
            // move all components, which are behind the substitution
            sub[prevIndex + calc.Length + i - nextIndex] = components[i];
        }

        return sub;
    }
}