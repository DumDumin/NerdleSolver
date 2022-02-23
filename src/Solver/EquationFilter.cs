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
            int counter = 0;
            for (int i = 0; i < comparison.Comparison.Count; i++)
            {
                if (comparison.Comparison[i] == ComparisonStatus.Correct)
                {
                    if (eq[i] != comparison.Equation[i])
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
                    if (eq[i] == comparison.Equation[i])
                    {
                        break;
                    }
                    else if (!buffer.Contains(comparison.Equation[i]))
                    {
                        break;
                    }
                    else
                    {
                        // Remove the wrong positioned component from the buffer
                        // it cannot be used for other wrong positioned components anymore
                        buffer.Remove(comparison.Equation[i]);
                    }
                }
                counter++;
            }
            // False positions must be checked after all wrongplaces are checked
            for (int i = 0; i < comparison.Comparison.Count; i++)
            {
                if (comparison.Comparison[i] == ComparisonStatus.False && buffer.Contains(comparison.Equation[i]))
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

}