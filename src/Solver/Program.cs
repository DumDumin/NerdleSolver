using NLog;
using static Solver.EquationComponent;

namespace Solver;

internal class Program
{
    static void Main(string[] args)
    {
        var config = new NLog.Config.LoggingConfiguration();
        var logconsole = new NLog.Targets.ConsoleTarget("logconsole");
        config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
        NLog.LogManager.Configuration = config;

        Console.Write("Creating possibilities...");
        List<EquationComponent[]> possibilities = Solver.CreateAllValidPossibilities(8);
        Console.WriteLine($"Created {possibilities.Count}");

        Console.WriteLine("Solving...");
        IGuesser guesser = new EightDigitFixedGuesser(possibilities);
        Solver solver = new Solver(guesser, possibilities);
        EquationComponent[] solution = solver.Solve(CompareHuman, out int tries);
        Console.WriteLine($"Solved {Equation.FormatAsString(solution)} in {tries} tries");

        // possibilities = PrintPairsOfGoodFirstAndSecondTry(possibilities);
    }

    private static List<EquationComponent[]> PrintPairsOfGoodFirstAndSecondTry(List<EquationComponent[]> possibilities)
    {
        possibilities = Equation.Filter(possibilities, new List<Rule>()
            {
                (EquationComponent[] eq) => eq.Distinct().Count(c => Equation.IsOperator(c)) == 3,
                (EquationComponent[] eq) => eq.Distinct().Count(c => !Equation.IsOperator(c)) == 5,
            });
        Console.WriteLine($"{possibilities.Count} possibibilities");

        int counter = 0;
        foreach (var possibility in possibilities)
        {
            // filter all elements that are present in the current possibility
            var distinct = possibility.Distinct();
            var rules = new List<Rule>();
            rules.Add((EquationComponent[] eq) => Equation.GetEqualIndex(possibility) == Equation.GetEqualIndex(eq));
            foreach (var item in distinct.Where(c => c != Equal))
            {
                rules.Add((EquationComponent[] eq) => eq.Count(c => c == item) == 0);
            }

            var filtered = Equation.Filter(possibilities, rules);
            foreach (var item in filtered)
            {
                Console.WriteLine(Equation.FormatAsString(possibility));
                Console.WriteLine(Equation.FormatAsString(item));
            }
            counter += filtered.Count;
        }
        Console.WriteLine($"Found {counter} pairs");
        return possibilities;
    }

    static EquationComponent[] eq = new EquationComponent[]
    {
            Add, Three, Add, Three, Multiply, Zero, Equal, Three
    };

    static EquationComparison Compare(EquationComponent[] equation)
    {
        return eq.Compare(equation);
    }

    static EquationComparison CompareHuman(EquationComponent[] guess)
    {
        // the user has to put this guess into nerdlegame.com
        Console.WriteLine(guess);

        // the user has type in the feedback
        string? input = Console.ReadLine();
        return EquationComparison.FromString(input, guess);
    }
}