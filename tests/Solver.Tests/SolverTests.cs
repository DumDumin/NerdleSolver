using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using static Solver.EquationComponent;

namespace Solver.Tests
{
    public class SolverTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Given_NoInformation_When_Solve_Then_Return_DefaultGuess()
        {
            List<EquationComponent> components = new List<EquationComponent>(){
                Nine, Add, Eight, Substract, Five, Equal, One, Two};

            Equation expected = new Equation(components);

            Dictionary<Equation, EquationComparison> info = new Dictionary<Equation, EquationComparison>();
            Equation guess = Solver.Solve(info);

            guess.Should().BeEquivalentTo(expected);
        }

        // [Test]
        // [Ignore("No ready to be implemented")]
        // public void Given_NoInformation_When_Solve_Then_Return_DefaultGuessAAAA()
        // {
        //     Equation expected = new Equation(
        //         new List<long>() { 9, 8, 3, 14 },
        //         new List<Operator>() { Operator.Add, Operator.Substract, Operator.Equal }
        //     );

        //     Dictionary<Equation, EquationComparison> info = new Dictionary<Equation, EquationComparison>();
        //     Equation guess = Solver.Solve(info);
        //     EquationComparison comparison = expected.Compare(guess);
        //     info.Add(guess, comparison);
        //     guess = Solver.Solve(info);

        //     guess.Should().BeEquivalentTo(expected);
        // }
    }
}