using System;
using System.IO;
using Xunit;

namespace SudokuSolver.Tests
{
    public class SolverTests
    {
        string GenerateTempPuzzle(string inputFile)
        {
            Random rnd = new();
            string temp = $"./puzzle{rnd.Next(5, 100)}";
            File.Copy("./puzzle1.txt", temp + ".txt");

            return temp;
        }

        void CleanupTempFile(string file)
        {
            File.Delete(file + ".sln.txt");
            File.Delete(file + ".txt");
        }

        // a solver should print out solved sudokus
        [Fact]
        public void SolverHandlesBadPath()
        {
            PassingSolver solver = new();

            Assert.False(solver.Load("invalid path name"));
        }

        [Fact]
        public void SolverPrintsSolvedSudoku()
        {
            string temp = GenerateTempPuzzle("./puzzle1.txt");
            PassingSolver solver = new();

            _ = solver.Load(temp + ".txt");

            _ = solver.Solve();

            Assert.True(File.Exists(temp + ".sln.txt"));

            CleanupTempFile(temp);
        }

        [Fact]
        public void SolverDoesNotPrintUnsolvedSudokus()
        {
            string temp = GenerateTempPuzzle("./puzzle1.txt");
            FailingSolver solver = new();

            _ = solver.Load(temp + ".txt");

            _ = solver.Solve();

            Assert.False(File.Exists(temp + ".sln.txt"));

            CleanupTempFile(temp);
        }

        [Fact]
        public void SolverHandlesMalformedSudoku()
        {
            PassingSolver solver = new();
            // Puzzle 
            Assert.False(solver.Load("./puzzle2.txt"));
        }

        [Fact]
        public void SolverRenamesSolutionFile()
        {
            string temp = GenerateTempPuzzle("./puzzle1.txt");
            PassingSolver solver = new();

            _ = solver.Load(temp + ".txt");

            _ = solver.Solve();

            Assert.True(File.Exists(temp + ".sln.txt"));

            CleanupTempFile(temp);
        }

        [Fact]
        public void CSSolverFailsGracefully()
        {
            Model.ConstraintSatisfactionSolver solver = new();

            _ = solver.Load("./puzzle3.txt");

            Assert.False(solver.Solve());

            File.Delete("./puzzle3.sln.txt");
        }

        [Fact]
        public void BTSolverFailsGracefully()
        {
            Model.BacktrackingSolver solver = new();

            _ = solver.Load("./puzzle3.txt");

            Assert.False(solver.Solve());

            File.Delete("./puzzle3.sln.txt");
        }

    }
}
