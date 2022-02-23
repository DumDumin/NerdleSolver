using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Solver;
using static Solver.EquationComponent;

namespace Solver.Tests;

public class EquationValidateSyntaxTests
{

    [Test]
    public void Given_NotAllowedEquation_When_Validate_Then_ReturnFalse()
    {
        EquationComponent[] components = new EquationComponent[0];
        components.ValidateSyntax().Should().BeFalse();
    }

    [Test]
    public void Given_DivideByZero_When_Validate_Then_ReturnFalse()
    {
        EquationComponent[] components = new EquationComponent[] {
                One, Divide, Zero, Equal, One};
        components.ValidateSyntax().Should().BeFalse();
    }

    [Test]
    public void Given_DivideByZeroTwoTimes_When_Validate_Then_ReturnFalse()
    {
        EquationComponent[] components = new EquationComponent[] {
                One, Divide, One, Divide, Zero, Equal, One};
        components.ValidateSyntax().Should().BeFalse();
    }

    [Test]
    public void Given_EquationStartsWithMultiply_When_Validate_Then_ReturnFalse()
    {
        EquationComponent[] components = new EquationComponent[] {
                Multiply, One, Equal, One};
        components.ValidateSyntax().Should().BeFalse();
    }

    [Test]
    public void Given_EquationStartsWithDivide_When_Validate_Then_ReturnFalse()
    {
        EquationComponent[] components = new EquationComponent[]
        {
                Divide, One, Equal, One
        };
        components.ValidateSyntax().Should().BeFalse();
    }

    [Test]
    public void Given_EquationStartsWithEqual_When_Validate_Then_ReturnFalse()
    {
        EquationComponent[] components = new EquationComponent[]
        {
                Equal, One
        };
        components.ValidateSyntax().Should().BeFalse();
    }

    [Test]
    public void Given_EquationStartsWithAdd_When_Validate_Then_ReturnTrue()
    {
        EquationComponent[] components = new EquationComponent[]
        {
                Add, One, Equal, One
        };
        components.ValidateSyntax().Should().BeTrue();
    }

    [Test]
    public void Given_EquationStartsWithSubstract_When_Validate_Then_ReturnTrue()
    {
        EquationComponent[] components = new EquationComponent[]
        {
                Substract, One, Add, One, Equal, Zero
        };
        components.ValidateSyntax().Should().BeTrue();
    }

    [Test]
    public void Given_OperatorInFrontOfEqual_When_Validate_Then_ReturnFalse()
    {
        EquationComponent[] components = new EquationComponent[]
        {
                Zero, Add, Equal, Zero
        };
        components.ValidateSyntax().Should().BeFalse();
    }

    [Test]
    public void Given_AddOperatorBehindOfEqual_When_Validate_Then_ReturnTrue()
    {
        EquationComponent[] components = new EquationComponent[]
        {
                Zero, Equal, Add, Zero
        };
        components.ValidateSyntax().Should().BeTrue();
    }

    [Test]
    public void Given_SubsctractOperatorBehindOfEqual_When_Validate_Then_ReturnTrue()
    {
        EquationComponent[] components = new EquationComponent[]
        {
                Zero, Equal, Substract, Zero
        };
        components.ValidateSyntax().Should().BeTrue();
    }

    [Test]
    public void Given_MultiplyOperatorBehindOfEqual_When_Validate_Then_ReturnFalse()
    {
        EquationComponent[] components = new EquationComponent[]
        {
                Zero, Equal, Multiply, Zero
        };
        components.ValidateSyntax().Should().BeFalse();
    }

    [Test]
    public void Given_DivideOperatorBehindOfEqual_When_Validate_Then_ReturnFalse()
    {
        EquationComponent[] components = new EquationComponent[]
        {
                Zero, Equal, Divide, One
        };
        components.ValidateSyntax().Should().BeFalse();
    }

    [Test]
    public void Given_EquationEndsWithOperator_When_Validate_Then_ReturnFalse()
    {
        EquationComponent[] components = new EquationComponent[]
        {
                One, Add, One, Equal, Two, Substract
        };
        components.ValidateSyntax().Should().BeFalse();
    }

    [Test]
    public void Given_MultipleEqualOperators_When_Validate_Then_ReturnFalse()
    {
        EquationComponent[] components = new EquationComponent[]
        {
                One, Equal, One, Equal, One
        };
        components.ValidateSyntax().Should().BeFalse();
    }

    [Test]
    public void Given_AddDivide_When_Validate_Then_ReturnFalse()
    {
        EquationComponent[] components = new EquationComponent[]
        {
                One, Add, Divide, One, Equal, One
        };
        components.ValidateSyntax().Should().BeFalse();
    }

    [Test]
    public void Given_MultiplyDivide_When_Validate_Then_ReturnFalse()
    {
        EquationComponent[] components = new EquationComponent[] {
                One, Multiply, Divide, One, Equal, One};
        components.ValidateSyntax().Should().BeFalse();
    }

    [Test]
    public void Given_ValidEquation_When_ToString_Return_HumanReadableString()
    {
        EquationComponent[] components = new EquationComponent[] {
                Four, Divide, Two, Multiply, Four, Equal, Eight};

        components.FormatAsString().Should().Be("4/2*4=8");
    }
}