using System;
using System.Collections.Generic;

class Day6
{
    static void Main()
    {
        foreach (string inputFile in System.IO.Directory.EnumerateFiles(".input"))
        {
            string[] input = System.IO.File.ReadAllLines(inputFile);
            int[] parsedInput = ParseInput(input);

            int partAResult = CalculatePartA(parsedInput);
            long partBResult = CalculatePartB(parsedInput);

            Console.WriteLine(inputFile);
            Console.WriteLine($"Part A: {partAResult}");
            Console.WriteLine($"Part B: {partBResult}");
        }
    }

    static int CalculatePartA(int[] input)
    {
        int[] starterFishBuckets = new int[9];
        int[] newFishBuckets = new int[9];
        foreach (int fish in input)
            starterFishBuckets[fish] += 1;

        for (int day = 0; day <= 80; day++)
        {
            int newFish = starterFishBuckets[0] + newFishBuckets[0];
            for (int i = 0; i < 8; i++)
            {
                starterFishBuckets[i] = starterFishBuckets[i + 1];
                newFishBuckets[i] = newFishBuckets[i + 1];
            }
            starterFishBuckets[6] = newFish;
            newFishBuckets[8] = newFish;
        }

        int sum = 0;
        for (int i = 0; i < 8; i++)
        {
            sum += starterFishBuckets[i];
            sum += newFishBuckets[i];
        }
        return sum;
    }

    static long CalculatePartB(int[] input)
    {
        long[] starterFishBuckets = new long[9];
        long[] newFishBuckets = new long[9];
        foreach (int fish in input)
            starterFishBuckets[fish] += 1;

        for (int day = 0; day <= 256; day++)
        {
            long newFish = starterFishBuckets[0] + newFishBuckets[0];
            for (int i = 0; i < 8; i++)
            {
                starterFishBuckets[i] = starterFishBuckets[i + 1];
                newFishBuckets[i] = newFishBuckets[i + 1];
            }
            starterFishBuckets[6] = newFish;
            newFishBuckets[8] = newFish;
        }

        long sum = 0;
        for (int i = 0; i < 8; i++)
        {
            sum += starterFishBuckets[i];
            sum += newFishBuckets[i];
        }
        return sum;
    }

    static int[] ParseInput(string[] input)
    {
        string[] splitLine = input[0].Split(",");
        int[] parsedInput = new int[splitLine.Length];
        for (int i = 0; i < splitLine.Length; i++)
        {
            parsedInput[i] = int.Parse(splitLine[i]);
        }

        return parsedInput;
    }
}
