using static Solver.EquationComponent;

namespace Solver;

public class RandomGuesser : IGuesser
{
    private Random rnd = new Random();

    public RandomGuesser(List<EquationComponent[]> allPossibilites)
    {
    }

    public EquationComponent[] Guess(List<EquationComponent[]> remainingPossibilities, int tryCount)
    {
        int index = rnd.Next(0, remainingPossibilities.Count - 1);
        return remainingPossibilities[index];
    }
}