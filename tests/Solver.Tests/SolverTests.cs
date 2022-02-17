using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using static Solver.EquationComponent;

namespace Solver.Tests
{
    public class SolverTests
    {
        [Test]
        public void Given_EquationNumberZero_When_GenerateEquation_Then_ReturnCorrectEquation()
        {
            EquationComponent[] components = new EquationComponent[] {
                Zero, Zero, Zero, Zero, Zero, Zero, Zero, Zero};

            Solver.GenerateComponents(0, 8).Should().BeEquivalentTo(components);
        }

        [Test]
        public void Given_EquationNumberOne_When_GenerateEquation_Then_ReturnCorrectEquation()
        {
            EquationComponent[] components = new EquationComponent[] {
                One, Zero, Zero, Zero, Zero, Zero, Zero, Zero};

            Solver.GenerateComponents(1, 8).Should().BeEquivalentTo(components);
        }

        [Test]
        public void Given_DigitCountThree_When_CountValidGuesses_ReturnTen()
        {
            Solver.CountValidGuesses(3).Should().Be(10);
        }

        [Test]
        [Ignore("No Default Guess implemented right now")]
        public void Given_NoInformation_When_Solve_Then_Return_DefaultGuess()
        {
            EquationComponent[] components = new EquationComponent[] {
                Nine, Add, Eight, Substract, Five, Equal, One, Two};

            Equation expected = new Equation(components);
            Equation guess = Solver.Guess(new List<EquationComponent[]>());

            guess.Should().BeEquivalentTo(expected);
        }
    }
}