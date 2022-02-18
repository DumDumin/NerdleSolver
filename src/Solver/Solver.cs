using static Solver.EquationComponent;

namespace Solver
{
    public static class Solver
    {
        private static Random rnd = new Random();
        public static Equation Guess(List<EquationComponent[]> possibilities)
        {
            int index = rnd.Next(0, possibilities.Count-1);
            return new Equation(possibilities[index]);
        }

        public static Equation Solve(int digitCount, Func<Equation, EquationComparison> compare, out int tries)
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

                if (Equation.ValidateSyntax(components))
                {
                    if (Equation.Validate(components))
                    {
                        lock (possibilities)
                        {
                            possibilities.Add(components);
                        }
                    }
                }
            });

            List<EquationComparison> comparisons = new List<EquationComparison>();
            while(comparisons.Count < digitCount)
            {
                Equation guess = Guess(possibilities);
                EquationComparison comparison = compare(guess);
                comparisons.Add(comparison);
                if(comparison.Comparison.All(c => c == ComparisonStatus.Correct))
                {
                    tries = comparisons.Count;
                    return guess;
                }
                else
                {
                    Console.WriteLine($"{guess} out if {possibilities.Count} possibilities");
                }

                possibilities = Equation.Filter(possibilities, comparison);
            }

            throw new Exception($"Unable to find a solution in {comparisons.Count} tries");
        }

        public static long CountValidGuesses(int digitCount)
        {
            // Enum count = 15
            // Possible places = 8
            // => 15^8 => base 15 system
            var componentCount = Enum.GetNames(typeof(EquationComponent)).Length;
            long allPossibilities = (long)Math.Pow(componentCount, digitCount);

            long validEquationCount = 0;
            Parallel.For(0, allPossibilities, (i) =>
            {
                EquationComponent[] components = GenerateComponents(i, digitCount);

                if (Equation.ValidateSyntax(components))
                {
                    if (Equation.Validate(components))
                    {
                        Interlocked.Increment(ref validEquationCount);
                    }
                }
            });

            return validEquationCount;
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
}