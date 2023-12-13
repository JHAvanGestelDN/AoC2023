using System.Collections;
using Day00;

namespace Day07 {
    internal class Program : Base {
        public static void Main(string[] args) {
            new Program();
        }

        override protected long SolveOne() {
            var input = ReadFileToArray(PathOne);
            var games = new List<Game>();
            foreach (var s in input) {
                var split = s.Split(" ");
                var hand = split[0];
                var bid = split[1];
                var game = new Game() {
                Hand = hand,
                Bid = int.Parse(bid)
                };
                game.DetermineType();
                games.Add(game);
            }
            var sortedGames = games.OrderBy(x => x.HandType).ToList().GroupBy(x => x.HandType);
            int rank = input.Length;
            foreach (var sortedGame in sortedGames) {
                if (sortedGame.Count() == 1) {
                    sortedGame.First().Rank = rank;
                    rank--;
                }
                else {
                    var sortedGameList = sortedGame.ToList();
                    //sort the list with a custom comparer
                    sortedGameList.Sort();
                    foreach (var game in sortedGameList) {
                        game.Rank = rank;
                        rank--;
                    }
                }
            }

            //sort the game list based on rank
            games = games.OrderBy(g => g.Rank).ToList();

            return games.Sum(x => x.Value);

        }

        override protected long SolveTwo() {
            var input = ReadFileToArray(PathOne);
            var games = new List<JokerGame>();
            foreach (var s in input) {
                var split = s.Split(" ");
                var hand = split[0];
                var bid = split[1];
                var game = new JokerGame() {
                Hand = hand,
                Bid = int.Parse(bid)
                };
                game.DetermineTypeWithJoker();
                games.Add(game);
            }
            var sortedGames = games.OrderBy(x => x.HandType).ToList().GroupBy(x => x.HandType);
            int rank = input.Length;
            foreach (var sortedGame in sortedGames) {
                if (sortedGame.Count() == 1) {
                    sortedGame.First().Rank = rank;
                    rank--;
                }
                else {
                    var sortedGameList = sortedGame.ToList();

                    sortedGameList.Sort();
                    foreach (var game in sortedGameList) {
                        game.Rank = rank;
                        rank--;
                    }
                }
            }

            //sort the game list based on rank
            games = games.OrderBy(g => g.Rank).ToList();

            return games.Sum(x => x.Value);
        }
    }

    class JokerGame : Game, IComparable {
        public void DetermineTypeWithJoker() {
            if (!Hand.Contains('J')) {
                DetermineType();
                return;
            }
            else {
                // We have a joker that can be used as any card and we need to determine the best hand.
                // We can use the joker as any card to make the best hand.
                // Iterate through card types to determine highest rank.
                CardEnum bestCardReplacement = CardEnum.A; // Assuming "A" is the best card.
                TypeEnum bestType = TypeEnum.HighCard; // Assuming HighCard is the worst type.

                foreach (CardEnum cardType in Enum.GetValues(typeof(CardEnum))) {
                    string tempHand = Hand.Replace('J', GetCardChar(cardType));
                    var tempList = new List<char>(tempHand);
                    tempList.Sort();
                    tempHand = new string(tempList.ToArray());

                    var tempGame = new Game() {
                    Hand = tempHand,
                    Bid = Bid
                    };

                    tempGame.DetermineType();

                    if (tempGame.HandType < bestType) {
                        bestType = tempGame.HandType;
                    }
                }

                HandType = bestType;
            }
        }
        public int CompareTo(object? obj) {
            Game other = (Game)obj;
            return HandIsHigher(this.Hand, other.Hand);
        }

        public int HandIsHigher(string hand, string otherHand) {
            // characters in order of value A K Q J T 9 8 7 6 5 4 3 2
            for (int i = 0; i < 5; i++) {
                JokerCardEnum firstHandCard = GetJokerCardEnum(hand[i]);
                JokerCardEnum otherHandCard = GetJokerCardEnum(otherHand[i]);
                if (firstHandCard < otherHandCard) {
                    return -1;
                }
                else if (firstHandCard > otherHandCard) {
                    return 1;
                }
            }
            return 0;

        }
        private JokerCardEnum GetJokerCardEnum(char card) {
            switch (card) {
                case 'A':
                    return JokerCardEnum.A;
                case 'K':
                    return JokerCardEnum.K;
                case 'Q':
                    return JokerCardEnum.Q;
                case 'J':
                    return JokerCardEnum.J;
                case 'T':
                    return JokerCardEnum.T;
                case '9':
                    return JokerCardEnum.Nine;
                case '8':
                    return JokerCardEnum.Eight;
                case '7':
                    return JokerCardEnum.Seven;
                case '6':
                    return JokerCardEnum.Six;
                case '5':
                    return JokerCardEnum.Five;
                case '4':
                    return JokerCardEnum.Four;
                case '3':
                    return JokerCardEnum.Three;
                case '2':
                    return JokerCardEnum.Two;
                default:
                    throw new ArgumentOutOfRangeException(nameof(card), card, null);
            }
        }
    }
    class Game : IComparable {
        public string Hand { get; set; }
        public int Bid { get; set; }
        public int Rank { get; set; }

