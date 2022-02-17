using static Solver.EquationComponent;


namespace Solver
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Solving...");
            Equation eq = Solver.Solve(8, Compare, out int tries);
            Console.WriteLine($"Solved {eq} in {tries} tries");
        }

        static Equation eq = new Equation(new EquationComponent[]
        {
            Two, Add, Four, Equal, One, Three, Substract, Seven
        });

        static EquationComparison Compare(Equation equation)
        {
            return eq.Compare(equation);
        }
    }
}

