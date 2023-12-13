using Day00;

namespace Day06 {
    internal class Program : Base {
        public static void Main(string[] args) {
            new Program();
        }

        override protected long SolveOne() {
            var input = ReadFileToArray(PathOne);
            var result = Work(input);


            return result;
        }
        private static long Work(string[] input) {
            var times = input[0].Split("Time: ")[1].Split(" ").Where(x => x != "").Select(long.Parse).ToList();
            var distances = input[1].Split("Distance: ")[1].Split(" ").Where(x => x != "").Select(long.Parse).ToList();
            var games = new List<Games>();
            for (var i = 0; i < times.Count; i++) {
                games.Add(new Games() {
                Time = times[i],
                Distance = distances[i]
                });
            }

            long result = 1;
            foreach (var game in games) {
                //determine how long we can hold the button max
                long maxTime = game.Time;
                long maxTimeTmp = maxTime;
                long amountOfWinningCombis = 0;
                for (long i = maxTime; i >= 0; i--) {
                    maxTimeTmp = maxTime;
                    //press the button for this amount of time, remove i from max and calculate distances for remaining time
                    long speed = 1 * i;
                    long distance = speed * (maxTimeTmp - i);
                    if (distance < 0) distance = 0;

                    //if disnance is more than max distances this combi wins the game
                    if (distance > game.Distance) {
                        amountOfWinningCombis++;
                    }
                }
                result *= amountOfWinningCombis;

            }
            return result;
        }

        override protected long SolveTwo() {
            var input = ReadFileToArray(PathTwo); //manually adjusted the path 2 file
            var result = Work(input);
            return result;
        }
    }
    class Games {
        public long Time { get; set; }
        public long Distance { get; set; }
    }
}
