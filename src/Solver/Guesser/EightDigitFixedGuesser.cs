using static Solver.EquationComponent;

namespace Solver
{
    public class EightDigitFixedGuesser : IGuesser
    {
        private List<EquationComponent[]> allPossibilites;

        public EightDigitFixedGuesser(List<EquationComponent[]> allPossibilites)
        {
            this.allPossibilites = allPossibilites;
        }

        public Equation Guess(List<EquationComponent[]> remainingPossibilities, int tryCount)
        {
            if(tryCount == 0)
                return new Equation(new EquationComponent[]{Eight,Divide,Four,Multiply,Three,Equal,Zero,Six});
            else if(tryCount == 1)
                return new Equation(new EquationComponent[]{Nine,Equal,Two,One,Substract,Seven,Add,Five});
            else
                return new Equation(remainingPossibilities
                    .OrderByDescending(components => components.Distinct().Count())
                    .ThenByDescending(components => components.Count(c => c == Zero))
                    .ThenByDescending(components => components.Count(c => c == Add))
                    .ThenByDescending(components => components.Count(c => c == Substract))
                    .ThenByDescending(components => components.Count(c => c == Multiply))
                    .ThenByDescending(components => components.Count(c => c == One))
                    .ThenByDescending(components => components.Count(c => c == Two))
                    .ThenByDescending(components => components.Count(c => c == Three))
                    .ThenByDescending(components => components.Count(c => c == Four))
                    .ThenByDescending(components => components.Count(c => c == Six))
                    .ThenByDescending(components => components.Count(c => c == Five))
                    .ThenByDescending(components => components.Count(c => c == Seven))
                    .ThenByDescending(components => components.Count(c => c == Eight))
                    .ThenByDescending(components => components.Count(c => c == Nine))
                    .First());
        }
    }
}