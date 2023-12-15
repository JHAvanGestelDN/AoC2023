using Day00;
using System.Linq;
using System.Collections.Generic;

namespace Day11 {
    internal class Program : Base {
        public static void Main(string[] args) {
            new Program();
        }

        override protected long SolveOne() {
            var input = ReadFileToArray(PathOneSample);
            List<int> emptyColumns = new List<int>();
            List<int> emptyRows = new List<int>();
            for (var y = 0; y < input.Length; y++) {
                if (input[y].Length == input[y].Count(x => x == '.')) {
                    emptyRows.Add(y);
                }
            }
            for (int x = 0; x < input[0].Length; x++) {
                if (input.All(y => y[x] == '.')) {
                    emptyColumns.Add(x);
                }
            }

            List<Coordinate> galaxies = new List<Coordinate>();
            for (int y = 0; y < input.Length; y++) {
                for (int x = 0; x < input[0].Length; x++) {
                    if (input[y][x] == '#') {
                        galaxies.Add(new Coordinate(x, y));
                    }
                }
            }
            //update coordinates depending on empty rows and columns
            //if we add an extra row the y index of all galaxies below that row should be increased by 1
            //if we add an extra column the x index of all galaxies to the right of that column should be increased by 1

            foreach (var galaxy in galaxies) {
                foreach (var emptyRow in emptyRows) {
                    if (galaxy.Y > emptyRow) {
                        galaxy.Y++;
                    }
                }
                foreach (var emptyColumn in emptyColumns) {
                    if (galaxy.X > emptyColumn) {
                        galaxy.X++;
                    }
                }
            }
            int maxRowLenght = input.Length+emptyRows.Count;
            int maxColumnLenght = input[0].Length+emptyColumns.Count;
            HashSet<Coordinate> coordinates = new HashSet<Coordinate>(galaxies);
            for (int y = 0; y < maxColumnLenght; y++) {
                for (int x = 0; x < maxRowLenght; x++) {
                    //check if coordinate is in the HashSet
                    if (coordinates.Contains(new Coordinate(x, y))) {
                        Console.Write("#");
                    }
                    else {
                        Console.Write(".");
                    }
                }
                Console.WriteLine();
            }


            //copy input and extend array
            // // char[,] copy = new char[input.Length + emptyRows.Count, input[0].Length + emptyColumns.Count];
            // // //copy data and add empty rows (we add the columns later)
            // //
            // // int addedRows = 0;
            // // for (int y = 0; y < input.Length; y++) {
            // //     int addedColumns = 0;
            // //
            // //     if (emptyRows.Contains(y)) {
            // //         //insert empty row
            // //         for (int x = 0; x < copy.GetLength(1); x++) {
            // //             copy[y + addedRows, x] = '.';
            // //             copy[y + addedRows + 1, x] = '.';
            // //         }
            // //         addedRows++;
            // //         continue;
            // //     }
            // //     for (int x = 0; x < input[0].Length; x++) {
            // //         if (emptyColumns.Contains(x)) {
            // //             //insert empty column
            // //             copy[y, x + addedColumns] = '.';
            // //             copy[y, x + addedColumns + 1] = '.';
            // //             addedColumns++;
            // //
            // //         }
            // //         else
            // //             copy[y + addedRows, x + addedColumns] = input[y][x];
            // //     }
            // // }
            //
            //
            // //print copy
            // for (int y = 0; y < copy.GetLength(0); y++) {
            //     for (int x = 0; x < copy.GetLength(1); x++) {
            //         Console.Write(copy[y, x]);
            //     }
            //     Console.WriteLine();
            // }


            return 0;
        }

        override protected long SolveTwo() {
            throw new System.NotImplementedException();
        }
    }
}
