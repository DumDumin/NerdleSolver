using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Solver;
using static Solver.EquationComponent;

namespace Solver.Tests
{
    public class EquationSyntaxTests
    {

        [Test]
        public void Given_NotAllowedEquation_When_Validate_Then_ReturnFalse()
        {
            List<EquationComponent> components = new List<EquationComponent>();
            Equation.ValidateSyntax(components).Should().BeFalse();
        }

        [Test]
        public void Given_DivideByZero_When_Validate_Then_ReturnFalse()
        {
            List<EquationComponent> components = new List<EquationComponent>(){
                One, Divide, Zero, Equal, One};
            Equation.ValidateSyntax(components).Should().BeFalse();
        }
        [Test]
        public void Given_DivideByZeroTwoTimes_When_Validate_Then_ReturnFalse()
        {
            List<EquationComponent> components = new List<EquationComponent>(){
                One, Divide, One, Divide, Zero, Equal, One};
            Equation.ValidateSyntax(components).Should().BeFalse();
        }

        [Test]
        public void Given_EquationStartsWithMultiply_When_Validate_Then_ReturnFalse()
        {
            List<EquationComponent> components = new List<EquationComponent>(){
                Multiply, One, Equal, One};
            Equation.ValidateSyntax(components).Should().BeFalse();
        }
        [Test]
        public void Given_EquationStartsWithDivide_When_Validate_Then_ReturnFalse()
        {
            List<EquationComponent> components = new List<EquationComponent>()
            {
                Divide, One, Equal, One
            };
            Equation.ValidateSyntax(components).Should().BeFalse();
        }

        [Test]
        public void Given_EquationStartsWithEqual_When_Validate_Then_ReturnFalse()
        {
            List<EquationComponent> components = new List<EquationComponent>()
            {
                Equal, One
            };
            Equation.ValidateSyntax(components).Should().BeFalse();
        }

        [Test]
        public void Given_EquationStartsWithAdd_When_Validate_Then_ReturnTrue()
        {
            List<EquationComponent> components = new List<EquationComponent>()
            {
                Add, One, Equal, One
            };
            Equation.ValidateSyntax(components).Should().BeTrue();
        }

        [Test]
        public void Given_EquationStartsWithSubstract_When_Validate_Then_ReturnTrue()
        {
            List<EquationComponent> components = new List<EquationComponent>()
            {
                Substract, Zero, Equal, Zero
            };
            Equation.ValidateSyntax(components).Should().BeTrue();
        }

        [Test]
        public void Given_OperatorInFrontOfEqual_When_Validate_Then_ReturnFalse()
        {
            List<EquationComponent> components = new List<EquationComponent>()
            {
                Zero, Add, Equal, Zero
            };
            Equation.ValidateSyntax(components).Should().BeFalse();
        }

        [Test]
        public void Given_OperatorBehindOfEqual_When_Validate_Then_ReturnFalse()
        {
            List<EquationComponent> components = new List<EquationComponent>()
            {
                Zero, Equal, Add, Zero
            };
            Equation.ValidateSyntax(components).Should().BeFalse();
        }

        [Test]
        public void Given_EquationEndsWithOperator_When_Validate_Then_ReturnFalse()
        {
            List<EquationComponent> components = new List<EquationComponent>()
            {
                One, Add, One, Equal, Two, Substract
            };
            Equation.ValidateSyntax(components).Should().BeFalse();
        }

        [Test]
        public void Given_MultipleEqualOperators_When_Validate_Then_ReturnFalse()
        {
            List<EquationComponent> components = new List<EquationComponent>()
            {
                One, Equal, One, Equal, One
            };
            Equation.ValidateSyntax(components).Should().BeFalse();
        }

        [Test]
        public void Given_AddDivide_When_Validate_Then_ReturnFalse()
        {
            List<EquationComponent> components = new List<EquationComponent>()
            {
                One, Add, Divide, One, Equal, One
            };
            Equation.ValidateSyntax(components).Should().BeFalse();
        }

        [Test]
        public void Given_MultiplyDivide_When_Validate_Then_ReturnFalse()
        {
            List<EquationComponent> components = new List<EquationComponent>(){
                One, Multiply, Divide, One, Equal, One};
            Equation.ValidateSyntax(components).Should().BeFalse();
        }

        [Test]
        public void Given_ValidEquation_When_ToString_Return_HumanReadableString()
        {
            List<EquationComponent> components = new List<EquationComponent>(){
                Four, Divide, Two, Multiply, Four, Equal, Eight};

            Equation equation = new Equation(components);

            equation.ToString().Should().Be("4/2*4=8");
        }
    }
}