using System;
using System.Collections.Generic;
using System.Linq;

class Day19
{
    static void Main()
    {
        foreach (string inputFile in System.IO.Directory.EnumerateFiles(".input"))
        {
            string[] input = System.IO.File.ReadAllLines(inputFile);
            List<List<Coord>> parsedInput = ParseInput(input);

            int partAResult = CalculatePartA(parsedInput);
            int partBResult = CalculatePartB(parsedInput);

            Console.WriteLine(inputFile);
            Console.WriteLine($"Part A: {partAResult}");
            Console.WriteLine($"Part B: {partBResult}");
        }
    }

    static int CalculatePartA(List<List<Coord>> input)
    {
        List<HashSet<(int, int, int)>> scannerVectorsList = new List<HashSet<(int, int, int)>>();
        foreach(List<Coord> scannerReadings in input)
        {
            HashSet<(int, int, int)> scannerVectors = new HashSet<(int, int, int)>();
            for (int coord1Id = 0; coord1Id < scannerReadings.Count - 2; coord1Id++)
            {
                // Need every combination of possible vector
                for (int coord2Id = coord1Id+1; coord2Id < scannerReadings.Count - 1; coord2Id++)
                {
                    // Could be in any orientation, so +/- is ambiguous
                    int dx = Math.Abs(scannerReadings[coord2Id].x - scannerReadings[coord1Id].x);
                    int dy = Math.Abs(scannerReadings[coord2Id].y - scannerReadings[coord1Id].y);
                    int dz = Math.Abs(scannerReadings[coord2Id].z - scannerReadings[coord1Id].z);
                    // Could be in any rotation, so sort them
                    List<int> sorted = new List<int>() {dx, dy, dz};
                    sorted.Sort();
                    scannerVectors.Add((sorted[0], sorted[1], sorted[2]));
                }
            }
            scannerVectorsList.Add(scannerVectors);
        }

        int nBeacons = 0;
        int nScanners = scannerVectorsList.Count;
        int[,] intersectionMatrix = new int[nScanners, nScanners];
        for(int scanner1Id = 0; scanner1Id < nScanners; scanner1Id++)
        {
            nBeacons += input[scanner1Id].Count;
            for(int scanner2Id = scanner1Id+1; scanner2Id < nScanners; scanner2Id++)
            {
                List<(int, int, int)> scannerIntersections = scannerVectorsList[scanner1Id].Intersect(scannerVectorsList[scanner2Id]).ToList();
                intersectionMatrix[scanner1Id, scanner2Id] = scannerIntersections.Count;
                //nBeacons -= Math.Sqrt(scannerIntersections.Count) * 2;
                Console.Write($"{scannerIntersections.Count}   ");
            }
            Console.WriteLine();
        }

        return nBeacons;
    }

    static int CalculatePartB(List<List<Coord>> input)
    {
        return 0;
    }

    static List<List<Coord>> ParseInput(string[] input)
    {
        List<Coord> scanner = new List<Coord>();
        List<List<Coord>> parsedInput = new List<List<Coord>>();
        foreach(string line in input.Skip(1))
        {
            if (line.StartsWith("---"))
            {
                parsedInput.Add(scanner);
                scanner = new List<Coord>();
            }
            else if (line == "")
            {
                continue;
            }
            else
            {
                string[] splitLine = line.Split(",");
                int x = int.Parse(splitLine[0]);
                int y = int.Parse(splitLine[1]);
                int z = int.Parse(splitLine[2]);
                scanner.Add(new Coord(x, y, z));
            }
        }
        parsedInput.Add(scanner);
        return parsedInput;
    }

    class Coord
    {
        public int x;
        public int y;
        public int z;

        public Coord(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}
