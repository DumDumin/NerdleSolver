using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Solver;
using static Solver.EquationComponent;
using static Solver.ComparisonStatus;

namespace Solver.Tests
{
    public class EquationFilterTests
    {

        [Test]
        public void Given_TwoEquationsAndComparison_When_Filter_Then_ReturnOneEquation()
        {
            // correct equation is One Add Two Equals Three (1+2=3)
            EquationComponent[] equation = new EquationComponent[] {
                One, Add, Two, Equal, Three};
            EquationComponent[] compare = new EquationComponent[] {
                One, Add, Four, Equal, Five};

            EquationComparison comparison = Equation.Compare(new Equation(equation), new Equation(compare));

            // components to check
            EquationComponent[] componentOne = new EquationComponent[] {
                One, Add, One, Equal, Two};
            EquationComponent[] componentTwo = new EquationComponent[] {
                One, Substract, One, Equal, Zero};

            Equation.Filter(
                new List<EquationComponent[]>() {
                    componentOne, componentTwo },
                comparison
            ).Should().BeEquivalentTo(new List<EquationComponent[]>(){componentOne});
        }

        [Test]
        public void Given_TwoEquationsAndComparison_When_Filter_Then_ReturnOneEquationA()
        {
            // correct equation is One Add Two Equals Three (1+2=3)
            EquationComponent[] equation = new EquationComponent[] {
                One, Add, Two, Add, Three, Equal, Six};
            EquationComponent[] compare = new EquationComponent[] {
                One, Add, One, Add, Four, Equal, Six};

            EquationComparison comparison = Equation.Compare(new Equation(equation), new Equation(compare));

            // components to check
            EquationComponent[] componentOne = new EquationComponent[] {
                One, Add, Four, Add, One, Equal, Six};
            EquationComponent[] componentTwo = new EquationComponent[] {
                One, Add, One, Add, Five, Equal, Six};

            Equation.Filter(
                new List<EquationComponent[]>() {
                    componentOne, componentTwo },
                comparison
            ).Should().BeEquivalentTo(new List<EquationComponent[]>(){componentOne});
        }

        [Test]
        public void Given_TwoEquationsAndComparison_When_Filter_Then_ReturnOneEquationAA()
        {
            // correct equation is One Add Two Equals Three (1+2=3)
            EquationComponent[] equation = new EquationComponent[] {
                One, Add, Two, Add, Three, Equal, Six};
            EquationComponent[] compare = new EquationComponent[] {
                Two, Add, Three, Add, One, Equal, Six};

            EquationComparison comparison = Equation.Compare(new Equation(equation), new Equation(compare));

            // components to check
            EquationComponent[] componentOne = new EquationComponent[] {
                One, Add, Two, Add, Three, Equal, Six};
            EquationComponent[] componentTwo = new EquationComponent[] {
                Three, Add, One, Add, Two, Equal, Six};
            EquationComponent[] componentThree = new EquationComponent[] {
                One, Add, One, Add, Four, Equal, Six};

            Equation.Filter(
                new List<EquationComponent[]>() {
                    componentOne, componentTwo, componentThree },
                comparison
            ).Should().BeEquivalentTo(new List<EquationComponent[]>(){componentOne, componentTwo});
        }
    }
}