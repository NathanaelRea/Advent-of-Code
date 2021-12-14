using System;
using System.Collections.Generic;
using System.Linq;

class Day13
{
    static void Main()
    {
        foreach (string inputFile in System.IO.Directory.EnumerateFiles(".input"))
        {
            string[] input = System.IO.File.ReadAllLines(inputFile);
            (NodeList, List<(int, int)>) parsedInput = ParseInput(input);

            int partAResult = CalculatePartA(parsedInput);
            string partBResult = CalculatePartB(parsedInput);

            Console.WriteLine(inputFile);
            Console.WriteLine($"Part A: {partAResult}");
            Console.WriteLine($"Part B: {partBResult}");
        }
    }

    static int CalculatePartA((NodeList nodeList, List<(int, int)> folds) input)
    {
        (int x, int y) firstFold = input.folds[0];
        input.nodeList.Fold(firstFold);
        return input.nodeList.Count();
    }

    static string CalculatePartB((NodeList nodeList, List<(int, int)> folds) input)
    {
        input.folds.RemoveAt(0); // Part A already used first fold

        foreach ((int x, int y) fold in input.folds)
            input.nodeList.Fold(fold);

        return input.nodeList.Print();
    }

    static (NodeList, List<(int, int)>) ParseInput(string[] input)
    {
        int x, y;
        NodeList nodeList = new NodeList();
        List<(int, int)> folds = new List<(int, int)>();

        foreach (string line in input)
        {
            if (line == "")
                continue;

            if (line.StartsWith("fold"))
            {
                string[] splitLine = line.Split("=");
                x = y = 0;
                if (splitLine[0].EndsWith("x"))
                    x = int.Parse(splitLine[1]);
                else
                    y = int.Parse(splitLine[1]);
                folds.Add((x, y));
            }
            else
            {
                string[] splitLine = line.Split(",");
                x = int.Parse(splitLine[0]);
                y = int.Parse(splitLine[1]);
                nodeList.AddNode(x, y);
            }
        }

        return (nodeList, folds);
    }

    class NodeList
    {
        private int maxX;
        private int maxY;
        private List<Node> nodes = new List<Node>();

        public int Count()
        {
            RemoveDuplicateNodes();
            return nodes.Count;
        }
        public string Print()
        {
            RemoveDuplicateNodes();
            int[,] grid = new int[maxX+1, maxY+1];
            foreach(Node node in nodes)
                grid[node.x, node.y] = 1;

            string output = "\n";
            for (int row = 0; row <= maxY; row++)
            {
                for (int col = 0; col <= maxX; col++)
                {
                    if (grid[col,row] == 1)
                        output += "#";
                    else
                        output += ".";
                }
                output += "\n";
            }

            return output;
        }

        public void AddNode(int x, int y)
        {
            nodes.Add(new Node(x, y));
        }

        public void Fold((int x, int y) fold)
        {
            if (fold.x == 0)
            {
                foreach (Node node in nodes)
                {
                    if (node.y > fold.y)
                    {
                        node.y = 2 * fold.y - node.y;
                    }
                }
            }
            else
            {
                foreach (Node node in nodes)
                {
                    if (node.x > fold.x)
                    {
                        node.x = 2 * fold.x - node.x;
                    }
                }
            }
        }

        private void RemoveDuplicateNodes()
        {
            maxX = 0;
            maxY = 0;
            HashSet<(int, int)> nodeHash = new HashSet<(int, int)>();
            foreach (Node node in nodes)
            {
                nodeHash.Add(node.AsTuple());
                maxX = Math.Max(maxX, node.x);
                maxY = Math.Max(maxY, node.y);
            }

            nodes = new List<Node>();
            foreach ((int, int) nodeTuple in nodeHash)
                nodes.Add(new Node(nodeTuple.Item1, nodeTuple.Item2));
        }
    }

    class Node
    {
        public int x;
        public int y;

        public Node(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public (int, int) AsTuple()
        {
            return (x, y);
        }
    }
}