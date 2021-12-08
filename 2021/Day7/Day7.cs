using System;
using System.Collections.Generic;

class Day7
{
    static void Main()
    {
        foreach (string inputFile in System.IO.Directory.EnumerateFiles(".input"))
        {
            string[] input = System.IO.File.ReadAllLines(inputFile);
            (Dictionary<int, int>, int) parsedInput = ParseInput(input);

            int partAResult = CalculatePartA(parsedInput);
            int partBResult = CalculatePartB(parsedInput);

            Console.WriteLine(inputFile);
            Console.WriteLine($"Part A: {partAResult}");
            Console.WriteLine($"Part B: {partBResult}");
        }
    }

    delegate int CostFunction(Dictionary<int, int> crabsAtLocation, int index);

    static int CalculatePartA((Dictionary<int, int> crabsAtLocation, int maxIndex) input)
    {
        return TernaySearchMin(PartACostFunction, input.crabsAtLocation, 0, input.maxIndex);
    }
    
    static int CalculatePartB((Dictionary<int, int> crabsAtLocation, int maxIndex) input)
    {
        return TernaySearchMin(PartBCostFunction, input.crabsAtLocation, 0, input.maxIndex);
    }

    static int TernaySearchMin(CostFunction costFct, Dictionary<int, int> crabsAtLocation, int left, int right)
    {
        int a = left + (right - left) / 3;
        int b = left + 2 * (right - left) / 3;

        if (b - a <= 1)
            return Math.Min(costFct(crabsAtLocation, a), costFct(crabsAtLocation, b));

        if (costFct(crabsAtLocation, a) < costFct(crabsAtLocation, b))
            return TernaySearchMin(costFct, crabsAtLocation, left, b);
        else
            return TernaySearchMin(costFct, crabsAtLocation, a, right);
    }

    static int PartACostFunction(Dictionary<int, int> crabsAtLocation, int index)
    {
        int cost = 0;
        foreach (KeyValuePair<int, int> kvp in crabsAtLocation)
        {
            int crabIndex = kvp.Key;
            int numCrabs = kvp.Value;
            int crabDeleta = Math.Abs(crabIndex - index);
            cost += numCrabs * crabDeleta;
        }
        return cost;
    }

    static int PartBCostFunction(Dictionary<int, int> crabsAtLocation, int index)
    {
        int cost = 0;
        foreach (KeyValuePair<int, int> kvp in crabsAtLocation)
        {
            int crabIndex = kvp.Key;
            int numCrabs = kvp.Value;
            int crabDeleta = Math.Abs(crabIndex - index);
            cost += numCrabs * crabDeleta * (crabDeleta + 1) / 2;
        }
        return cost;
    }

    static (Dictionary<int, int>, int) ParseInput(string[] input)
    {
        string[] splitLine = input[0].Split(",");
        int[] parsedInput = new int[splitLine.Length];
        for (int i = 0; i < splitLine.Length; i++)
        {
            parsedInput[i] = int.Parse(splitLine[i]);
        }

        int maxIndex = 0;
        Dictionary<int, int> crabsAtLocation = new Dictionary<int, int>();
        for (int i = 0; i < parsedInput.Length; i++)
        {
            maxIndex = Math.Max(maxIndex, parsedInput[i]);
            if (crabsAtLocation.ContainsKey(parsedInput[i]))
                crabsAtLocation[parsedInput[i]] += 1;
            else
                crabsAtLocation[parsedInput[i]] = 1;
        }

        return (crabsAtLocation, maxIndex);
    }
}

