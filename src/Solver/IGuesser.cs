namespace Solver;

public interface IGuesser
{
    EquationComponent[] Guess(List<EquationComponent[]> remainingPossibilities, int tryCount);
}