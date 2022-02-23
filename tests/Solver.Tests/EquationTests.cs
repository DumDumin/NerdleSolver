using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Solver;
using static Solver.EquationComponent;

namespace Solver.Tests;

public class EquationTests
{
    [Test]
    public void Given_DivideResultsInFloatingPoint_When_Validate_Then_ReturnFalse()
    {
        EquationComponent[] components = new EquationComponent[] {
                Add, Four, Divide, Nine, Divide, Nine, Equal, Zero};
        components.Validate().Should().BeFalse();
    }

    [Test]
    public void Given_NotEqualEquations_When_Compare_Then_Return_CorrectFeedback()
    {
        EquationComponent[] componentsOne = new EquationComponent[] {
                Four, Multiply, Two, Divide, Four, Equal, Two};
        EquationComponent[] componentsTwo = new EquationComponent[] {
                Four, Multiply, Two, Divide, Eight, Equal, One};

        var expected = new List<ComparisonStatus>() {
                    ComparisonStatus.Correct,
                    ComparisonStatus.Correct,
                    ComparisonStatus.Correct,
                    ComparisonStatus.Correct,
                    ComparisonStatus.False,
                    ComparisonStatus.Correct,
                    ComparisonStatus.False
                };
        var result = componentsOne.Compare(componentsTwo);

        result.Comparison.Should().ContainInOrder(expected);
    }

    [Test]
    public void Given_CorrectNumberAtWrongPlace_When_Compare_Then_Return_CorrectFeedback()
    {
        EquationComponent[] componentsOne = new EquationComponent[] {
                Four, Multiply, Two, Divide, Four, Equal, Two};
        EquationComponent[] componentsTwo = new EquationComponent[] {
                Four, Multiply, Two, Divide, Two, Equal, Four};

        var expected = new List<ComparisonStatus>() {
                    ComparisonStatus.Correct,
                    ComparisonStatus.Correct,
                    ComparisonStatus.Correct,
                    ComparisonStatus.Correct,
                    ComparisonStatus.WrongPlace,
                    ComparisonStatus.Correct,
                    ComparisonStatus.WrongPlace
                };

        var result = componentsOne.Compare(componentsTwo);

        result.Comparison.Should().ContainInOrder(expected);
    }

    [Test]
    public void Given_CorrectOperatorsAtWrongPlace_When_Compare_Then_Return_CorrectFeedback()
    {
        EquationComponent[] componentsOne = new EquationComponent[] {
                Four, Divide, Two, Multiply, Four, Equal, Eight};
        EquationComponent[] componentsTwo = new EquationComponent[] {
                Four, Multiply, Two, Divide, Four, Equal, Two};

        var expected = new List<ComparisonStatus>() {
                    ComparisonStatus.Correct,
                    ComparisonStatus.WrongPlace,
                    ComparisonStatus.Correct,
                    ComparisonStatus.WrongPlace,
                    ComparisonStatus.Correct,
                    ComparisonStatus.Correct,
                    ComparisonStatus.False
                };

        var result = componentsOne.Compare(componentsTwo);

        result.Comparison.Should().ContainInOrder(expected);
    }

    [Test]
    public void Given_CompareString_When_CreateFromString_Then_Created()
    {
        EquationComponent[] eq = new EquationComponent[] { Two, Equal, Two };
        EquationComparison comp = EquationComparison.FromString("012", eq);
        comp.Should().BeEquivalentTo(new EquationComparison(eq, new List<ComparisonStatus>() { ComparisonStatus.False, ComparisonStatus.Correct, ComparisonStatus.WrongPlace }));
    }

    [Test]
    public void Given_InvalidCompareString_When_CreateFromString_Then_Throw()
    {
        EquationComponent[] eq = new EquationComponent[] { Two, Equal, Two };
        Action act = () => EquationComparison.FromString("3", eq);
        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Given_ValidString_When_CreateEquation_Then_Created()
    {
        string eq = "1+1+2=04";
        EquationComponent[] equation = Equation.FromString(eq);
        equation.Should().BeEquivalentTo(new EquationComponent[] {
                One, Add, One, Add, Two, Equal, Zero, Four});
    }

    [Test]
    public void Given_MultiplyThenAdd_When_Substitute_Then_Return_NewEquation()
    {
        EquationComponent[] components = new EquationComponent[] {
                One, Multiply, One, Add, One};

        EquationComponent[] expected = new EquationComponent[] {
                One, Add, One};

        components.Substitute(out bool valid).Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Given_AddThenMultiply_When_Substitute_Then_Return_NewEquation()
    {
        EquationComponent[] components = new EquationComponent[] {
                One, Add, One, Multiply, One};

        EquationComponent[] expected = new EquationComponent[] {
                One, Add, One};

        components.Substitute(out bool valid).Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Given_MultiplyThenMultiplyThenAdd_When_Substitute_Then_Return_NewEquation()
    {
        EquationComponent[] components = new EquationComponent[] {
                One, Multiply, One, Multiply, One, Add, One};

        EquationComponent[] expected = new EquationComponent[] {
                One, Add, One};

        components.Substitute(out bool valid).Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Given_DivideThenMultiplyThenAdd_When_Substitute_Then_Return_NewEquation()
    {
        EquationComponent[] components = new EquationComponent[] {
                One, Divide, One, Multiply, One, Add, One};

        EquationComponent[] expected = new EquationComponent[] {
                One, Add, One};

        components.Substitute(out bool valid).Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Given_InvalidEquation_When_Validate_Then_ReturnFalse()
    {
        EquationComponent[] components = new EquationComponent[] {
                Add, Three, Multiply, Zero, Three, Four, Equal, Zero };

        components.Validate().Should().BeFalse();
    }
}