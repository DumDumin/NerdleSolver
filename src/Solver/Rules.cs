using static Solver.EquationComponent;

namespace Solver
{
    using Rule = System.Func<EquationComponent[], bool>;
    public static class Rules
    {
        public static Rule ContainsNoZeros = (EquationComponent[] eq) => eq.Count(c => c == Zero) == 0;
    }
}