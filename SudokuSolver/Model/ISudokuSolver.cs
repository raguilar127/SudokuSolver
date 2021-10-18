using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Model
{
    public interface ISudokuSolver
    {
        string MethodName { get; set; }
        Board Board { get; set; }
        Stopwatch Timer { get; set; }
        bool Load(string path);
        bool Solve();
        bool SolverMethod();
        bool Print();


    }
}
