using System.Diagnostics.CodeAnalysis;
using Day00;

namespace Day05 {
    internal class Program : Base {

        public static void Main(string[] args) {
            new Program();
        }

        override protected long SolveOne() {
            var input = ReadFileToArray(PathOne);
            var seedLine = input[0];
            var seedIds = seedLine.Split(": ")[1].Split(" ").Select(long.Parse);
            var rangeMaps = CreateRangeMaps(input);
            var lowest = ProcessSeeds(seedIds, rangeMaps);


            return lowest;
        }
        private static Dictionary<int, List<RangeMap>> CreateRangeMaps(string[] input) {
            Dictionary<int, List<RangeMap>> rangeMaps = new Dictionary<int, List<RangeMap>>();

            int mapIndex = 0;
            bool processing = false;
            for (var i = 2; i < input.Length; i++) {
                string line = input[i];
                if (string.IsNullOrEmpty(line)) {
                    processing = false;
                    mapIndex++;
                    continue;
                }
                if (line.Contains(':')) {
                    processing = true;
                    continue;
                }
                if (processing) {
                    if (!rangeMaps.ContainsKey(mapIndex)) {
                        rangeMaps.Add(mapIndex, new List<RangeMap>());
                    }
                    var split = line.Split(" ").Select(long.Parse).ToList();
                    var rangeMap = new RangeMap() {
                    DestinationRangeStart = split[0],
                    SourceRangeStart = split[1],
                    RangeLength = split[2]
                    };
                    rangeMaps[mapIndex].Add(rangeMap);
                }
            }
            return rangeMaps;
        }
        private long ProcessSeeds(IEnumerable<long> seedIds, Dictionary<int, List<RangeMap>> rangeMaps) {
            long lowest = long.MaxValue;


            foreach (var seedId in seedIds) {
                var seedNumber = seedId;
                var soil = rangeMaps[0].Select(x => x.GetDestination(seedNumber)).FirstOrDefault(y => y >= 0, defaultValue: seedNumber);
                var fertilizer = rangeMaps[1].Select(x => x.GetDestination(soil)).FirstOrDefault(y => y >= 0, defaultValue: soil);
                var water = rangeMaps[2].Select(x => x.GetDestination(fertilizer)).FirstOrDefault(y => y >= 0, defaultValue: fertilizer);
                var light = rangeMaps[3].Select(x => x.GetDestination(water)).FirstOrDefault(y => y >= 0, defaultValue: water);
                var temp = rangeMaps[4].Select(x => x.GetDestination(light)).FirstOrDefault(y => y >= 0, defaultValue: light);
                var humidity = rangeMaps[5].Select(x => x.GetDestination(temp)).FirstOrDefault(y => y >= 0, defaultValue: temp);
                var location = rangeMaps[6].Select(x => x.GetDestination(humidity)).FirstOrDefault(y => y >= 0, defaultValue: humidity);
                if (location < lowest) {
                    lowest = location;
                }
            }
            return lowest;
        }



        override protected long SolveTwo() {
            var input = ReadFileToArray(PathOne);
            var seedLine = input[0];

            List<List<long>> seedIdRanges = new List<List<long>>();
            var seedNumber = seedLine.Split(": ")[1].Split(" ").Select(long.Parse).ToList();
            var seedIds = new List<long>();

            for (var i = 0; i < seedNumber.Count; i += 2) {
                var start = seedNumber[i];
                var end = seedNumber[i] + seedNumber[i + 1];

                for (var j = start; j < end; j++) {
                    if (seedIds.Count % 50000 == 0) {
                        seedIdRanges.Add(seedIds);
                        seedIds = new List<long>();
                    }
                    seedIds.Add(j);
                }

                seedIdRanges.Add(seedIds);
            }


            var rangeMaps = CreateRangeMaps(input);
            var tasks = new List<Task<long>>();

            foreach (var seedIdRange in seedIdRanges) {
                tasks.Add(Task.Run(() => ProcessSeeds(seedIdRange, rangeMaps)));
            }


            return Threading(tasks).Result;
        }
        private static async Task<long> Threading(IEnumerable<Task<long>> tasks) {
            long result = long.MaxValue;
            List<Exception> exceptions = new List<Exception>();

            foreach (var task in tasks) {
                try {
                    var res = await task;
                    if (res < result) {
                        result = res;
                    }
                }
                catch (Exception ex) {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Any()) {
                throw new AggregateException(exceptions);
            }

            return result;
        }
    }

    class RangeMap {
        public long DestinationRangeStart { get; set; }
        public long SourceRangeStart { get; set; }
        public long RangeLength { get; set; }

        public long DestinationRangeEnd => DestinationRangeStart + RangeLength;
        public long SourceRangeEnd => SourceRangeStart + RangeLength;
        public long GetDestination(long source) {
            if (source >= SourceRangeStart && source <= SourceRangeEnd) {
                return DestinationRangeStart + (source - SourceRangeStart);
            }
            return -1;
        }

        public long GetSource(long destination) {
            if (destination >= DestinationRangeStart && destination <= DestinationRangeEnd) {
                return SourceRangeStart + (destination - DestinationRangeStart);
            }
            return -1;
        }

        public long ReveseGetDestination() {
            long lowest = long.MaxValue;
            for (long i = DestinationRangeStart; i < DestinationRangeEnd; i++) {
                var source = SourceRangeStart + (i);
                if (source < lowest) {
                    lowest = source;
                }

            }
            return lowest;
        }
    }
}
