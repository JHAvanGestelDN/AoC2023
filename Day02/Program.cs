using Day00;

namespace Day02 {
    internal class Program : Base {
        public static void Main(string[] args) {
            new Program();
        }

        override protected long SolveOne() {
            var input = ReadFileToArray(PathOne);
            var games = input.Select(line => new Game(line)).ToList();

            const int maxBlue = 14;
            const int maxRed = 12;
            const int maxGreen = 13;


            return games.Where(game => game is { Blue: <= maxBlue, Red: <= maxRed, Green: <= maxGreen }).Sum(game => game.Id);
        }

        override protected long SolveTwo() {
            var input = ReadFileToArray(PathOne);
            var games = input.Select(line => new Game(line)).ToList();


            return games.Sum(game => game.Power);
        }
    }
}
