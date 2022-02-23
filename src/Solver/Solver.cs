using static Solver.EquationComponent;

namespace Solver;

public class Solver
{
    private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

    private readonly IGuesser guesser;
    private readonly List<EquationComponent[]> allPossibilities;

    public Solver(IGuesser guesser, List<EquationComponent[]> allPossibilities)
    {
        this.guesser = guesser;
        this.allPossibilities = allPossibilities;
    }

    public EquationComponent[] Solve(Func<EquationComponent[], EquationComparison> compare, out int tries)
    {
        List<EquationComponent[]> possibilities = new List<EquationComponent[]>(allPossibilities);

        List<EquationComparison> comparisons = new List<EquationComparison>();
        while (comparisons.Count < 10)
        {
            EquationComponent[] guess = guesser.Guess(possibilities, comparisons.Count);
            EquationComparison comparison = compare(guess);
            comparisons.Add(comparison);
            if (comparison.Comparison.All(c => c == ComparisonStatus.Correct))
            {
                tries = comparisons.Count;
                return guess;
            }
            else
            {
                logger.Info($"{guess} out of {possibilities.Count} possibilities");
            }

            possibilities = possibilities.Filter(comparison);
        }

        throw new Exception($"Unable to find a solution in {comparisons.Count} tries");
    }

    public static List<EquationComponent[]> CreateAllValidPossibilities(int digitCount)
    {
        // Enum count = 15
        // Possible places = 8
        // => 15^8 => base 15 system
        var componentCount = Enum.GetNames(typeof(EquationComponent)).Length;
        long allPossibilities = (long)Math.Pow(componentCount, digitCount);

        List<EquationComponent[]> possibilities = new List<EquationComponent[]>();
        Parallel.For(0, allPossibilities, (i) =>
        {
            EquationComponent[] components = GenerateComponents(i, digitCount);

            if (components.ValidateSyntax())
            {
                if (components.Validate())
                {
                    lock (possibilities)
                    {
                        possibilities.Add(components);
                    }
                }
            }
        });
        return possibilities;
    }

    public static Dictionary<EquationComponent, int> CountComponents(List<EquationComponent[]> possibilities)
    {
        Dictionary<EquationComponent, int> result = new Dictionary<EquationComponent, int>();
        foreach (EquationComponent item in Enum.GetValues(typeof(EquationComponent)))
        {
            result.Add(item, 0);
        }
        foreach (var eq in possibilities)
        {
            // foreach (EquationComponent item in eq)
            // {
            //     result[item]++;
            // }
            foreach (EquationComponent item in Enum.GetValues(typeof(EquationComponent)))
            {
                if (eq.Contains(item))
                {
                    result[item]++;
                }
            }
        }
        return result;
    }

    public static EquationComponent[] GenerateComponents(long equationNumber, int digitCount)
    {
        EquationComponent[] components = new EquationComponent[digitCount];
        GetNextDigit(equationNumber, digitCount, components);
        return components;
    }

    // Optimized for performance
    private static void GetNextDigit(long equationNumber, int digitCount, EquationComponent[] components)
    {
        // Algorithm:
        // Enum = i % 15 
        // i = i / 15
        // Recursion

        long result = Math.DivRem(equationNumber, 15, out long rest);
        components[--digitCount] = (EquationComponent)(rest);
        if (digitCount > 0)
        {
            GetNextDigit(result, digitCount, components);
        }
    }
}