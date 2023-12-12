using Day00;
using Day00.Maps;

namespace Day03 {
    internal class Program : Base {
        public static void Main(string[] args) {
            new Program();
        }

        override protected long SolveOne() {
            var input = ReadFileToArray(PathOne);
            var numbers = FindNumbers(input);
            int partNumbers = 0;

            //check around all digits af a number (starting with the first coordinate)
            //a symbol is any character that is not a digit or a '.'
            //if we find a symbol, we can continue to the next number
            //if we dont we need to move to the next digit of the number
            foreach (var (coordinate, number) in numbers) {
                bool hasSymbol = false;
                for (var i = 0; i < number.Length; i++) {
                    //loop through all digits of the number
                    //check if the current coordinate has a symbol around it
                    //up
                    if (coordinate.Y - 1 >= 0 && IsSymbol(input[coordinate.Y - 1][coordinate.X])) {
                        hasSymbol = true;
                        break;
                    }
                    //down
                    if (coordinate.Y + 1 < input.Length && IsSymbol(input[coordinate.Y + 1][coordinate.X])) {
                        hasSymbol = true;
                        break;
                    }
                    //left
                    if (coordinate.X - 1 >= 0 && IsSymbol(input[coordinate.Y][coordinate.X - 1])) {
                        hasSymbol = true;
                        break;
                    }
                    //right
                    if (coordinate.X + 1 < input[coordinate.Y].Length && IsSymbol(input[coordinate.Y][coordinate.X + 1])) {
                        hasSymbol = true;
                        break;
                    }
                    //up left
                    if (coordinate.Y - 1 >= 0 && coordinate.X - 1 >= 0 && IsSymbol(input[coordinate.Y - 1][coordinate.X - 1])) {
                        hasSymbol = true;
                        break;
                    }
                    //up right
                    if (coordinate.Y - 1 >= 0 && coordinate.X + 1 < input[coordinate.Y].Length && IsSymbol(input[coordinate.Y - 1][coordinate.X + 1])) {
                        hasSymbol = true;
                        break;
                    }
                    //down left
                    if (coordinate.Y + 1 < input.Length && coordinate.X - 1 >= 0 && IsSymbol(input[coordinate.Y + 1][coordinate.X - 1])) {
                        hasSymbol = true;
                        break;
                    }
                    //down right
                    if (coordinate.Y + 1 < input.Length && coordinate.X + 1 < input[coordinate.Y].Length && IsSymbol(input[coordinate.Y + 1][coordinate.X + 1])) {
                        hasSymbol = true;
                        break;
                    }

                    //if we reach this point, we can move to the next digit of the number
                    coordinate.X++;
                }
                if (hasSymbol) {
                    partNumbers += int.Parse(number);
                }

            }
            return partNumbers;

        }


        //iterate over input starting at 0,0 and find all numbers bij checking if character is digit and moving to the right.
        //Cretes a dictonary where the coordinate is the first digit of the number and the string is the whole number
        private static Dictionary<Coordinate, string> FindNumbers(IReadOnlyList<string> input) {
            Dictionary<Coordinate, string> numbers = new Dictionary<Coordinate, string>(); // Coordinate represent the index of the first digit of the number. int is the whole number
            for (var i = 0; i < input.Count; i++) {
                bool found = false;
                string currentNumber = "";
                Coordinate currentCoordinate = null;
                for (var j = 0; j < input[i].Length; j++) {

                    //we have reached the end of the line and we have a number
                    if (j == input[i].Length - 1 && found) {
                        currentNumber += char.IsDigit(input[i][j]) ? input[i][j] : "";
                        numbers.Add(currentCoordinate, currentNumber);
                        currentNumber = "";
                        found = false;
                    }

                    //we are starting a new number
                    if (!found && char.IsDigit(input[i][j])) {
                        currentCoordinate = new Coordinate(j, i);
                        currentNumber += input[i][j];
                        found = true;
                        continue;
                    }

                    //we are adding to the current number
                    if (found && char.IsDigit(input[i][j])) {
                        currentNumber += input[i][j];
                        continue;
                    }

                    if (found && !char.IsDigit(input[i][j])) {
                        //we have reached the end of the number
                        numbers.Add(currentCoordinate, currentNumber);
                        currentNumber = "";
                        found = false;
                    }
                }
            }
            return numbers;
        }
        private static bool IsSymbol(char c) {
            return !char.IsDigit(c) && c != '.';
        }

