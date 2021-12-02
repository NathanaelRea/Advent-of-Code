using System;

class Day1
{
    static void Main()
    {
        foreach (string inputFile in System.IO.Directory.EnumerateFiles(".input"))
        {
            string[] input = System.IO.File.ReadAllLines(inputFile);
            int[] parsedInput = Array.ConvertAll(input, int.Parse);

            double partAResult = NumberOfIncreasingValues(parsedInput, 1);
            double partBResult = NumberOfIncreasingValues(parsedInput, 3);

            Console.WriteLine(inputFile);
            Console.WriteLine($"Part A: {partAResult}");
            Console.WriteLine($"Part B: {partBResult}");
        }
    }

    static double NumberOfIncreasingValues(int[] input, int slidingWindowSize)
    {
        double count = 0;
        for (int i = slidingWindowSize; i < input.Length; i++)
        {
            if (input[i] > input[i - slidingWindowSize])
                count += 1;
        }
        return count;
    }
}