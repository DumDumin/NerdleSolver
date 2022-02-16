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
        public void Given_EquationNumberZero_When_GenerateEquation_Then_ReturnCorrectEquation()
        {
            List<EquationComponent> components = new List<EquationComponent>(){
                Zero, Zero, Zero, Zero, Zero, Zero, Zero, Zero};

            Solver.GenerateComponents(0, 8).Should().BeEquivalentTo(components);
        }

        [Test]
        public void Given_EquationNumberOne_When_GenerateEquation_Then_ReturnCorrectEquation()
        {
            List<EquationComponent> components = new List<EquationComponent>(){
                One, Zero, Zero, Zero, Zero, Zero, Zero, Zero};

            Solver.GenerateComponents(1, 8).Should().BeEquivalentTo(components);
        }

        [Test]
        public void Given_DigitCountThree_When_CountValidGuesses_ReturnTen()
        {
            Solver.CountValidGuesses(3).Should().Be(10);
        }

        [Test]
        public void Given_DigitCountFour_When_CountValidGuesses_ReturnTen()
        {
            Solver.CountValidGuesses(4).Should().Be(31);
        }

        [Test]
        public void Given_DigitCountFive_When_CountValidGuesses_ReturnTen()
        {
            Solver.CountValidGuesses(5).Should().Be(516);
        }

        [Test]
        public void Given_NoInformation_When_Solve_Then_Return_DefaultGuess()
        {
            List<EquationComponent> components = new List<EquationComponent>(){
                Nine, Add, Eight, Substract, Five, Equal, One, Two};

            Equation expected = new Equation(components);
            Equation guess = Solver.Guess(new List<EquationComparison>());

            guess.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Given_NoInformation_When_Solve_Then_Return_DefaultGuessA()
        {
            List<EquationComponent> components = new List<EquationComponent>(){
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

            List<EquationComponent> componentsExpected = new List<EquationComponent>(){
                Three, Add, Four, Add, Seven, Equal, One, Four};
            Equation expected = new Equation(componentsExpected);

            Equation result = Solver.Guess(new List<EquationComparison>() { eqComparison });

            expected.Should().BeEquivalentTo(result);
        }
    }
}