using Day00;

namespace Day08 {
    internal class Program : Base {
        public static void Main(string[] args) {
            new Program();
        }

        override protected long SolveOne() {
            var input = ReadFileToArray(PathOne);
            string instructions = input[0];

            //parse and create datastructure
            var allNodes = GenerateMap(input);

            //traverse until we reach node ZZZ - note the amount of steps needed
            int steps = 0;
            int instructionPointer = 0;
            Node currentNode = GetOrCreateNode(allNodes, "AAA");
            while (currentNode.Name != "ZZZ") {
                var instruction = instructions.Substring(instructionPointer, 1);
                instructionPointer++;
                if(instructionPointer >= instructions.Length) instructionPointer = 0;
                steps++;

                if (instruction == "L") {
                    currentNode = currentNode.Left;
                } else if (instruction == "R") {
                    currentNode = currentNode.Right;
                }
            }


            return steps;
        }
        private HashSet<Node> GenerateMap(string[] input) {
            HashSet<Node> allNodes = new HashSet<Node>();
            for (var i = 2; i < input.Length; i++) {
                var split = input[i].Split(" = ");
                var rootNode = GetOrCreateNode(allNodes, split[0]);

                var split2 = split[1].Trim().Replace("(", string.Empty).Replace(")", string.Empty).Split(",");

                rootNode.Left = GetOrCreateNode(allNodes, split2[0]);
                rootNode.Right = GetOrCreateNode(allNodes, split2[1]);
            }
            return allNodes;
        }




        override protected long SolveTwo() {
            var input = ReadFileToArray(PathOne);
            string instructions = input[0];
            var allNodes = GenerateMap(input);

            //traverse until we reach node ZZZ - note the amount of steps needed

            List<Node> allStartingNodes = allNodes.Where(n => n.Name.EndsWith("A")).ToList();
            List<long> intervals = [];
            foreach (var startNode in allStartingNodes) {
                long steps = 0;
                var instructionPointer = 0;

                Node currentNode = GetOrCreateNode(allNodes, startNode.Name);
                while (currentNode.Name[2] != 'Z') {
                    var instruction = instructions.Substring(instructionPointer, 1);
                    instructionPointer++;
                    if (instructionPointer >= instructions.Length) instructionPointer = 0;
                    steps++;

                    if (instruction == "L") {
                        currentNode = currentNode.Left;
                    }
                    else if (instruction == "R") {
                        currentNode = currentNode.Right;
                    }
                }
                intervals.Add(steps);
            }


            return LCM(intervals);



        }

        public bool allNodesEndWithZ(List<Node> nodes) {
            bool result = false;
            foreach (var node in nodes) {
                if (node.Name.EndsWith("Z")) {
                    result = true;
                } else {
                    result = false;
                    break;
                }
            }
            return result;
        }
        private Node GetOrCreateNode(ISet<Node> allNodes, string name) {
            Node node = allNodes.Where(n => n.Name == name.Trim()).FirstOrDefault();
            if (node != null)
                return node;
            node = new Node { Name = name.Trim() };
            allNodes.Add(node);

            return node;
        }
    }
    class Node : IEquatable<Node> {
        public string Name { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }
        //compare equality by name
        public bool Equals(Node? other) {
            return other != null && Name.Equals(other.Name);
        }
        public override bool Equals(object? obj) {
            if (obj == null || GetType() != obj.GetType())
                return false;
            return Equals((Node)obj);
        }
        public override int GetHashCode() {
            return Name.GetHashCode();
        }
    }
}
