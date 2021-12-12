using System;
using System.Collections.Generic;
using System.Linq;
class Day12
{
    static void Main()
    {
        foreach (string inputFile in System.IO.Directory.EnumerateFiles(".input"))
        {
            string[] input = System.IO.File.ReadAllLines(inputFile);
            Node parsedInput = ParseInput(input);

            int partAResult = CalculatePartA(parsedInput);
            int partBResult = CalculatePartB(parsedInput);

            Console.WriteLine(inputFile);
            Console.WriteLine($"Part A: {partAResult}");
            Console.WriteLine($"Part B: {partBResult}");
        }
    }

    static int CalculatePartA(Node head)
    {
        return CalculateConnections(head, true, new HashSet<string> {});
    }

    static int CalculatePartB(Node head)
    {
        return CalculateConnections(head, false, new HashSet<string> {});
    }

    static int CalculateConnections(Node a, bool canVisitSmallCaveAgain, HashSet<string> previousVisited)
    {
        if (a.val == "end")
            return 1;

        HashSet<string> newVisited = new HashSet<string>(previousVisited);
        newVisited.Add(a.val);
 
        int count = 0;

        foreach (Node b in a.largeCaveConnections)
            count += CalculateConnections(b, canVisitSmallCaveAgain, newVisited);
        
        foreach (Node b in a.smallCaveConnections)
        {
            if (!newVisited.Contains(b.val))
                count += CalculateConnections(b, canVisitSmallCaveAgain, newVisited);
            else if (!canVisitSmallCaveAgain)
                count += CalculateConnections(b, true, newVisited);
        }

        return count;
    }

    static Node ParseInput(string[] input)
    {
        Dictionary<string, Node> nodeDict = new Dictionary<string, Node>();

        foreach (string line in input)
        {
            Node a, b;
            string[] splitLine = line.Split("-");
            string aName = splitLine[0];
            string bName = splitLine[1];

            if (nodeDict.ContainsKey(aName))
                a = nodeDict[aName];
            else
                a = nodeDict[aName] = new Node(aName);
            
            if (nodeDict.ContainsKey(bName))
                b = nodeDict[bName];
            else
                b = nodeDict[bName] = new Node(bName);

            a.Connect(b);
            b.Connect(a);
        }

        return nodeDict["start"];
    }

    class Node
    {
        public string val;
        public List<Node> smallCaveConnections;
        public List<Node> largeCaveConnections;

        public Node(string val)
        {
            this.val = val;
            this.smallCaveConnections = new List<Node>();
            this.largeCaveConnections = new List<Node>();
        }

        public void Connect(Node other)
        {
            if (other.val == "start" || this.val == "end")
                return;
            
            if (other.val.Any(char.IsLower))
                smallCaveConnections.Add(other);
            else
                largeCaveConnections.Add(other);
        }
    }
}
