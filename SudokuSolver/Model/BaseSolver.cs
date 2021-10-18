using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace SudokuSolver.Model
{
    /// <summary>
    /// Author: Robert Aguilar
    /// Date: 2021-10-17
    /// </summary>
    public abstract class BaseSolver : ISudokuSolver
    {
        public string MethodName { get; set; }

        protected BaseSolver(string methodName)
        {
            MethodName = methodName;
        }

        public Board Board { get; set; }
        public Stopwatch Timer { get; set; }

        public bool Load(string path)
        {
            // set up a new board
            Board = new Board
            {
                PathToPuzzle = path,
                Name = Path.GetFileName(path),
                SolutionName = Path.GetFileName(Path.ChangeExtension(path, ".sln.txt")),
                Cells = new List<Cell>()
            };

            // Read the entire file into a string with new lines stripped. 
            // Indexes on the board are tracked by Cell objects.
            string text = "";
            try
            {
                text = File.ReadAllText(Board.PathToPuzzle);
                text = text.Replace(Environment.NewLine, "");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Board = null;
                return false;
            }

            // Check for any malformed sudokus
            if(!Regex.IsMatch(text, @"[\dX]{81}"))
            {
                Board = null;
                return false;
            }

            // A puzzle is considered invalid if there are not 81 
            // total empty spaces ('X') and filled spaces (numbers)
            if (text.Length != 81)
            {
                Board = null;
                return false;
            }

            for (int i = 0; i < 81; i++)
            {
                // Parse each read character into integers and create Cell objects
                bool valid = int.TryParse(text[i].ToString(), out int val);

                // For any Cells that did not get a value, fill the list of
                // potential values with valid numbers
                Board.Cells.Add(new Cell(i)
                {
                    Value = valid ? val : null,
                    PossibleValues = valid ? new List<int>() : Enumerable.Range(1, 9).ToList(),
                });
            }

            return true;
        }

        public bool Solve()
        {
            if (Board == null)
            {
                return false;
            }

            Timer = Stopwatch.StartNew();

            Board.IsValid = SolverMethod();

            Timer.Stop();

            if (Board.IsValid)
            {
                Print();
            }

            return Board.IsValid;
        }

        public abstract bool SolverMethod();

        public bool Print()
        {

            string[] lines = new string[9];

            // Group all cells by row and concatenate the values into a string
            var groups = from row in Board.Cells.GroupBy(x => x.Row)
                         let values = row.OrderBy(x => x.Column).Select(x => (x.Value.HasValue) ? x.Value.Value.ToString() : "X")
                         let line = String.Join("", values)
                         select (row, line);

            foreach (var (row, line) in groups)
            {
                lines[row.Key] = line;
            }

            string path = Directory.GetParent(Board.PathToPuzzle).FullName;
            try
            {
                File.WriteAllLines(Path.Combine(path, Board.SolutionName), lines);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }

            return true;
        }

    }
}
