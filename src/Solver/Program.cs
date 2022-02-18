using static Solver.EquationComponent;


namespace Solver
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Console.WriteLine($"Count {Solver.CountValidGuesses(8)} possibilities");
            Console.WriteLine("Solving...");
            Equation solution = Solver.Solve(8, Compare, out int tries);
            Console.WriteLine($"Solved {solution} in {tries} tries");
        }

        static Equation eq = new Equation(new EquationComponent[]
        {
            Zero, Zero, Add, Zero, Equal, Five, Substract, Five
        });

        static EquationComparison Compare(Equation equation)
        {
            return eq.Compare(equation);
        }
    }
}

