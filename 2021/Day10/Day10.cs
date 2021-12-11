using System;
using System.Collections.Generic;

class Day10
{
    static void Main()
    {
        foreach (string inputFile in System.IO.Directory.EnumerateFiles(".input"))
        {
            string[] input = System.IO.File.ReadAllLines(inputFile);
            List<char[]> parsedInput = ParseInput(input);

            int partAResult = CalculatePartA(parsedInput);
            long partBResult = CalculatePartB(parsedInput);

            Console.WriteLine(inputFile);
            Console.WriteLine($"Part A: {partAResult}");
            Console.WriteLine($"Part B: {partBResult}");
        }
    }

    static int CalculatePartA(List<char[]> input)
    {
        Dictionary<char, int> scoreMap = new Dictionary<char, int>
        {
            {')', 3},
            {']', 57},
            {'}', 1197},
            {'>', 25137},
        };

        int syntaxErrorScore = 0;
        foreach (char[] line in input)
        {
            Stack<char> bracketStack = new Stack<char>();
            foreach (char c in line)
            {
                if (c == '(')
                    bracketStack.Push(')');
                else if (c == '[')
                    bracketStack.Push(']');
                else if (c == '{')
                    bracketStack.Push('}');
                else if (c == '<')
                    bracketStack.Push('>');
                else if (bracketStack.Count == 0 || c != bracketStack.Pop())
                {
                    syntaxErrorScore += scoreMap[c];
                    break;
                }
            }
        }

        return syntaxErrorScore;
    }

    static long CalculatePartB(List<char[]> input)
    {
        Dictionary<char, int> scoreMap = new Dictionary<char, int>
        {
            {')', 1},
            {']', 2},
            {'}', 3},
            {'>', 4},
        };
        
        List<long> syntaxErrorScores = new List<long>();
        foreach (char[] line in input)
        {
            bool illegal = false;
            Stack<char> bracketStack = new Stack<char>();
            foreach (char c in line)
            {
                if (c == '(')
                    bracketStack.Push(')');
                else if (c == '[')
                    bracketStack.Push(']');
                else if (c == '{')
                    bracketStack.Push('}');
                else if (c == '<')
                    bracketStack.Push('>');
                else if (bracketStack.Count == 0 || c != bracketStack.Pop())
                {
                    illegal = true;
                    break;
                }
            }

            if (illegal)
                continue;

            long syntaxErrorScore = 0;
            while (bracketStack.Count > 0)
                syntaxErrorScore = syntaxErrorScore * 5 + scoreMap[bracketStack.Pop()];
            syntaxErrorScores.Add(syntaxErrorScore);
        }

        // Doesn't seem very likely we'd get a sorted list
        // So we don't need to shuffle for worst case guarantee
        return QuickSelect(syntaxErrorScores, syntaxErrorScores.Count / 2);
    }

    static long QuickSelect(List<long> lst, int k)
    {
        int lo = 0, hi = lst.Count - 1;
        while (hi > lo)
        {
            int j = Partition(lst, lo, hi);
            if (j < k)
                lo = j + 1;
            else if (j > k)
                hi = j - 1;
            else
                break;
        }
        return lst[k];
    }

    static int Partition(List<long> lst, int i, int j)
    {
        long pivot = lst[(j + i) / 2];
        while (true)
        {
            while (lst[i] < pivot)
                i++;
            
            while (lst[j] > pivot)
                j--;

            // Indices crossed
            if (i >= j)
                return j;
            
            // Swap
            long tmp = lst[i];
            lst[i] = lst[j];
            lst[j] = tmp;
        }
    }

    static List<char[]> ParseInput(string[] input)
    {
        List<char[]> parsedInput = new List<char[]>();

        foreach (string line in input)
            parsedInput.Add(line.ToCharArray());

        return parsedInput;
    }
}
