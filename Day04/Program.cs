using Day00;

namespace Day04 {
    internal class Program : Base {
        public static void Main(string[] args) {
            new Program();
        }

        override protected long SolveOne() {
            var input = ReadFileToArray(PathOne);
            List<Card> cards = new List<Card>();
            foreach (var line in input) {
                var split = line.Split(" | ");
                var cardInfo = split[0].Split(": ")[0]; //Card X:
                var winningNumbers = split[0].Split(": ")[1].Split(' ').Where(x => x != "").Select(int.Parse); //Card X:
                var myNumbers = split[1].Split(' ')
                .Where(x => x != "")
                .Select(int.Parse);
                var card = new Card {
                Id = int.Parse(cardInfo.Split(' ').Last(x => x != "")),
                WinningNumbers = winningNumbers.ToArray(),
                MyNumbers = myNumbers.ToArray()
                };
                card.GetCardScore();
                cards.Add(card);


            }
            return cards.Sum(x => x.CardScore);
        }

        override protected long SolveTwo() {
            var input = ReadFileToArray(PathOne);
            List<Card> cards = new List<Card>();
            Dictionary<Card, int> cardCollection = new Dictionary<Card, int>();
            foreach (var line in input) {
                var split = line.Split(" | ");
                var cardInfo = split[0].Split(": ")[0]; //Card X:
                var winningNumbers = split[0].Split(": ")[1].Split(' ').Where(x => x != "").Select(int.Parse); //Card X:
                var myNumbers = split[1].Split(' ')
                .Where(x => x != "")
                .Select(int.Parse);
                var card = new Card {
                Id = int.Parse(cardInfo.Split(' ').Last(x => x != "")),
                WinningNumbers = winningNumbers.ToArray(),
                MyNumbers = myNumbers.ToArray()
                };
                cards.Add(card);
            }
            int index = 1;
            cards.ForEach(card => {
                cardCollection.Add(card, 1); //add the originals
            });

            foreach (var card in cards) {
                int currentAmountOfThisCard = cardCollection[card];
                var score = card.GetCardScore();
                for (int i = index; i < index + score; i++) {
                    cardCollection[cards[i]] += 1 * currentAmountOfThisCard;
                }
                index++;

            }


            return cardCollection.Sum(x => x.Value);
        }
    }
    class Card {
        public int Id { get; set; }
        public int[] WinningNumbers { get; set; }
        public int[] MyNumbers { get; set; }

        public int CardScore { get; set; }

        //returns amount of matching numbers for part 2
        public int GetCardScore() {
            int amountOfMatchingNumbers = 0;
            MyNumbers.ToList().ForEach(myNumber => {
                if (WinningNumbers.Contains(myNumber)) {
                    amountOfMatchingNumbers++;
                }
            });
            //The first match makes the card worth one point and each match after the first doubles the point value of that card.
            CardScore = amountOfMatchingNumbers == 0 ? 0 : (int)Math.Pow(2, amountOfMatchingNumbers - 1);
            return amountOfMatchingNumbers;
        }
    }
}
