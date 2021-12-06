using System;
using System.Collections.Generic;

class Day5
{
    static void Main()
    {
        foreach (string inputFile in System.IO.Directory.EnumerateFiles(".input"))
        {
            string[] input = System.IO.File.ReadAllLines(inputFile);
            GridBase parsedInput = ParseInput(input);

            int partAResult = parsedInput.CalculateOverlap(true);
            int partBResult = parsedInput.CalculateOverlap(false);

            Console.WriteLine(inputFile);
            Console.WriteLine($"Part A: {partAResult}");
            Console.WriteLine($"Part B: {partBResult}");
        }
    }

    static GridBase ParseInput(string[] input)
    {
        GridBase parsedInput = new GridBase();

        foreach (string line in input)
        {
            string[] splitLine = line.Split(" -> ");
            string[] c1Str = splitLine[0].Split(",");
            string[] c2Str = splitLine[1].Split(",");

            Coord coord1 = new Coord(int.Parse(c1Str[0]), int.Parse(c1Str[1]));
            Coord coord2 = new Coord(int.Parse(c2Str[0]), int.Parse(c2Str[1]));

            parsedInput.AddLine(coord1, coord2);
        }

        return parsedInput;
    }
}

class GridBase
{
    private int sizeX;
    private int sizeY;
    private List<(Coord, Coord)> lines;

    public GridBase()
    {
        sizeX = 0;
        sizeY = 0;
        lines = new List<(Coord, Coord)>();
    }

    public void AddLine(Coord coord1, Coord coord2)
    {
        lines.Add((coord1, coord2));
        sizeX = Math.Max(sizeX, Math.Max(coord1.x, coord2.x));
        sizeY = Math.Max(sizeY, Math.Max(coord1.y, coord2.y));
    }

    public int CalculateOverlap(bool isPartA)
    {
        int[,] grid = new int[sizeX + 1, sizeY + 1];
        int overlapCount = 0;

        foreach ((Coord coord1, Coord coord2) line in lines)
        {
            int xDiff = line.coord2.x.CompareTo(line.coord1.x);
            int yDiff = line.coord2.y.CompareTo(line.coord1.y);
            
            // Part A only Horz/Vert
            if (isPartA && xDiff != 0 && yDiff != 0)
                continue;

            int x = line.coord1.x;
            int y = line.coord1.y;
            while (!(x == line.coord2.x && y == line.coord2.y))
            {
                if (grid[x, y] == 1)
                    overlapCount += 1;
                grid[x, y] += 1;

                x += xDiff;
                y += yDiff;
            }

            // coord 2 inclusive
            if (grid[x, y] == 1)
                overlapCount += 1;
            grid[x, y] += 1;
        }

        return overlapCount;
    }
}

class Coord
{
    public int x;
    public int y;

    public Coord(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}