        public long Value => Bid * Rank;

        public TypeEnum HandType { get; set; }
        public void DetermineType() {

            var occurrences = new Dictionary<char, int>();
            foreach (var symbol in Hand) {
                if (occurrences.ContainsKey(symbol)) {
                    occurrences[symbol]++;
                }
                else {
                    occurrences[symbol] = 1;
                }
            }

            var numberOfDifferentCards = occurrences.Keys.Count;
            var numberOfOccurrences = occurrences.Values.ToList();

            numberOfOccurrences.Sort();

            if (numberOfDifferentCards == 1) {
                HandType = TypeEnum.FiveOfAKind;
            }
            else if (numberOfDifferentCards == 2 && numberOfOccurrences[1] == 4) {
                HandType = TypeEnum.FourOfAKind;
            }
            else if (numberOfDifferentCards == 2 && numberOfOccurrences[1] == 3) {
                HandType = TypeEnum.FullHouse;
            }
            else if (numberOfDifferentCards == 3 && numberOfOccurrences[2] == 3) {
                HandType = TypeEnum.ThreeOfAKind;
            }
            else if (numberOfDifferentCards == 3) {
                HandType = TypeEnum.TwoPair;
            }
            else if (numberOfDifferentCards == 4) {
                HandType = TypeEnum.OnePair;
            }
            else {
                HandType = TypeEnum.HighCard;
            }
        }

        public int CompareTo(object? obj) {
            Game other = (Game)obj;
            return HandIsHigher(this.Hand, other.Hand);
        }
        public int HandIsHigher(string hand, string otherHand) {
            // characters in order of value A K Q J T 9 8 7 6 5 4 3 2
            for (int i = 0; i < 5; i++) {
                CardEnum firstHandCard = GetCardEnum(hand[i]);
                CardEnum otherHandCard = GetCardEnum(otherHand[i]);
                if (firstHandCard < otherHandCard) {
                    return -1;
                }
                else if (firstHandCard > otherHandCard) {
                    return 1;
                }
            }
            return 0;

        }
        public char GetCardChar(CardEnum card) {
            switch (card) {
                case CardEnum.A:
                    return 'A';
                case CardEnum.K:
                    return 'K';
                case CardEnum.Q:
                    return 'Q';
                case CardEnum.J:
                    return 'J';
                case CardEnum.T:
                    return 'T';
                case CardEnum.Nine:
                    return '9';
                case CardEnum.Eight:
                    return '8';
                case CardEnum.Seven:
                    return '7';
                case CardEnum.Six:
                    return '6';
                case CardEnum.Five:
                    return '5';
                case CardEnum.Four:
                    return '4';
                case CardEnum.Three:
                    return '3';
                case CardEnum.Two:
                    return '2';
                default:
                    throw new ArgumentOutOfRangeException(nameof(card), card, null);
            }
        }
        public CardEnum GetCardEnum(char card) {
            switch (card) {
                case 'A':
                    return CardEnum.A;
                case 'K':
                    return CardEnum.K;
                case 'Q':
                    return CardEnum.Q;
                case 'J':
                    return CardEnum.J;
                case 'T':
                    return CardEnum.T;
                case '9':
                    return CardEnum.Nine;
                case '8':
                    return CardEnum.Eight;
                case '7':
                    return CardEnum.Seven;
                case '6':
                    return CardEnum.Six;
                case '5':
                    return CardEnum.Five;
                case '4':
                    return CardEnum.Four;
                case '3':
                    return CardEnum.Three;
                case '2':
                    return CardEnum.Two;
                default:
                    throw new ArgumentOutOfRangeException(nameof(card), card, null);
            }
        }
    }

    enum TypeEnum {
        FiveOfAKind = 1,
        FourOfAKind = 2,
        FullHouse = 3,
        ThreeOfAKind = 4,
        TwoPair = 5,
        OnePair = 6,
        HighCard = 7
    }
    enum CardEnum {
        A = 1,
        K = 2,
        Q = 3,
        J = 4,
        T = 5,
        Nine = 6,
        Eight = 7,
        Seven = 8,
        Six = 9,
        Five = 10,
        Four = 11,
        Three = 12,
        Two = 13
    }

    enum JokerCardEnum {
        A = 1,
        K = 2,
        Q = 3,
        T = 4,
        Nine = 5,
        Eight = 6,
        Seven = 7,
        Six = 8,
        Five = 9,
        Four = 10,
        Three = 11,
        Two = 12,
        J = 13,
    }
}
