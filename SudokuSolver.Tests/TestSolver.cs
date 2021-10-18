using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Tests
{
    public class FailingSolver : Model.BaseSolver
    {
        public FailingSolver() : base("Test Failing Solver")
        {

        }
        public override bool SolverMethod()
        {
            return false;
        }
    }

    public class PassingSolver : Model.BaseSolver
    {
        public PassingSolver() : base("Test Passing Solver")
        {

        }
        public override bool SolverMethod()
        {
            return true;
        }
    }
}
