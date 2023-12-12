namespace Day02 {
    class Game {
        internal int Blue { get; set; }
        internal int Red { get; set; }
        internal int Green { get; set; }
        internal int Id { get; set; }
        internal int Power { get; set; }

        public Game(string input) {
            var split = input.Split(':');
            var gameNumber = split[0].Split(' ')[1];
            Id = int.Parse(gameNumber);
            var sets = split[1].Split(';');
            foreach (var set in sets) {
                // a set looks like this: 7 green, 14 red, 5 blue; 8 red, 4 green; 6 green, 18 red, 9 blue
                //split and determine the color and the number
                var colors = set.Split(',');
                foreach (var color in colors) {
                    var colorSplit = color.Trim().Split(' ');
                    var colorName = colorSplit[1];
                    var colorNumber = int.Parse(colorSplit[0]);
                    switch (colorName) {
                        case "green":
                            Green = Math.Max(Green, colorNumber);
                            break;
                        case "red":
                            Red = Math.Max(Red, colorNumber);
                            break;
                        case "blue":
                            Blue = Math.Max(Blue, colorNumber);

                            break;
                    }
                }
                Power = Blue * Red * Green;
            }
        }
    }
}
