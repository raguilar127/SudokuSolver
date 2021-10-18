using SudokuSolver.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SudokuSolver
{
    class Program
    {
        static void Main()
        {
            string[] puzzles = FindPuzzles();
            bool retry;
            do
            {
                Console.Clear();
                ShowMainMenu(puzzles);

                bool response = int.TryParse(Console.ReadLine(), out int choice);

                if (!response || choice == 0 || choice > puzzles.Length + 1)
                {
                    Console.WriteLine("Invalid choice. Retry? (Y / N)");
                    retry = "Y".Equals(Console.ReadLine(), StringComparison.OrdinalIgnoreCase);
                }
                else if (choice == puzzles.Length + 1)
                {
                    retry = false;
                }
                else
                {
                    CreateSolver(puzzles[choice - 1]);
                    Console.WriteLine("Solve another? (Y / N)");
                    retry = "Y".Equals(Console.ReadLine(), StringComparison.OrdinalIgnoreCase);
                }

            } while (retry);

            Exit();
        }

        private static void ShowMainMenu(string[] puzzles)
        {
            Console.WriteLine("Choose a puzzle to solve." + Environment.NewLine);
            foreach (var p in puzzles.Select((name, i) => new { i, name }))
            {
                Console.WriteLine($"{p.i + 1}:  {Path.GetFileName(p.name)}");
            }
            Console.WriteLine($"{puzzles.Length + 1}:  Exit");
        }

        private static string[] FindPuzzles()
        {
            // Filter out Solution files
            string[] paths = Directory.GetFiles(Environment.CurrentDirectory)
                                      .Where(f => Regex.IsMatch(Path.GetFileName(f), "puzzle\\d+\\.txt"))
                                      .ToArray();
            
            // If no files exist, print a helpful message and exit.
            if (!paths.Any())
            {
                Console.WriteLine("No Puzzles found.");
                Console.WriteLine();
                Console.WriteLine("Place puzzle files in the current directory and run the application again.");
                Exit();
            }

            // Create a list of board objects using the file names.
            return paths;
        }


        private static void CreateSolver(string path)
        {

            List<ISudokuSolver> solvers = new()
            {
                new ConstraintSatisfactionSolver(),
                new BacktrackingSolver(),
            };

            foreach (ISudokuSolver solver in solvers)
            {
                if (!solver.Load(path))
                {
                    Console.WriteLine("Could not load puzzle.");
                    break;
                }
                bool solved = solver.Solve();

                if (solved)
                {
                    Console.WriteLine($"Puzzle: {solver.Board.Name}");
                    Console.WriteLine($"Solution: {solver.Board.SolutionName}");
                    Console.WriteLine($"Solution Method: {solver.MethodName}");
                    Console.WriteLine($"Time Elapsed: {solver.Timer.ElapsedMilliseconds}ms" + Environment.NewLine);
                    return;
                }
            }


            Console.WriteLine($"Solution: No solution exists for this puzzle." + Environment.NewLine);
            return;
        }


        private static void Exit()
        {
            Console.WriteLine(Environment.NewLine + "Goodbye :(");
            Console.WriteLine("Press any key to exit...");
            _ = Console.ReadLine();
            Environment.Exit(0);
        }
    }
}
