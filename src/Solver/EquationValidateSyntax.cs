using static Solver.EquationComponent;

namespace Solver;

public static class EquationValidateSyntax
{
    public static bool ValidateSyntax(this EquationComponent[] components)
    {
        return
            ContainsExcatlyOneEqualSign(components) &&
            StartAndEndSignsAreAllowed(components) &&
            ConsecutiveOperatorsAreAllowed(components) &&
            NoDivisionByZero(components);
    }

    // Optimized for performance
    private static bool NoDivisionByZero(this EquationComponent[] components, int skip = 0)
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
            long number = components.GetNextNumber(ref index);
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
    private static bool ContainsExcatlyOneEqualSign(this EquationComponent[] components)
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

    private static bool StartAndEndSignsAreAllowed(this EquationComponent[] components)
    {
        return
            components[0] != Multiply &&
            components[0] != Divide &&
            components[0] != Equal &&
            !components[components.Length - 1].IsOperator();
    }

    private static bool ConsecutiveOperatorsAreAllowed(this EquationComponent[] components)
    {
        for (int i = 0; i < components.Length - 1; i++)
        {
            if (components[i].IsOperator() && components[i + 1].IsOperator())
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
}