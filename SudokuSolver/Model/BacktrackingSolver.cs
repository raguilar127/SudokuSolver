using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SudokuSolver.Model
{
    public class BacktrackingSolver : BaseSolver
    {
        public BacktrackingSolver() : base("Backtracking")
        {

        }

        public override bool SolverMethod()
        {
            // Loop over all cells that have no value set
            foreach (Cell cell in Board.Cells.Where(x => !x.Value.HasValue))
            {
                // Get each set of neighboring cells that have values set
                var row = Board.Cells.Where(x => x.Row == cell.Row && x.Value.HasValue);
                var column = Board.Cells.Where(x => x.Column == cell.Column && x.Value.HasValue);
                var block = Board.Cells.Where(x => x.Block == cell.Block && x.Value.HasValue);

                // Possible values are 1 through 9 using the backtracking method.
                // We could probably speed it up slightly by pruning useless values like we
                // did for constraint checking.
                foreach (int val in cell.PossibleValues)
                {
                    // check if any neighbors already have this value set
                    if (new[] { row, block, column }.Any(x => x.Any(y => y.Value == val)))
                    {
                        continue;
                    }
                    // the value is valid enough to set on the cell
                    cell.Value = val;

                    // recurse for the remainder of the cells
                    if (SolverMethod())
                    {
                        // unroll if we're done solving
                        return true;
                    }

                    // if the solver fails down the chain we unroll to here and 
                    // reset the failing cell's value to null
                    cell.Value = null;
                }
                // if the cell could not be set, then we'll return false and repeat on the next 
                // possible value for that cell
                if (!cell.Value.HasValue)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
