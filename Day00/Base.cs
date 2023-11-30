using System.Diagnostics;
using System.Reflection;
namespace Day00
{
    public abstract class Base
    {
        protected readonly string PathOne;
        protected readonly string PathOneSample;
        protected readonly string PathTwo;
        protected readonly string PathTwoSample;

        protected Base()
        {
            PathOne = GetPath("input1.txt");
            PathTwo = GetPath("input2.txt");
            PathOneSample = GetPath("input1.sample.txt");
            PathTwoSample = GetPath("input2.sample.txt");

            var (res, time) = Run(SolveOne);
            Console.WriteLine($"The answer to part one is: {res}\nCalculated in: {time}.\n");

            (res, time) = Run(SolveTwo);
            Console.WriteLine($"The answer to part two is: {res} \nCalculated in: {time}.\n");
        }


        private Stopwatch Stopwatch { get; } = new Stopwatch();

        private (long res, TimeSpan time) Run(Func<long> solve)
        {
            Stopwatch.Start();
            var res = solve();
            Stopwatch.Stop();
            var time = Stopwatch.Elapsed;
            Stopwatch.Reset();
            return (res, time);
        }

        private static string GetPath(string filename)
        {
            return Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ??
            throw new InvalidOperationException(), filename);
        }

        static protected string[] ReadFileToArray(string path)
        {
            try
            {
                return File.ReadAllLines(path);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("File not found: " + path);
                return Array.Empty<string>();
            }
        }

        static protected List<long> ConvertArrayToLongList(string[] array)
        {
            return array != null ? array.Select(long.Parse).ToList() : new List<long>();
        }

        static protected List<int> ConvertArrayToIntList(string[] array)
        {
            return array != null ? array.Select(int.Parse).ToList() : new List<int>();
        }


        public static string ReverseString(string s)
        {
            var charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        abstract protected long SolveOne();
        abstract protected long SolveTwo();
    }
}
