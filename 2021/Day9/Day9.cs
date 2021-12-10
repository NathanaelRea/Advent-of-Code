using System;
using System.Collections.Generic;
using System.Linq;

class Day9
{
    static void Main()
    {
        foreach (string inputFile in System.IO.Directory.EnumerateFiles(".input"))
        {
            string[] input = System.IO.File.ReadAllLines(inputFile);
            int[,] parsedInput = ParseInput(input);

            int partAResult = CalculatePartA(parsedInput);
            int partBResult = CalculatePartB(parsedInput);

            Console.WriteLine(inputFile);
            Console.WriteLine($"Part A: {partAResult}");
            Console.WriteLine($"Part B: {partBResult}");
        }
    }

    static int CalculatePartA(int[,] input)
    {
        // Any 3x3 subArray may have a local min, so we need to examine all values
        int riskSum = 0;
        for (int rowIndex = 0; rowIndex < input.GetLength(0); rowIndex++)
        {
            for (int colIndex = 0; colIndex < input.GetLength(1); colIndex++)
            {
                if (isLocalMin(input, rowIndex, colIndex))
                    riskSum += 1 + input[rowIndex, colIndex];
            }
        }
        return riskSum;
    }

    static bool isLocalMin(int[,] input, int rowIndex, int colIndex)
    {
        int thisVal = input[rowIndex, colIndex];
        if (rowIndex > 0 && thisVal >= input[rowIndex-1, colIndex])
            return false;
        if (rowIndex < input.GetLength(0) - 1 && thisVal >= input[rowIndex+1, colIndex])
            return false;
        if (colIndex > 0 && thisVal >= input[rowIndex, colIndex-1])
            return false;
        if (colIndex < input.GetLength(1) - 1 && thisVal >= input[rowIndex, colIndex+1])
            return false;
        return true;
    }

    static int CalculatePartB(int[,] input)
    {
        // Since 9's are not in basins -> values are either 9 or non-9
        List<int> basinSizes = new List<int>();
        for (int rowIndex = 0; rowIndex < input.GetLength(0); rowIndex++)
        {
            for (int colIndex = 0; colIndex < input.GetLength(1); colIndex++)
            {
                if (input[rowIndex, colIndex] != 9)
                {
                    int size = calculateBasinSize(input, rowIndex, colIndex);
                    basinSizes.Add(size);
                }
            }
        }
        
        if (basinSizes.Count < 3)
            throw new ArgumentException();

        basinSizes = basinSizes.OrderByDescending(s=>s).ToList();
        return basinSizes[0] * basinSizes[1] * basinSizes[2];
    }

    static int calculateBasinSize(int[,] input, int rowIndexStart, int colIndexStart)
    {
        Stack<(int, int)> nodesToCheck = new Stack<(int, int)>();
        nodesToCheck.Push((rowIndexStart, colIndexStart));
        int size = 0;

        while(nodesToCheck.Count > 0)
        {
            (int curRow, int curCol) node = nodesToCheck.Pop();
            // Readability
            int row = node.curRow;
            int col = node.curCol;

            // We only add node to stack if within bounds
            // Check again if == 9 if another node added this
            if (input[row, col] != 9)
            {
                size += 1;
                input[row, col] = 9; 
            }

            if (row > 1 && input[row-1, col] != 9)
                nodesToCheck.Push((row-1, col));
            if (row < input.GetLength(0) - 1 && input[row+1, col] != 9)
                nodesToCheck.Push((row+1, col));
            if (col > 1 && input[row, col-1] != 9)
                nodesToCheck.Push((row, col-1));
            if (col < input.GetLength(1) - 1 && input[row, col+1] != 9)
                nodesToCheck.Push((row, col+1));
        }

        return size;
    }

    static int[,] ParseInput(string[] input)
    {
        int nCol = input[0].ToList().Count;
        int[,] parsedInput = new int[input.Length, nCol];

        for (int i = 0; i < input.Length; i++)
        {
            int[] line = input[i].ToList().Select(c => c - '0').ToArray();
            for (int j = 0; j < nCol; j++)
                parsedInput[i, j] = line[j];
        }

        return parsedInput;
    }
}
