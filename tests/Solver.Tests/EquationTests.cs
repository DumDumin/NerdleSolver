using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Solver;
using static Solver.EquationComponent;

namespace Solver.Tests
{
    public class EquationTests
    {
        [Test]
        public void Given_NotAllowedSyntax_When_Create_Then_ThrowException()
        {
            EquationComponent[] components = new EquationComponent[] {
                One, Multiply, Divide, One, Equal, One};
            Action act = () => new Equation(components);
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Given_MultiplyAsLastOperatorAndNotValid_When_Validate_Then_Return_False()
        {
            EquationComponent[] components = new EquationComponent[] {
                Five, Zero, Equal, Zero, Multiply, Zero};
            Equation.Validate(components).Should().BeFalse();
        }

        [Test]
        public void Given_MultiplyAsLastOperatorAndValid_When_Validate_Then_Return_True()
        {
            EquationComponent[] components = new EquationComponent[] {
                Five, Zero, Equal, One, Zero, Multiply, Five};
            Equation.Validate(components).Should().BeTrue();
        }

        [Test]
        public void Given_SubstractBehindEqual_When_Validate_Then_Return_False()
        {
            EquationComponent[] components = new EquationComponent[] {
                Seven, Equal, Substract, Seven};
            Equation.Validate(components).Should().BeFalse();
        }

        [Test]
        public void Given_EquationStartsWithSubstract_When_Validate_Then_ReturnTrue()
        {
            EquationComponent[] components = new EquationComponent[]{
                Substract, One, Add, One, Equal, Zero};
            Equation.Validate(components).Should().BeTrue();
        }

        [Test]
        public void Given_OneAddSubstractOneEqualsTwo_When_Validate_Then_Return_True()
        {
            EquationComponent[] components = new EquationComponent[] {
                One, Add, Substract, One, Equal, Zero};

            Equation.Validate(components).Should().BeTrue();
        }

        [Test]
        public void Given_SixPlusSevenEqualsThirteen_When_Validate_Then_Return_True()
        {
            EquationComponent[] components = new EquationComponent[] {
                Six, Add, Seven, Equal, One, Three};

            Equation.Validate(components).Should().BeTrue();
        }

        [Test]
        public void Given_AllowedEquation_When_Validate_Then_ReturnTrue()
        {
            EquationComponent[] components = new EquationComponent[] {
                One, Equal, One};

            Equation.Validate(components).Should().BeTrue();
        }

        [Test]
        public void Given_NotAllowedEquation_When_Validate_Then_Return_False()
        {
            EquationComponent[] components = new EquationComponent[] {
                One, Equal, Two};

            Equation.Validate(components).Should().BeFalse();
        }

        [Test]
        public void Given_OnePlusOneEqualsTwo_When_Validate_Then_Return_True()
        {
            EquationComponent[] components = new EquationComponent[] {
                One, Add, One, Equal, Two};

            Equation.Validate(components).Should().BeTrue();
        }

        [Test]
        public void Given_OneMinusOneEqualsZero_When_Validate_Then_Return_True()
        {
            EquationComponent[] components = new EquationComponent[] {
                One, Substract, One, Equal, Zero};

            Equation.Validate(components).Should().BeTrue();
        }

        [Test]
        public void Given_OnePlusOnePlusOneEqualsThree_When_Validate_Then_Return_True()
        {
            EquationComponent[] components = new EquationComponent[] {
                One, Add, One, Add, One, Equal, Three};

            Equation.Validate(components).Should().BeTrue();
        }

        [Test]
        public void Given_OnePlusOneEqualsOnePlusOne_When_Validate_Then_Return_True()
        {
            EquationComponent[] components = new EquationComponent[] {
                One, Add, One, Equal, One, Add, One};

            Equation.Validate(components).Should().BeTrue();
        }

        [Test]
        public void Given_TwoTimesTwoEqualsFour_When_Validate_Then_Return_True()
        {
            EquationComponent[] components = new EquationComponent[] {
                Two, Multiply, Two, Equal, Four};

            Equation.Validate(components).Should().BeTrue();
        }

        [Test]
        public void Given_TwoTimesTwoTimesTwoEqualsEight_When_Validate_Then_Return_True()
        {
            EquationComponent[] components = new EquationComponent[] {
                Two, Multiply, Two, Multiply, Two, Equal, Eight};

            Equation.Validate(components).Should().BeTrue();
        }

        [Test]
        public void Given_OnePlusTwoTimesTwoEqualsFive_When_Validate_Then_Return_True()
        {
            EquationComponent[] components = new EquationComponent[] {
                One, Add, Two, Multiply, Two, Equal, Five};

            Equation.Validate(components).Should().BeTrue();
        }

        [Test]
        public void Given_TwoTimesTwoPlusTwoEqualsSix_When_Validate_Then_Return_True()
        {
            EquationComponent[] components = new EquationComponent[] {
                Two, Multiply, Two, Add, Two, Equal, Six};

            Equation.Validate(components).Should().BeTrue();
        }

        [Test]
        public void Given_FourTimesTwoDividedByEightEqualsOne_When_Validate_Then_Return_True()
        {
            EquationComponent[] components = new EquationComponent[] {
                Four, Multiply, Two, Divide, Eight, Equal, One};

            Equation.Validate(components).Should().BeTrue();
        }

        [Test]
        public void Given_EqualEquations_When_Equals_Then_Return_True()
        {
            EquationComponent[] componentsOne = new EquationComponent[] {
                Four, Multiply, Two, Divide, Eight, Equal, One};
            EquationComponent[] componentsTwo = new EquationComponent[] {
                Four, Multiply, Two, Divide, Eight, Equal, One};

            Equation equationOne = new Equation(componentsOne);
            Equation equationTwo = new Equation(componentsTwo);

            equationOne.Equals(equationTwo).Should().BeTrue();
        }

        [Test]
        public void Given_NotEqualEquations_When_Equals_Then_Return_False()
        {
            EquationComponent[] componentsOne = new EquationComponent[] {
                Four, Multiply, Two, Divide, Four, Equal, Two};
            EquationComponent[] componentsTwo = new EquationComponent[] {
                Four, Multiply, Two, Divide, Eight, Equal, One};

            Equation equationOne = new Equation(componentsOne);
            Equation equationTwo = new Equation(componentsTwo);

            equationOne.Equals(equationTwo).Should().BeFalse();
        }

        [Test]
        public void Given_NotEqualEquations_When_Compare_Then_Return_CorrectFeedback()
        {
            EquationComponent[] componentsOne = new EquationComponent[] {
                Four, Multiply, Two, Divide, Four, Equal, Two};
            EquationComponent[] componentsTwo = new EquationComponent[] {
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
            EquationComponent[] componentsOne = new EquationComponent[] {
                Four, Multiply, Two, Divide, Four, Equal, Two};
            EquationComponent[] componentsTwo = new EquationComponent[] {
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
            EquationComponent[] componentsOne = new EquationComponent[] {
                Four, Divide, Two, Multiply, Four, Equal, Eight};
            EquationComponent[] componentsTwo = new EquationComponent[] {
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
    }
}