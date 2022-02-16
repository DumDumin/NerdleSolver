using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Solver;
using static Solver.EquationComponent;

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
            List<EquationComponent> components = new List<EquationComponent>();
            Equation equation = new Equation(components);

            equation.Validate().Should().BeFalse();
        }

        [Test]
        public void Given_EquationStartsWithMultiply_When_Validate_Then_ReturnFalse()
        {
            List<EquationComponent> components = new List<EquationComponent>()
            {
                Multiply, One, Equal, One
            };
            Equation equation = new Equation(components);

            equation.Validate().Should().BeFalse();
        }
        [Test]
        public void Given_EquationStartsWithDivide_When_Validate_Then_ReturnFalse()
        {
            List<EquationComponent> components = new List<EquationComponent>()
            {
                Divide, One, Equal, One
            };
            Equation equation = new Equation(components);

            equation.Validate().Should().BeFalse();
        }

        [Test]
        public void Given_EquationStartsWithEqual_When_Validate_Then_ReturnFalse()
        {
            List<EquationComponent> components = new List<EquationComponent>()
            {
                Equal, One
            };
            Equation equation = new Equation(components);

            equation.Validate().Should().BeFalse();
        }

        [Test]
        public void Given_EquationStartsWithAdd_When_Validate_Then_ReturnTrue()
        {
            List<EquationComponent> components = new List<EquationComponent>()
            {
                Add, One, Equal, One
            };
            Equation equation = new Equation(components);

            equation.Validate().Should().BeTrue();
        }

        [Test]
        public void Given_EquationStartsWithSubstract_When_Validate_Then_ReturnTrue()
        {
            List<EquationComponent> components = new List<EquationComponent>()
            {
                Substract, Zero, Equal, Zero
            };
            Equation equation = new Equation(components);

            equation.Validate().Should().BeTrue();
        }

        [Test]
        public void Given_OperatorInFrontOfEqual_When_Validate_Then_ReturnFalse()
        {
            List<EquationComponent> components = new List<EquationComponent>()
            {
                Zero, Add, Equal, Zero
            };
            Equation equation = new Equation(components);

            equation.Validate().Should().BeFalse();
        }

        [Test]
        public void Given_EquationEndsWithOperator_When_Validate_Then_ReturnFalse()
        {
            List<EquationComponent> components = new List<EquationComponent>()
            {
                One, Add, One, Equal, Two, Substract
            };
            Equation equation = new Equation(components);

            equation.Validate().Should().BeFalse();
        }

        [Test]
        public void Given_MultipleEqualOperators_When_Validate_Then_ReturnFalse()
        {
            List<EquationComponent> components = new List<EquationComponent>()
            {
                One, Equal, One, Equal, One
            };
            Equation equation = new Equation(components);

            equation.Validate().Should().BeFalse();
        }

        [Test]
        public void Given_SixPlusSevenEqualsThirteen_When_Validate_Then_Return_True()
        {
            List<EquationComponent> components = new List<EquationComponent>(){
                Six, Add, Seven, Equal, One, Three};
            Equation equation = new Equation(components);

            equation.Validate().Should().BeTrue();
        }

        [Test]
        public void Given_AllowedEquation_When_Validate_Then_ReturnTrue()
        {
            List<EquationComponent> components = new List<EquationComponent>(){
                One, Equal, One};
            Equation equation = new Equation(components);

            equation.Validate().Should().BeTrue();
        }

        [Test]
        public void Given_NotAllowedEquation_When_Validate_Then_Return_False()
        {
            List<EquationComponent> components = new List<EquationComponent>(){
                One, Equal, Two};
            Equation equation = new Equation(components);

            equation.Validate().Should().BeFalse();
        }

        [Test]
        public void Given_OnePlusOneEqualsTwo_When_Validate_Then_Return_True()
        {
            List<EquationComponent> components = new List<EquationComponent>(){
                One, Add, One, Equal, Two};
            Equation equation = new Equation(components);

            equation.Validate().Should().BeTrue();
        }

        [Test]
        public void Given_OneMiunsOneEqualsZero_When_Validate_Then_Return_True()
        {
            List<EquationComponent> components = new List<EquationComponent>(){
                One, Substract, One, Equal, Zero};
            Equation equation = new Equation(components);

            equation.Validate().Should().BeTrue();
        }

        [Test]
        public void Given_OnePlusOnePlusOneEqualsThree_When_Validate_Then_Return_True()
        {
            List<EquationComponent> components = new List<EquationComponent>(){
                One, Add, One, Add, One, Equal, Three};
            Equation equation = new Equation(components);

            equation.Validate().Should().BeTrue();
        }

        [Test]
        public void Given_OnePlusOneEqualsOnePlusOne_When_Validate_Then_Return_True()
        {
            List<EquationComponent> components = new List<EquationComponent>(){
                One, Add, One, Equal, One, Add, One};
            Equation equation = new Equation(components);

            equation.Validate().Should().BeTrue();
        }

        [Test]
        public void Given_TwoTimesTwoEqualsFour_When_Validate_Then_Return_True()
        {
            List<EquationComponent> components = new List<EquationComponent>(){
                Two, Multiply, Two, Equal, Four};
            Equation equation = new Equation(components);

            equation.Validate().Should().BeTrue();
        }

        [Test]
        public void Given_TwoTimesTwoTimesTwoEqualsEight_When_Validate_Then_Return_True()
        {
            List<EquationComponent> components = new List<EquationComponent>(){
                Two, Multiply, Two, Multiply, Two, Equal, Eight};
            Equation equation = new Equation(components);

            equation.Validate().Should().BeTrue();
        }

        [Test]
        public void Given_OnePlusTwoTimesTwoEqualsFive_When_Validate_Then_Return_True()
        {
            List<EquationComponent> components = new List<EquationComponent>(){
                One, Add, Two, Multiply, Two, Equal, Five};
            Equation equation = new Equation(components);

            equation.Validate().Should().BeTrue();
        }

        [Test]
        public void Given_TwoTimesTwoPlusTwoEqualsSix_When_Validate_Then_Return_True()
        {
            List<EquationComponent> components = new List<EquationComponent>(){
                Two, Multiply, Two, Add, Two, Equal, Six};
            Equation equation = new Equation(components);

            equation.Validate().Should().BeTrue();
        }

        [Test]
        public void Given_FourTimesTwoDividedByEightEqualsOne_When_Validate_Then_Return_True()
        {
            List<EquationComponent> components = new List<EquationComponent>(){
                Four, Multiply, Two, Divide, Eight, Equal, One};
            Equation equation = new Equation(components);

            equation.Validate().Should().BeTrue();
        }

        [Test]
        public void Given_EqualEquations_When_Equals_Then_Return_True()
        {
            List<EquationComponent> componentsOne = new List<EquationComponent>(){
                Four, Multiply, Two, Divide, Eight, Equal, One};
            List<EquationComponent> componentsTwo = new List<EquationComponent>(){
                Four, Multiply, Two, Divide, Eight, Equal, One};

            Equation equationOne = new Equation(componentsOne);
            Equation equationTwo = new Equation(componentsTwo);

            equationOne.Equals(equationTwo).Should().BeTrue();
        }

        [Test]
        public void Given_NotEqualEquations_When_Equals_Then_Return_False()
        {
            List<EquationComponent> componentsOne = new List<EquationComponent>(){
                Four, Multiply, Two, Divide, Four, Equal, Two};
            List<EquationComponent> componentsTwo = new List<EquationComponent>(){
                Four, Multiply, Two, Divide, Eight, Equal, One};

            Equation equationOne = new Equation(componentsOne);
            Equation equationTwo = new Equation(componentsTwo);

            equationOne.Equals(equationTwo).Should().BeFalse();
        }

        [Test]
        public void Given_NotEqualEquations_When_Compare_Then_Return_CorrectFeedback()
        {
            List<EquationComponent> componentsOne = new List<EquationComponent>(){
                Four, Multiply, Two, Divide, Four, Equal, Two};
            List<EquationComponent> componentsTwo = new List<EquationComponent>(){
                Four, Multiply, Two, Divide, Eight, Equal, One};

            Equation equationOne = new Equation(componentsOne);
            Equation equationTwo = new Equation(componentsTwo);

            var expected = new List<ComparisonStatus>() {
                    ComparisonStatus.Correct,
                    ComparisonStatus.Correct,
                    ComparisonStatus.Correct,
                    ComparisonStatus.Correct,
                    ComparisonStatus.False,
                    ComparisonStatus.Correct,
                    ComparisonStatus.False
                };
            var result = equationOne.Compare(equationTwo);
            
            result.Comparison.Should().ContainInOrder(expected);
        }

        [Test]
        public void Given_CorrectNumberAtWrongPlace_When_Compare_Then_Return_CorrectFeedback()
        {
            List<EquationComponent> componentsOne = new List<EquationComponent>(){
                Four, Multiply, Two, Divide, Four, Equal, Two};
            List<EquationComponent> componentsTwo = new List<EquationComponent>(){
                Four, Multiply, Two, Divide, Two, Equal, Four};

            Equation equationOne = new Equation(componentsOne);
            Equation equationTwo = new Equation(componentsTwo);

            var expected = new List<ComparisonStatus>() {
                    ComparisonStatus.Correct,
                    ComparisonStatus.Correct,
                    ComparisonStatus.Correct,
                    ComparisonStatus.Correct,
                    ComparisonStatus.WrongPlace,
                    ComparisonStatus.Correct,
                    ComparisonStatus.WrongPlace
                };
            
            var result = equationOne.Compare(equationTwo);
            
            result.Comparison.Should().ContainInOrder(expected);
        }

        [Test]
        public void Given_CorrectOperatorsAtWrongPlace_When_Compare_Then_Return_CorrectFeedback()
        {
            List<EquationComponent> componentsOne = new List<EquationComponent>(){
                Four, Divide, Two, Multiply, Four, Equal, Eight};
            List<EquationComponent> componentsTwo = new List<EquationComponent>(){
                Four, Multiply, Two, Divide, Four, Equal, Eight};

            Equation equationOne = new Equation(componentsOne);
            Equation equationTwo = new Equation(componentsTwo);

            var expected = new List<ComparisonStatus>() {
                    ComparisonStatus.Correct,
                    ComparisonStatus.WrongPlace,
                    ComparisonStatus.Correct,
                    ComparisonStatus.WrongPlace,
                    ComparisonStatus.Correct,
                    ComparisonStatus.Correct,
                    ComparisonStatus.Correct
                };

            var result = equationOne.Compare(equationTwo);
            
            result.Comparison.Should().ContainInOrder(expected);
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