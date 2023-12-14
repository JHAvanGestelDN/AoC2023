using Day00;

namespace Day01 {
    internal class Program : Base {
        public static void Main(string[] args) {
            new Program();
        }

        override protected long SolveOne() {
            var input = ReadFileToArray(PathOne);
            long sum = 0;
            foreach (var s in input) {
                string number = string.Empty;
                for (int i = 0; i < s.Length; i++) {
                    char c = s[i];
                    if (!char.IsDigit(c))
                        continue;
                    number += c;
                    break;
                }
                //reverse loop to do the same
                for (int i = s.Length - 1; i >= 0; i--) {
                    char c = s[i];
                    if (!char.IsDigit(c))
                        continue;
                    number += c;
                    break;
                }
                int.TryParse(number, out int result);
                sum += result;
            }
            return sum;
        }

        override protected long SolveTwo() {
            Dictionary<string, int> numbersInText = new Dictionary<string, int> {
            { "one", 1 },
            { "two", 2 },
            { "three", 3 },
            { "four", 4 },
            { "five", 5 },
            { "six", 6 },
            { "seven", 7 },
            { "eight", 8 },
            { "nine", 9 },
            { "1", 1 },
            { "2", 2 },
            { "3", 3 },
            { "4", 4 },
            { "5", 5 },
            { "6", 6 },
            { "7", 7 },
            { "8", 8 },
            { "9", 9 },
            { "0", 0 }


            };
            var input = ReadFileToArray(PathOne);
            long answer = 0;
            foreach (var s in input) {
                string result = string.Empty;
                List<int> numbers = new List<int>();

                // iterate over the array of numbers in text
                // check if s contains the number in text and if so, map the index of the first letter
                for (int i = 0; i < s.Length; i++) {
                    for (int j = 0; j < numbersInText.Count; j++) {
                        //check if the remaining amount of letters is enough to contain the number in text
                        if (i + numbersInText.ElementAt(j).Key.Length > s.Length)
                            continue;
                        string sub = s.Substring(i, numbersInText.ElementAt(j).Key.Length);
                        if (sub == numbersInText.ElementAt(j).Key) {
                            numbers.Add(numbersInText.ElementAt(j).Value);
                        }
                    }
                }
                result += numbers.First().ToString();
                result += numbers.Last().ToString();
                int res = int.Parse(result);
                answer += res;
            }
            return answer;
        }
    }
}
