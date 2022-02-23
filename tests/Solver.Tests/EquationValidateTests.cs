using FluentAssertions;
using NUnit.Framework;
using static Solver.EquationComponent;

namespace Solver.Tests;

public class EquationValidateTests
{
        [Test]
    public void Given_MultiplyAsLastOperatorAndNotValid_When_Validate_Then_Return_False()
    {
        EquationComponent[] components = new EquationComponent[] {
                Five, Zero, Equal, Zero, Multiply, Zero};
        components.Validate().Should().BeFalse();
    }

    [Test]
    public void Given_MultiplyAsLastOperatorAndNotValid_When_Validate_Then_Return_FalseAAAA()
    {
        EquationComponent[] components = new EquationComponent[] {
                Nine,Equal,Two,One,Substract,Seven,Add,Five};
        components.Validate().Should().BeFalse();
    }

    [Test]
    public void Given_MultiplyAsLastOperatorAndValid_When_Validate_Then_Return_True()
    {
        EquationComponent[] components = new EquationComponent[] {
                Five, Zero, Equal, One, Zero, Multiply, Five};
        components.Validate().Should().BeTrue();
    }

    [Test]
    public void Given_SubstractBehindEqual_When_Validate_Then_Return_False()
    {
        EquationComponent[] components = new EquationComponent[] {
                Seven, Equal, Substract, Seven};
        components.Validate().Should().BeFalse();
    }

    [Test]
    public void Given_EquationStartsWithSubstract_When_Validate_Then_ReturnTrue()
    {
        EquationComponent[] components = new EquationComponent[]{
                Substract, One, Add, One, Equal, Zero};
        components.Validate().Should().BeTrue();
    }

    [Test]
    public void Given_OneAddSubstractOneEqualsTwo_When_Validate_Then_Return_True()
    {
        EquationComponent[] components = new EquationComponent[] {
                One, Add, Substract, One, Equal, Zero};

        components.Validate().Should().BeTrue();
    }

    [Test]
    public void Given_SixPlusSevenEqualsThirteen_When_Validate_Then_Return_True()
    {
        EquationComponent[] components = new EquationComponent[] {
                Six, Add, Seven, Equal, One, Three};

        components.Validate().Should().BeTrue();
    }

    [Test]
    public void Given_AllowedEquation_When_Validate_Then_ReturnTrue()
    {
        EquationComponent[] components = new EquationComponent[] {
                One, Equal, One};

        components.Validate().Should().BeTrue();
    }

    [Test]
    public void Given_NotAllowedEquation_When_Validate_Then_Return_False()
    {
        EquationComponent[] components = new EquationComponent[] {
                One, Equal, Two};

        components.Validate().Should().BeFalse();
    }

    [Test]
    public void Given_OnePlusOneEqualsTwo_When_Validate_Then_Return_True()
    {
        EquationComponent[] components = new EquationComponent[] {
                One, Add, One, Equal, Two};

        components.Validate().Should().BeTrue();
    }

    [Test]
    public void Given_OneMinusOneEqualsZero_When_Validate_Then_Return_True()
    {
        EquationComponent[] components = new EquationComponent[] {
                One, Substract, One, Equal, Zero};

        components.Validate().Should().BeTrue();
    }

    [Test]
    public void Given_OnePlusOnePlusOneEqualsThree_When_Validate_Then_Return_True()
    {
        EquationComponent[] components = new EquationComponent[] {
                One, Add, One, Add, One, Equal, Three};

        components.Validate().Should().BeTrue();
    }

    [Test]
    public void Given_OnePlusOneEqualsOnePlusOne_When_Validate_Then_Return_True()
    {
        EquationComponent[] components = new EquationComponent[] {
                One, Add, One, Equal, One, Add, One};

        components.Validate().Should().BeTrue();
    }

    [Test]
    public void Given_TwoTimesTwoEqualsFour_When_Validate_Then_Return_True()
    {
        EquationComponent[] components = new EquationComponent[] {
                Two, Multiply, Two, Equal, Four};

        components.Validate().Should().BeTrue();
    }

    [Test]
    public void Given_TwoTimesTwoTimesTwoEqualsEight_When_Validate_Then_Return_True()
    {
        EquationComponent[] components = new EquationComponent[] {
                Two, Multiply, Two, Multiply, Two, Equal, Eight};

        components.Validate().Should().BeTrue();
    }

    [Test]
    public void Given_OnePlusTwoTimesTwoEqualsFive_When_Validate_Then_Return_True()
    {
        EquationComponent[] components = new EquationComponent[] {
                One, Add, Two, Multiply, Two, Equal, Five};

        components.Validate().Should().BeTrue();
    }

    [Test]
    public void Given_TwoTimesTwoPlusTwoEqualsSix_When_Validate_Then_Return_True()
    {
        EquationComponent[] components = new EquationComponent[] {
                Two, Multiply, Two, Add, Two, Equal, Six};

        components.Validate().Should().BeTrue();
    }

    [Test]
    public void Given_FourTimesTwoDividedByEightEqualsOne_When_Validate_Then_Return_True()
    {
        EquationComponent[] components = new EquationComponent[] {
                Four, Multiply, Two, Divide, Eight, Equal, One};

        components.Validate().Should().BeTrue();
    }
}