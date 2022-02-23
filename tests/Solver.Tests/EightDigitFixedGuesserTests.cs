using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using static Solver.EquationComponent;

namespace Solver.Tests;

public class EightDigitFixedGuesserTests
{
    [Test]
    public void Given_FirstTry_When_Guess_Then_ReturnFirstEquation()
    {
        EightDigitFixedGuesser guesser = new EightDigitFixedGuesser(new List<EquationComponent[]>());

        EquationComponent[] guess = guesser.Guess(new List<EquationComponent[]>(), 0);
        guess.Should().BeEquivalentTo(new EquationComponent[] { Five, Three, Add, One, Equal, Nine, Multiply, Six });
    }

    [Test]
    public void Given_SecondTry_When_Guess_Then_ReturnSecondEquation()
    {
        EightDigitFixedGuesser guesser = new EightDigitFixedGuesser(new List<EquationComponent[]>());

        EquationComponent[] guess = guesser.Guess(new List<EquationComponent[]>(), 1);
        guess.Should().BeEquivalentTo(new EquationComponent[] { Two, Eight, Divide, Seven, Equal, Four, Substract, Zero });
    }

    [Test]
    public void Given_ThirdTry_When_Guess_Then_ReturnRemainingEquation()
    {
        EightDigitFixedGuesser guesser = new EightDigitFixedGuesser(new List<EquationComponent[]>());

        var eq = new EquationComponent[] { One, Add, Two, Add, Eight, Equal, One, One };
        EquationComponent[] guess = guesser.Guess(new List<EquationComponent[]>() { eq }, 2);
        guess.Should().BeEquivalentTo(eq);
    }
}