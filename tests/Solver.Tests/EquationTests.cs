using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Solver;

namespace Solver.Tests
{
    public class EquationTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Given_NotAllowedEquation_When_Validate_Then_ReturnFalse()
        {
            List<long> numbers = new List<long>();
            List<Operator> operators = new List<Operator>();
            Equation equation = new Equation(numbers, operators);

            equation.Validate().Should().BeFalse();
        }

        [Test]
        public void Given_MultipleEqualOperators_When_Validate_Then_ReturnFalse()
        {
            List<long> numbers = new List<long>() { 1, 1, 1 };
            List<Operator> operators = new List<Operator>() { Operator.Equal, Operator.Equal };
            Equation equation = new Equation(numbers, operators);

            equation.Validate().Should().BeFalse();
        }

        [Test]
        public void Given_AllowedEquation_When_Validate_Then_ReturnTrue()
        {
            List<long> numbers = new List<long>() { 1, 1 };
            List<Operator> operators = new List<Operator>() { Operator.Equal };
            Equation equation = new Equation(numbers, operators);

            equation.Validate().Should().BeTrue();
        }

        [Test]
        public void Given_NotAllowedEquation_When_Validate_Then_Return_False()
        {
            List<long> numbers = new List<long>() { 1, 2 };
            List<Operator> operators = new List<Operator>() { Operator.Equal };
            Equation equation = new Equation(numbers, operators);

            equation.Validate().Should().BeFalse();
        }

        [Test]
        public void Given_OnePlusOneEqualsTwo_When_Validate_Then_Return_True()
        {
            List<long> numbers = new List<long>() { 1, 1, 2 };
            List<Operator> operators = new List<Operator>() { Operator.Add, Operator.Equal };
            Equation equation = new Equation(numbers, operators);

            equation.Validate().Should().BeTrue();
        }

        [Test]
        public void Given_OneMiunsOneEqualsZero_When_Validate_Then_Return_True()
        {
            List<long> numbers = new List<long>() { 1, 1, 0 };
            List<Operator> operators = new List<Operator>() { Operator.Substract, Operator.Equal };
            Equation equation = new Equation(numbers, operators);

            equation.Validate().Should().BeTrue();
        }

        [Test]
        public void Given_OnePlusOnePlusOneEqualsThree_When_Validate_Then_Return_True()
        {
            List<long> numbers = new List<long>() { 1, 1, 1, 3 };
            List<Operator> operators = new List<Operator>() { Operator.Add, Operator.Add, Operator.Equal };
            Equation equation = new Equation(numbers, operators);

            equation.Validate().Should().BeTrue();
        }

        [Test]
        public void Given_OnePlusOneEqualsOnePlusOne_When_Validate_Then_Return_True()
        {
            List<long> numbers = new List<long>() { 1, 1, 1, 1 };
            List<Operator> operators = new List<Operator>() { Operator.Add, Operator.Equal, Operator.Add };
            Equation equation = new Equation(numbers, operators);

            equation.Validate().Should().BeTrue();
        }

        [Test]
        public void Given_TwoTimesTwoEqualsFour_When_Validate_Then_Return_True()
        {
            List<long> numbers = new List<long>() { 2, 2, 4 };
            List<Operator> operators = new List<Operator>() { Operator.Multiply, Operator.Equal };
            Equation equation = new Equation(numbers, operators);

            equation.Validate().Should().BeTrue();
        }
        [Test]
        public void Given_TwoTimesTwoTimesTwoEqualsEight_When_Validate_Then_Return_True()
        {
            List<long> numbers = new List<long>() { 2, 2, 2, 8 };
            List<Operator> operators = new List<Operator>() { Operator.Multiply, Operator.Multiply, Operator.Equal };
            Equation equation = new Equation(numbers, operators);

            equation.Validate().Should().BeTrue();
        }

        [Test]
        public void Given_OnePlusTwoTimesTwoEqualsFive_When_Validate_Then_Return_True()
        {
            List<long> numbers = new List<long>() { 1, 2, 2, 5 };
            List<Operator> operators = new List<Operator>() { Operator.Add, Operator.Multiply, Operator.Equal };
            Equation equation = new Equation(numbers, operators);

            equation.Validate().Should().BeTrue();
        }

        [Test]
        public void Given_TwoTimesTwoPlusTwoEqualsSix_When_Validate_Then_Return_True()
        {
            List<long> numbers = new List<long>() { 2, 2, 2, 6 };
            List<Operator> operators = new List<Operator>() { Operator.Multiply, Operator.Add, Operator.Equal };
            Equation equation = new Equation(numbers, operators);

            equation.Validate().Should().BeTrue();
        }

        [Test]
        public void Given_FourTimesTwoDividedByEightEqualsOne_When_Validate_Then_Return_True()
        {
            List<long> numbers = new List<long>() { 4, 2, 8, 1 };
            List<Operator> operators = new List<Operator>() { Operator.Multiply, Operator.Divide, Operator.Equal };
            Equation equation = new Equation(numbers, operators);

            equation.Validate().Should().BeTrue();
        }

        [Test]
        public void Given_EqualEquations_When_Compare_Then_Return_True()
        {
            List<long> numbersOne = new List<long>() { 4, 2, 8, 1 };
            List<long> numbersTwo = new List<long>() { 4, 2, 8, 1 };
            List<Operator> operatorsOne = new List<Operator>() { Operator.Multiply, Operator.Divide, Operator.Equal };
            List<Operator> operatorsTwo = new List<Operator>() { Operator.Multiply, Operator.Divide, Operator.Equal };
            Equation equationOne = new Equation(numbersOne, operatorsOne);
            Equation equationTwo = new Equation(numbersTwo, operatorsTwo);

            equationOne.Equals(equationTwo).Should().BeTrue();
        }

        [Test]
        public void Given_NotEqualEquations_When_Compare_Then_Return_False()
        {
            List<long> numbersOne = new List<long>() { 4, 2, 4, 2 };
            List<long> numbersTwo = new List<long>() { 4, 2, 8, 1 };
            List<Operator> operatorsOne = new List<Operator>() { Operator.Multiply, Operator.Divide, Operator.Equal };
            List<Operator> operatorsTwo = new List<Operator>() { Operator.Multiply, Operator.Divide, Operator.Equal };
            Equation equationOne = new Equation(numbersOne, operatorsOne);
            Equation equationTwo = new Equation(numbersTwo, operatorsTwo);

            equationOne.Equals(equationTwo).Should().BeFalse();
        }
    }
}