namespace Solver
{
    public interface IGuesser
    {
        Equation Guess(List<EquationComponent[]> remainingPossibilities, int tryCount);
    }
}