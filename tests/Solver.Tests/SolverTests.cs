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
        public void Given_NoInformation_When_Solve_Then_Return_DefaultGuess()
        {
            EquationComponent[] components = new EquationComponent[] {
                Nine, Add, Eight, Substract, Five, Equal, One, Two};

            Equation expected = new Equation(components);
            Equation guess = Solver.Guess(new List<EquationComparison>());

            guess.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Given_NoInformation_When_Solve_Then_Return_DefaultGuessA()
        {
            EquationComponent[] components = new EquationComponent[] {
                Nine, Add, Eight, Substract, Five, Equal, One, Two};
            Equation guess = new Equation(components);

            var comparison = new List<ComparisonStatus>() {
                    ComparisonStatus.False,
                    ComparisonStatus.Correct,
                    ComparisonStatus.False,
                    ComparisonStatus.False,
                    ComparisonStatus.False,
                    ComparisonStatus.Correct,
                    ComparisonStatus.Correct,
                    ComparisonStatus.False
            };
            EquationComparison eqComparison = new EquationComparison(guess, comparison);

            EquationComponent[] componentsExpected = new EquationComponent[] {
                Three, Add, Four, Add, Seven, Equal, One, Four};
            Equation expected = new Equation(componentsExpected);

            Equation result = Solver.Guess(new List<EquationComparison>() { eqComparison });

            expected.Should().BeEquivalentTo(result);
        }
    }
}