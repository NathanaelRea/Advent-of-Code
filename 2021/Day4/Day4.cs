using System;
using System.Collections.Generic;

class Day4
{
    static void Main()
    {
        foreach (string inputFile in System.IO.Directory.EnumerateFiles(".input"))
        {
            string[] input = System.IO.File.ReadAllLines(inputFile);
            (Queue<int>, List<Board>) parsedInput = ParseInput(input);

            int partAResult = CalculateWinningBoardPartA(parsedInput);
            int partBResult = CalculateLosingBoardPartB(parsedInput);

            Console.WriteLine(inputFile);
            Console.WriteLine($"Part A: {partAResult}");
            Console.WriteLine($"Part B: {partBResult}");
        }
    }

    static int CalculateWinningBoardPartA((Queue<int> numbersToDraw, List<Board> boards) parsedInput)
    {
        Board winningBoard = null;
        while (parsedInput.numbersToDraw.Count > 0)
        {
            int num = parsedInput.numbersToDraw.Dequeue();

            for (int boardIndex = parsedInput.boards.Count - 1; boardIndex >= 0; boardIndex--)
            {
                Board curBoard = parsedInput.boards[boardIndex];
                curBoard.Blot(num);
                // Don't return immediately so we can use a queue and run PartB right after
                if (curBoard.hasBingo)
                {
                    winningBoard = curBoard;
                    parsedInput.boards.RemoveAt(boardIndex);
                }
            }

            if (winningBoard != null)
                return num * winningBoard.UnmarkedNumbersSum();
        }

        throw new ArgumentException("Did not find bingo");
    }

    static int CalculateLosingBoardPartB((Queue<int>  numbersToDraw, List<Board> boards) parsedInput)
    {
        while (parsedInput.numbersToDraw.Count > 0)
        {
            int num = parsedInput.numbersToDraw.Dequeue();

            for (int boardIndex = parsedInput.boards.Count - 1; boardIndex >= 0; boardIndex--)
            {
                Board curBoard = parsedInput.boards[boardIndex];
                curBoard.Blot(num);

                if (curBoard.hasBingo)
                {
                    if (parsedInput.boards.Count == 1)
                        return num * curBoard.UnmarkedNumbersSum();
                    else
                        parsedInput.boards.RemoveAt(boardIndex);
                }
            }
        }

        throw new ArgumentException("Did not find bingo");
    }

    static (Queue<int> , List<Board>) ParseInput(string[] input)
    {
        string[] numbersToDrawString = input[0].Split(",");
        Queue<int> numbersToDraw = new Queue<int>();
        foreach (string num in numbersToDrawString)
            numbersToDraw.Enqueue(int.Parse(num));

        List<Board> boards = new List<Board>();
        int boardStartRow = 2;
        while (boardStartRow < input.Length)
        {
            Dictionary<int, GridNumber> gridMap = new Dictionary<int, GridNumber>();
            for (int row = 0; row < 5; row++)
            {
                string[] boardRow = input[boardStartRow + row].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                for (int col = 0; col < 5; col++)
                    gridMap[int.Parse(boardRow[col])] = new GridNumber(row, col);
            }
            boards.Add(new Board(gridMap));
            boardStartRow += 6; // Board + space
        }

        return (numbersToDraw, boards);
    }
}

class GridNumber
{
    public bool isBlotted;
    public int col;
    public int row;

    public GridNumber(int col, int row)
    {
        this.col = col;
        this.row = row;
    }
}
class Board
{
    public bool hasBingo;
    private int[] blottedColCounts;
    private int[] blottedRowCounts;
    private Dictionary<int, GridNumber> gridMap;

    public Board(Dictionary<int, GridNumber> gridMap)
    {
        blottedColCounts = new int[] {0, 0, 0, 0, 0};
        blottedRowCounts = new int[] {0, 0, 0, 0, 0};

        this.gridMap = gridMap;
    }

    public void Blot(int number)
    {
        // Assume no duplicates
        if (gridMap.ContainsKey(number))
        {
            gridMap[number].isBlotted = true;
            blottedColCounts[gridMap[number].col] += 1;
            blottedRowCounts[gridMap[number].row] += 1;

            if (blottedColCounts[gridMap[number].col] == 5 ||
                blottedRowCounts[gridMap[number].row] == 5)
                hasBingo = true;
        }
    }

    public int UnmarkedNumbersSum()
    {
        int sum = 0;
        foreach (KeyValuePair<int, GridNumber> kvp in gridMap)
        {
            if (!kvp.Value.isBlotted)
                sum += kvp.Key;
        }
        return sum;
    }
}