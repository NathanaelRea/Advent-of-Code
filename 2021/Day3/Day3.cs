using System;
using System.Collections.Generic;

class Day3
{
    static void Main()
    {
        foreach (string inputFile in System.IO.Directory.EnumerateFiles(".input"))
        {
            string[] input = System.IO.File.ReadAllLines(inputFile);
            node treeHead = ParseInput(input);

            int partAResult = CalculatePartA(treeHead);
            int partBResult = CalculatePartB(treeHead);

            Console.WriteLine(inputFile);
            Console.WriteLine($"Part A: {partAResult}");
            Console.WriteLine($"Part B: {partBResult}");
        }
    }

    static int CalculatePartA(node treeHead)
    {
        // Gamma is most used bits
        List<int> gammaBits = new List<int>();

        int zerosSum = 0, onesSum = 0;
        Queue<node> curNodeQueue = new Queue<node>();
        Queue<node> nextNodeQueue = new Queue<node>();
        nextNodeQueue.Enqueue(treeHead);

        while (true)
        {
            curNodeQueue = nextNodeQueue;
            nextNodeQueue = new Queue<node>();
            while (curNodeQueue.Count != 0)
            {
                zerosSum = 0;
                onesSum = 0;
                node cur = curNodeQueue.Dequeue();
                if (cur.left != null)
                {
                    zerosSum += cur.left.value;
                    nextNodeQueue.Enqueue(cur.left);
                }
                if (cur.right != null)
                {
                    onesSum += cur.right.value;
                    nextNodeQueue.Enqueue(cur.right);
                }
            }

            // no more nodes below this level
            if (nextNodeQueue.Count == 0)
                break;

            if (onesSum >= zerosSum)
                gammaBits.Add(1);
            else
                gammaBits.Add(0);
        }

        // Epsilon is least used bits
        // In part A it's just the inverse of gamma
        List<int> epsilonBits = new List<int>();
        foreach (int i in gammaBits)
            epsilonBits.Add(1 - i);

        return MultiplyBitArrays(gammaBits, epsilonBits);
    }

    static int CalculatePartB(node treeHead)
    {
        node cur;
        
        cur = treeHead;
        List<int> gammaBits = new List<int>();
        while (!(cur.left == null && cur.right == null))
        {
            if (cur.left == null)
            {
                gammaBits.Add(1);
                cur = cur.right;
            }
            else if (cur.right == null)
            {
                gammaBits.Add(0);
                cur = cur.left;
            }
            else if (cur.right.value >= cur.left.value)
            {
                gammaBits.Add(1);
                cur = cur.right;
            }
            else
            {
                gammaBits.Add(0);
                cur = cur.left;
            }
        }

        cur = treeHead;
        List<int> epsilonBits = new List<int>();
        while (!(cur.left == null && cur.right == null))
        {
            if (cur.left == null)
            {
                epsilonBits.Add(1);
                cur = cur.right;
            }
            else if (cur.right == null)
            {
                epsilonBits.Add(0);
                cur = cur.left;
            }
            else if (cur.right.value < cur.left.value)
            {
                epsilonBits.Add(1);
                cur = cur.right;
            }
            else
            {
                epsilonBits.Add(0);
                cur = cur.left;
            }
        }

        return MultiplyBitArrays(gammaBits, epsilonBits);
    }

    static node ParseInput(string[] input)
    {
        node head = new node();

        foreach (string line in input)
        {
            node cur = head;
            char[] splitLine = line.ToCharArray();

            // Add bits to tree
            foreach (char bit in splitLine)
            {
                if (bit == '0')
                {
                    if (cur.left == null)
                        cur.left = new node();
                    cur = cur.left;
                }
                else
                {
                    if (cur.right == null)
                        cur.right = new node();
                    cur = cur.right;
                }
                cur.value += 1;
            }
        }

        return head;
    }

    static int MultiplyBitArrays(List<int> gammaBits, List<int> epsilonBits)
    {
        int gamma = 0, epsilon = 0;
        for (int i = 0; i < gammaBits.Count; i++)
        {
            int exp = (int)Math.Pow(2, gammaBits.Count - i - 1);
            gamma += gammaBits[i] * exp;
            epsilon += epsilonBits[i] * exp;
        }
        return gamma * epsilon;
    }
}

class node
{
    // zero
    public node left = null;

    // one
    public node right = null;

    // number of hits
    public int value = 0;
}