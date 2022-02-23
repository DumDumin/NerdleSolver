using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using static Solver.EquationComponent;
using System;

namespace Solver.Tests;

public class EquationFilterTests
{

    [Test]
    public void Given_EquationWithKnownFalseOperator_When_Filter_Then_RemoveIt()
    {
        // correct equation is One Add Two Equals Three (1+2=3)
        EquationComponent[] equation = new EquationComponent[] {
                One, Add, Two, Equal, Three};
        EquationComponent[] compare = new EquationComponent[] {
                One, Add, Four, Equal, Five};

        EquationComparison comparison = Equation.Compare(equation, compare);

        // components to check
        EquationComponent[] componentOne = new EquationComponent[] {
                One, Add, One, Equal, Two};
        EquationComponent[] componentTwo = new EquationComponent[] {
                One, Substract, One, Equal, Zero};

        new List<EquationComponent[]>() { componentOne, componentTwo }
            .Filter(comparison).Should().BeEquivalentTo(new List<EquationComponent[]>() { componentOne });
    }

    [Test]
    public void Given_EquationWithKnownFalseComponent_When_Filter_Then_RemoveIt()
    {
        EquationComponent[] equation = new EquationComponent[] {
                One, Add, Two, Add, Three, Equal, Six};
        EquationComponent[] compare = new EquationComponent[] {
                One, Add, One, Add, Four, Equal, Six};

        EquationComparison comparison = Equation.Compare(equation, compare);

        EquationComponent[] componentOne = new EquationComponent[] {
                One, Add, Four, Add, One, Equal, Six};
        EquationComponent[] componentTwo = new EquationComponent[] {
                One, Add, One, Add, Five, Equal, Six};

        new List<EquationComponent[]>() {
                    componentOne, componentTwo }.Filter(
            comparison
        ).Should().BeEquivalentTo(new List<EquationComponent[]>());
    }

    [Test]
    public void Given_EquationWithTwoAndThree_When_Filter_Then_RemoveIt()
    {
        // correct equation is One Add Two Equals Three (1+2=3)
        EquationComponent[] equation = new EquationComponent[] {
                One, Add, Two, Add, Three, Equal, Six};
        EquationComponent[] compare = new EquationComponent[] {
                Two, Add, Three, Add, One, Equal, Six};

        EquationComparison comparison = Equation.Compare(equation, compare);

        EquationComponent[] componentOne = new EquationComponent[] {
                One, Add, One, Add, Four, Equal, Six};

        new List<EquationComponent[]>() {
                    componentOne }.Filter(
            comparison
        ).Should().BeEquivalentTo(new List<EquationComponent[]>());
    }

    [Test]
    public void Given_EquationWithCorrectComponents_When_Filter_Then_DoNotRemoveThem()
    {
        EquationComponent[] equation = new EquationComponent[] {
                One, Add, Two, Add, Three, Equal, Six};
        EquationComponent[] compare = new EquationComponent[] {
                Two, Add, Three, Add, One, Equal, Six};

        EquationComparison comparison = Equation.Compare(equation, compare);

        EquationComponent[] componentOne = new EquationComponent[] {
                One, Add, Two, Add, Three, Equal, Six};
        EquationComponent[] componentTwo = new EquationComponent[] {
                Three, Add, One, Add, Two, Equal, Six};

        new List<EquationComponent[]>() {
                    componentOne, componentTwo }.Filter(
            comparison
        ).Should().BeEquivalentTo(new List<EquationComponent[]>() { componentOne, componentTwo });
    }

    [Test]
    public void Given_EquationWithoutEnoughOnes_When_Filter_Then_RemoveIt()
    {
        // One is correct and One has WrongPlace
        EquationComponent[] equation = new EquationComponent[] {
                One, Add, One, Add, Three, Multiply, Two, Equal, Zero, Eight};
        EquationComponent[] compare = new EquationComponent[] {
                One, Add, Four, Multiply, Five, Add, One, Equal, Two, Two};

        EquationComparison comparison = Equation.Compare(equation, compare);

        EquationComponent[] componentOne = new EquationComponent[] {
                One, Add, Two, Add, Eight, Multiply, Eight, Equal, Six, Seven};

        new List<EquationComponent[]>() {
                    componentOne }.Filter(
            comparison
        ).Should().BeEquivalentTo(new List<EquationComponent[]>() { });
    }

    [Test]
    public void Given_EquationWithCorrectZeroAfterFalseZero_When_Filter_Then_DoNotRemove()
    {
        // One is correct and One has WrongPlace
        EquationComponent[] equation = new EquationComponent[] {
                Zero, Zero, Add, Zero, Equal, Five, Substract, Five};
        EquationComponent[] compare = new EquationComponent[] {
                Zero, Zero, Zero, Zero, Zero, Zero, Equal, Zero};

        EquationComparison comparison = Equation.Compare(equation, compare);

        EquationComponent[] componentOne = new EquationComponent[] {
                Zero, Zero, Add, Zero, Equal, Five, Substract, Five};

        new List<EquationComponent[]>() {
                    componentOne }.Filter(
            comparison
        ).Should().BeEquivalentTo(new List<EquationComponent[]>() { componentOne });
    }

    [Test]
    public void Given_ContainsNoZeros_When_Filter_Then_RemoveComponentOne()
    {
        EquationComponent[] componentOne = new EquationComponent[] {
                Zero, Zero, Add, Zero, Equal, Five, Substract, Five};
        EquationComponent[] componentTwo = new EquationComponent[] {
                One, Add, Two, Add, Eight, Equal, One, One};

        new List<EquationComponent[]>() { componentOne, componentTwo }.Filter(
            new List<Func<EquationComponent[], bool>>() {
                    Rules.ContainsNoZeros
            }
        ).Should().BeEquivalentTo(new List<EquationComponent[]>() { componentTwo });
    }
}