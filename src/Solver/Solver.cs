using System.Linq;
using System.Collections.Generic;
using System;
using static Solver.EquationComponent;

namespace Solver
{
    public static class Solver
    {
        public static Equation Solve(Dictionary<Equation, EquationComparison> info)
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
                throw new NotImplementedException();
            }
        }
    }
}