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
            int equalIndex = components.GetEqualIndex();

            long leftside = components.Skip(0).Take(equalIndex).ToArray().Calculate(out bool validLeft);
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
            equation = equation.Skip(startIndex).ToArray().Substitute(out valid);
            startIndex = 0;
            long result = equation.GetNextNumber(ref startIndex);
            if (!valid)
                return result;
            if (op == Substract)
                result = -result;
            result = equation.Calculate(result, startIndex, equation.Length - 1, out valid);
            return result;
        }
        else
        {
            equation = equation.Substitute(out valid);
            long result = equation.GetNextNumber(ref startIndex);
            if (!valid)
                return result;
            result = equation.Calculate(result, startIndex, equation.Length - 1, out valid);
            return result;
        }
    }

    private static long Calculate(this EquationComponent[] equation, long result, int startIndex, int endIndex, out bool valid)
    {
        valid = true;
        if (startIndex >= endIndex) return result;

        var op = equation.GetNextOperator(ref startIndex);
        if (op == Add)
        {
            result += equation.GetNextNumber(ref startIndex);
            result = equation.Calculate(result, startIndex, endIndex, out valid);
        }
        else if (op == Substract)
        {
            result -= equation.GetNextNumber(ref startIndex);
            result = equation.Calculate(result, startIndex, endIndex, out valid);
        }
        else
        {
            throw new NotImplementedException($"{op} was unexpected. {equation.FormatAsString()}");
        }

        return result;
    }

    internal static EquationComponent[] Substitute(this EquationComponent[] components, out bool valid)
    {
        int opIndex = 0;
        valid = true;
        for (int i = 0; i < components.Length; i++)
        {
            if (components[i].IsOperator() && (components[i] == Multiply || components[i] == Divide))
            {
                opIndex = i;
                break;
            }
        }
        var op = components[opIndex];

        if (op == Multiply || op == Divide)
        {
            int prevIndex = opIndex - 1;
            int nextIndex = opIndex + 1;
            var previous = components.GetPreviousNumber(ref prevIndex);
            prevIndex++; // we need to increment it again, to get the index of the previous number starting
            var next = components.GetNextNumber(ref nextIndex);
            int result;
            if (op == Multiply)
                result = previous * next;
            else
            {
                var rest = previous % next;
                if (rest != 0)
                {
                    // End calculation of division is not valid
                    valid = false;
                    return components;
                }
                else
                {
                    result = previous / next;
                }
            }


            EquationComponent[] calc = Equation.FromNumber(result);
            EquationComponent[] sub = new EquationComponent[prevIndex + calc.Length + components.Length - nextIndex];
            for (int i = 0; i < prevIndex; i++)
            {
                sub[i] = components[i];
            }
            for (int i = prevIndex; i < prevIndex + calc.Length; i++)
            {
                sub[i] = calc[i - prevIndex];
            }
            for (int i = nextIndex; i < components.Length; i++)
            {
                sub[prevIndex + calc.Length + i - nextIndex] = components[i];
            }

            if (nextIndex < components.Length && components[nextIndex].IsOperator())
                components = Substitute(sub, out valid);
            else
                components = sub;
        }
        return components;
    }
}