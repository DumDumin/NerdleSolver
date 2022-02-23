namespace Solver;

public static class EquationFilter
{
    public static IEnumerable<EquationComponent[]> Filter(this IEnumerable<EquationComponent[]> equations, IEnumerable<Rule> rules)
    {
        return equations.Where(eq => rules.All(rule => rule(eq))).ToList();
    }

    public static List<EquationComponent[]> Filter(this IEnumerable<EquationComponent[]> equations, EquationComparison comparison)
    {
        List<EquationComponent[]> result = new List<EquationComponent[]>();
        foreach (var eq in equations)
        {
            List<EquationComponent> buffer = eq.ToList();
            // check every position of both equations
            if (CheckCorrectAndWrongPlacedComponents(comparison, eq, buffer))
            {
                if (!ContainsNotAllowedComponents(comparison, buffer))
                {
                    result.Add(eq);
                }
            }
        }
        return result;
    }

    private static bool ContainsNotAllowedComponents(EquationComparison comparison, List<EquationComponent> buffer)
    {
        // False positions must be checked after all wrongplaces are checked
        for (int i = 0; i < comparison.Comparison.Count; i++)
        {
            if (comparison.Comparison[i] == ComparisonStatus.False && buffer.Contains(comparison.Equation[i]))
            {
                // TODO TEST return buffer.Contains(comparison.Equation[i])
                // => test should fail
                return true;
            }
        }
        return false;
    }

    private static bool CheckCorrectAndWrongPlacedComponents(EquationComparison comparison, EquationComponent[] eq, List<EquationComponent> buffer)
    {
        for (int i = 0; i < comparison.Comparison.Count; i++)
        {
            if (comparison.Comparison[i] == ComparisonStatus.Correct)
            {
                if (eq[i] == comparison.Equation[i])
                {
                    // Remove the correct positioned component from the buffer
                    // it cannot be used for wrong positioned components anymore
                    buffer.Remove(eq[i]);
                }
                else
                {
                    return false;
                }
            }
            else if (comparison.Comparison[i] == ComparisonStatus.WrongPlace)
            {
                if (eq[i] == comparison.Equation[i])
                {
                    return false;
                }
                else if (!buffer.Contains(comparison.Equation[i]))
                {
                    return false;
                }
                else
                {
                    // Remove the wrong positioned component from the buffer
                    // it cannot be used for other wrong positioned components anymore
                    buffer.Remove(comparison.Equation[i]);
                }
            }
        }

        return true;
    }
}