        override protected long SolveTwo() {
            var input = ReadFileToArray(PathOne);
            var starSymbols = new HashSet<Coordinate>();

            //generate a list of all coordinates with a star
            for (var i = 0; i < input.Length; i++) {
                for (int j = 0; j < input[i].Length; j++) {
                    if (input[i][j] == '*')
                        starSymbols.Add(new Coordinate(j, i));
                }
            }

            //iterate over the stars checking the surrounding for numbers.
            //Starting with left and right which is relativly easy because a number either ends or starts with a star
            //Then we check the up left, up, up right, down left, down, down right
            //for each of these positions we need to determine which part of the number we are looking at
            //we assume we are looking at the middle part and move right adding all subsequent digits to the number
            //once we reach the end of the line or a symbol we reverse the number and add every number to the list.
            // we reverse again to get the number in the correct order and add it to the list of the current star
            //to get the result we aggregate the list of numbers for each star when the list has more than 1 number and add the result to the total
            Dictionary<Coordinate, List<int>> starNumbers = new Dictionary<Coordinate, List<int>>();
            foreach (var starSymbol in starSymbols) {
                starNumbers.Add(starSymbol, []);
                //check left
                if (starSymbol.X - 1 >= 0 && char.IsDigit(input[starSymbol.Y][starSymbol.X - 1])) //there is a number to the left
                {
                    string number = "";
                    Coordinate currentCoordinate = new Coordinate(starSymbol.X - 1, starSymbol.Y);
                    char currentChar = input[currentCoordinate.Y][currentCoordinate.X];
                    while (currentCoordinate.X >= 0 && char.IsDigit(currentChar)) {
                        number += currentChar;
                        currentCoordinate.X--;
                        if (currentCoordinate.X >= 0)
                            currentChar = input[currentCoordinate.Y][currentCoordinate.X];
                    }
                    //reverse the number
                    char[] charArray = number.ToCharArray();
                    Array.Reverse(charArray);
                    number = new string(charArray);
                    starNumbers[starSymbol].Add(int.Parse(number));
                }
                //check right
                if (starSymbol.X + 1 < input[starSymbol.Y].Length && char.IsDigit(input[starSymbol.Y][starSymbol.X + 1])) //there is a number to the right
                {
                    string number = "";
                    Coordinate currentCoordinate = new Coordinate(starSymbol.X + 1, starSymbol.Y);
                    char currentChar = input[currentCoordinate.Y][currentCoordinate.X];
                    while (currentCoordinate.X < input[currentCoordinate.Y].Length && char.IsDigit(currentChar)) {
                        number += currentChar;
                        currentCoordinate.X++;
                        if (currentCoordinate.X < input[currentCoordinate.Y].Length)
                            currentChar = input[currentCoordinate.Y][currentCoordinate.X];
                    }
                    starNumbers[starSymbol].Add(int.Parse(number));
                }

                //check up left
                bool hasUpLeft = false;
                if (starSymbol.X - 1 >= 0 && starSymbol.Y - 1 >= 0 && char.IsDigit(input[starSymbol.Y - 1][starSymbol.X - 1])) //there is a number to the up left
                {
                    hasUpLeft = true;
                    var number = ExploreNumber(new Coordinate(starSymbol.X - 1, starSymbol.Y - 1), input);
                    starNumbers[starSymbol].Add(int.Parse(number));
                }
                //check up
                bool hasUp = false;
                if (starSymbol.Y - 1 >= 0 && char.IsDigit(input[starSymbol.Y - 1][starSymbol.X])) //there is a number to the up
                {
                    hasUp = true;
                    if (!hasUpLeft) {
                        var number = ExploreNumber(new Coordinate(starSymbol.X, starSymbol.Y - 1), input);
                        starNumbers[starSymbol].Add(int.Parse(number));
                    }
                }
                //check up right
                if (starSymbol.X + 1 < input[starSymbol.Y].Length && starSymbol.Y - 1 >= 0 && char.IsDigit(input[starSymbol.Y - 1][starSymbol.X + 1]) && !hasUp) //there is a number to the up right
                {
                    var number = ExploreNumber(new Coordinate(starSymbol.X + 1, starSymbol.Y - 1), input);
                    starNumbers[starSymbol].Add(int.Parse(number));
                }
                //check down left
                bool hasDownLeft = false;
                if (starSymbol.X - 1 >= 0 && starSymbol.Y + 1 < input.Length && char.IsDigit(input[starSymbol.Y + 1][starSymbol.X - 1])) //there is a number to the down left
                {
                    var number = ExploreNumber(new Coordinate(starSymbol.X - 1, starSymbol.Y + 1), input);
                    hasDownLeft = true;
                    starNumbers[starSymbol].Add(int.Parse(number));
                }
                //check down
                bool hasDown = false;
                if (starSymbol.Y + 1 < input.Length && char.IsDigit(input[starSymbol.Y + 1][starSymbol.X])) //there is a number to the down
                {
                    hasDown = true;
                    if (!hasDownLeft) {
                        //we have not already added the number to the down left
                        var number = ExploreNumber(new Coordinate(starSymbol.X, starSymbol.Y + 1), input);
                        starNumbers[starSymbol].Add(int.Parse(number));
                    }
                }
                //check down right
                if (starSymbol.X + 1 < input[starSymbol.Y].Length && starSymbol.Y + 1 < input.Length && char.IsDigit(input[starSymbol.Y + 1][starSymbol.X + 1]) && !hasDown) //there is a number to the down right
                {
                    var number = ExploreNumber(new Coordinate(starSymbol.X + 1, starSymbol.Y + 1), input);
                    starNumbers[starSymbol].Add(int.Parse(number));
                }


            }


            return starNumbers.Where(x => x.Value.Count > 1).Sum(keyValuePair => keyValuePair.Value.Aggregate(1L, (current, number) => current * number));
        }
        private static string ExploreNumber(Coordinate coordinate, string[] input) {
            string number = "";
            Coordinate currentCoordinate = new Coordinate(coordinate.X, coordinate.Y);
            char currentChar = input[currentCoordinate.Y][currentCoordinate.X];
            while (currentCoordinate.X < input[coordinate.Y].Length && char.IsDigit(currentChar)) {
                number += currentChar;
                currentCoordinate.X++; //move right
                if (currentCoordinate.X < input[currentCoordinate.Y].Length)
                    currentChar = input[currentCoordinate.Y][currentCoordinate.X];
            }

            //reverse the number
            char[] charArray = number.ToCharArray();
            Array.Reverse(charArray);
            number = new string(charArray);

            //move left
            currentCoordinate.X = coordinate.X - 1;
            currentChar = currentCoordinate.X >= 0 ? input[currentCoordinate.Y][currentCoordinate.X] : ' ';

            while (currentCoordinate.X >= 0 && char.IsDigit(currentChar)) {
                number += currentChar;
                currentCoordinate.X--; //move left
                if (currentCoordinate.X >= 0)
                    currentChar = input[currentCoordinate.Y][currentCoordinate.X];
            }
            //reverse the number
            charArray = number.ToCharArray();
            Array.Reverse(charArray);
            number = new string(charArray);
            return number;
        }
    }
}
