using System;
using System.Collections.Generic;
using System.Linq;

class Day2
{
    static void Main()
    {
        foreach (string inputFile in System.IO.Directory.EnumerateFiles(".input"))
        {
            string[] input = System.IO.File.ReadAllLines(inputFile);
            List<(int, int)> parsedInput = ParseInput(input);

            double partAResult = CalculateDistancePartA(parsedInput);
            double partBResult = CalculateDistancePartB(parsedInput);

            Console.WriteLine(inputFile);
            Console.WriteLine($"Part A: {partAResult}");
            Console.WriteLine($"Part B: {partBResult}");
        }
    }

    static double CalculateDistancePartA(List<(int, int)> input)
    {
        double x = 0;
        double y = 0;
        foreach ((int xInc, int yInc) in input)
        {
            x += xInc;
            y += yInc;
        }
        return x * y;
    }

    static double CalculateDistancePartB(List<(int, int)> input)
    {
        double aim = 0;
        double x = 0;
        double y = 0;
        foreach ((int xInc, int aimInc) in input)
        {
            aim += aimInc;
            x += xInc;
            y += aim * xInc;
        }
        return x * y;
    }

    static List<(int, int)> ParseInput(string[] input)
    {
        List<(int, int)>  parsedInput = new List<(int, int)> ();

        foreach (string line in input)
        {
            string[] splitLine = line.Split(" ");

            int x = 0;
            int y = 0;
            switch(splitLine[0])
            {
                case "forward":
                    x = int.Parse(splitLine[1]);
                    break;
                case "down":
                    y += int.Parse(splitLine[1]);
                    break;
                case "up":
                    y -= int.Parse(splitLine[1]);
                    break;
            }

            parsedInput.Add((x, y));
        }

        return parsedInput;
    }
}