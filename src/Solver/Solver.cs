using System.Linq;
using System.Collections.Generic;
using System;
using static Solver.EquationComponent;
using System.Threading.Tasks;

namespace Solver
{
    public static class Solver
    {
        public static Equation Guess(List<EquationComparison> info)
        {
            if (info.Count == 0)
            {
                List<EquationComponent> components = new List<EquationComponent>(){
                    Nine, Add, Eight, Substract, Five, Equal, One, Two};
                Equation guess = new Equation(components);

                return guess;
            }
            else
            {
                List<EquationComponent> components = new List<EquationComponent>(){
                Three, Add, Four, Add, Seven, Equal, One, Four};
                Equation guess = new Equation(components);

                return guess;
            }
        }

        public static long CountValidGuesses(int digitCount)
        {
            // Enum count = 15
            // Possible places = 8
            // => 15^8 => base 15 system
            var componentCount = Enum.GetNames(typeof(EquationComponent)).Length;
            long allPossibilities = (long)Math.Pow(componentCount, digitCount);

            long validEquationCount = 0;
            Parallel.For(0, allPossibilities, (i ) => {
                List<EquationComponent> components = GenerateComponents(i, digitCount);
                
                if(Equation.ValidateSyntax(components))
                {
                    Equation eq = new Equation(components);
                    if(eq.Validate())
                    {
                        validEquationCount++;
                    }
                }
            });

            return validEquationCount;
        }

        public static List<EquationComponent> GenerateComponents(long equationNumber, int digitCount)
        {
            EquationComponent[] components = new EquationComponent[digitCount];
            GetNextDigit(equationNumber, digitCount, components);
            return components.ToList();
        }

        private static void GetNextDigit(long equationNumber, int digitCount, EquationComponent[] components)
        {
            // Algorithm:
            // Enum = i % 15 
            // i = i / 15
            // Recursion

            long result = Math.DivRem(equationNumber, 15, out long rest);
            components[--digitCount] = (EquationComponent)(rest);
            if(digitCount > 0)
            {
                GetNextDigit(result, digitCount, components);
            }
        }
    }
}