using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Model
{
    public class ConstraintSatisfactionSolver : BaseSolver
    {
        public ConstraintSatisfactionSolver() : base("Constraint Satisfaction")
        {

        }

        public override bool SolverMethod()
        {
            // The idea for this function is directly from the wikipedia article
            // https://en.wikipedia.org/wiki/Sudoku_solving_algorithms#Constraint_programming
            // This piqued my interest, because it would allow us to skip the backtracking
            // process described in the first section of the article.

            // This function goes through the cells and reduces the possible values
            // based on any other cell neighbors

            foreach (Cell cell in Board.Cells)
            {
                var row = Board.Cells.Where(x => x.Row == cell.Row);
                var column = Board.Cells.Where(x => x.Column == cell.Column);
                var block = Board.Cells.Where(x => x.Block == cell.Block);

                // The selector for the next step just compares each possible value
                // to it's cell's neighbors
                static bool selector(IEnumerable<Cell> items, int val)
                {
                    return items.Where(y => y.Value.HasValue).Select(c => c.Value.Value).Contains(val);
                }

                // Use the selector to remove any values that would be invalid
                foreach (IEnumerable<Cell> set in new[] { row, block, column })
                {
                    _ = cell.PossibleValues.RemoveAll(x => selector(set, x));
                }

            }

            // Because we have no reduced any possible valid entries for each cell, we
            // should assume that some cells might be solved already
            var filled = Board.Cells.Where(x => x.PossibleValues.Count == 1);

            // if that is the case, update the cell value with the last possible value
            // and clear the list
            if (filled.Any())
            {
                foreach (Cell cell in filled)
                {
                    cell.Value = cell.PossibleValues.First();
                    cell.PossibleValues.Clear();
                }

                SolverMethod();
            }
            if (Board.Cells.Any(x => !x.Value.HasValue))
            {
                return false;
            }
            return true;
        }
    }
}
