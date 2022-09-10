using System;
using System.Collections.Generic;
using System.Linq;

class Day18
{
    static void Main()
    {
        foreach (string inputFile in System.IO.Directory.EnumerateFiles(".input"))
        {
            string[] input = System.IO.File.ReadAllLines(inputFile);
            List<Node> parsedInput = ParseInput(input);

            int partAResult = CalculatePartA(parsedInput);
            int partBResult = CalculatePartB(parsedInput);

            Console.WriteLine(inputFile);
            Console.WriteLine($"Part A: {partAResult}");
            Console.WriteLine($"Part B: {partBResult}");
        }
    }

    static int CalculatePartA(List<Node> input)
    {
        Node cur = input[0];
        foreach(Node sfNum in input.Skip(1))
        {

            Console.WriteLine("New:");
            //Print(cur);
            Console.WriteLine("===");

            bool somethingHappened;
            do
            {
                somethingHappened = false;
                somethingHappened |= Explode(cur, 1);
                //Print(cur);
                somethingHappened |= Split(cur);
                //Print(cur);
            } while (somethingHappened);
        }
        return 0;
    }

    static void Print(Node sfNumber)
    {
        Console.WriteLine();
    }

    static bool Explode(Node sfNumber, int height)
    {
        bool somethingHappened = false;

        if (height == 3 && sfNumber.left != null && sfNumber.right != null)
        {
            somethingHappened = true;
            sfNumber.Parent.Snip();
        }

        return somethingHappened;
    }

    static bool Split(Node sfNumber)
    {
        bool somethingHappened = false;


        return somethingHappened;
    }

    static int CalculatePartB(List<Node> input)
    {
        return 0;
    }

    static List<Node> ParseInput(string[] input)
    {
        List<Node> parsedInput = new List<Node>();
        foreach(string line in input)
        {
            Node head = new Node();
            ParseNode(head, line, 0, line.Length);
            parsedInput.Add(head);
        }
        return parsedInput;
    }

    static void ParseNode(Node node, string input, int start, int end)
    {
        int bracketCount = 0;
        for (int i = start + 1; i < end - 1; i++)
        {
            if (input[i] == '[')
                bracketCount++;
            else if (input[i] == ']')
                bracketCount--;
            else if (bracketCount == 0 && input[i] == ',')
            {
                Node left = new Node();
                Node right = new Node();

                if (input[start+1] != '[' && input[i-1] != ']')
                    left.val = ParseNumber(input, start+1, i-1);
                else
                    ParseNode(left, input, start+1, i-1);

                if (input[i+1] != '[' && input[end-1] != ']')
                    right.val = ParseNumber(input, i+1, end-1);
                else
                    ParseNode(right, input, i+1, end-1);

                
                node.left = left;
                node.right = right;
            }
        }
    }

    static int ParseNumber(string input, int start, int end)
    {
        int n = 0;
        int pow = 1;
        for (int i = end; i >= start; i--)
        {
            n += (input[i] - '0') * pow;
            pow *= 10;
        }
        return n;
    }

    class Node
    {
        public int val;
        public Node left;
        public Node right;

        public Node()
        {
            //val = -1;
            left = null;
            right = null;
        }
    }
}

