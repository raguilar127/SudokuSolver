using System.Collections.Generic;

namespace SudokuSolver.Model
{
    public class Cell
    {
        public int Row
        {
            get
            {
                // Rows can be thought of as sets of 9
                // so we can just divide the index by 9
                int r = Index / 9;
                return r;
            }
        }

        public int Column
        {
            get
            {
                // Just like Rows, the columns can be 
                // derived from which 'place' in the row
                // that the index is. So the modulo op gives us 
                // a simple solution.
                int c = Index % 9;
                return c;
            }
        }

        public int Block
        {
            get
            {
                // blk_row is the top, middle, and bottom row on 
                // a sudoku board. We can just divide the row by three
                // and round it off, so it's 0, 1, or 2
                int blk_row = Row / 3;
                // This algorithm I actually thought of by using Excel to plot
                // the indexes from A1:I9
                // I then plotted MOD(ROUNDDOWN(A1/3,0),(K11+1)*9) below the indexes
                // K11 is where the grid of row numbers is
                // This basically fills the 9 x 9 grid counting in triplicate like
                // 1 1 1 2 2 2 3 3 3. I derived this from the index, just so that
                // the cell could be standalone
                int blk = Index / 3 % ((Row + 1) * 9);
                // now we can get the block by checking the third and adding 3 based on 
                // the block row
                blk = (blk % 3) + (blk_row * 3);
                // There was probably an easier way to do this, but I needed to see the
                // problem visually, so I played around in Excel prior to starting to code
                return blk;
            }

        }

        public int Index { get; set; }

        public List<int> PossibleValues { get; set; }

        public int? Value { get; set; }

        public Cell(int index)
        {
            Index = index;
        }

    }
}
