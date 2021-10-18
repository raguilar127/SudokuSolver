using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Model
{
    public class Board
    {
        public string Name { get; set; }
        public string SolutionName { get; set; }
        public string PathToPuzzle { get; set; }
        public List<Cell> Cells { get; set; }

        public bool IsValid { get; set; }

        
    }
}
