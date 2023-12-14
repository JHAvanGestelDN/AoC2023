using Day00;

namespace Day09 {
    internal class Program : Base {
        public static void Main(string[] args) {
            new Program();
        }

        override protected long SolveOne() {
            var input = ReadFileToArray(PathOne);
            long result = 0;

            foreach (var s in input) {
                Reading reading = new Reading();
                reading.ParseReading(s);
                result +=  reading.DetermineNextReading(reading.Readings);

            }
            return result;
        }

        override protected long SolveTwo() {
            var input = ReadFileToArray(PathOne);
            long result = 0;

            foreach (var s in input) {
                Reading reading = new Reading();
                reading.ParseReading(s);
                result += reading.DetermineEarlierReading(reading.Readings);

            }
            return result;
        }

        class Reading() {
            public List<int> Readings { get; set; }
            public void ParseReading(string s) {
                Readings = s.Split(' ').Select(int.Parse).ToList();
            }
            public int DetermineNextReading(List<int> readings) {
                var differences = Extrapolate(readings, out var end);
                if (!end) {
                    differences.Add(0);
                }
                else {
                    differences.Add(DetermineNextReading(differences));
                }

                return readings.Last() + differences.Last();
            }
            public int DetermineEarlierReading(List<int> readings) {
                var differences = Extrapolate(readings, out var end);
                if (!end) {
                    differences.Insert(0,0);
                }
                else {
                    differences.Insert(0, DetermineEarlierReading(differences));
                }

                return readings.First() - differences.First();
            }
            private static List<int> Extrapolate(List<int> readings, out bool end) {
                List<int> differences = new List<int>();
                for (int i = 0; i < readings.Count - 1; i++) {
                    int difference = readings[i + 1] - readings[i];
                    differences.Add(difference);
                }
                end = differences.Any(x => x != 0);
                return differences;
            }
        }
    }
}
