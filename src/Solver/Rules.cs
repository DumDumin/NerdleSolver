global using Rule = System.Func<Solver.EquationComponent[], bool>;
using static Solver.EquationComponent;

namespace Solver;

public static class Rules
{
    public static Rule ContainsNoZeros = (EquationComponent[] eq) => eq.Count(c => c == Zero) == 0;
}
