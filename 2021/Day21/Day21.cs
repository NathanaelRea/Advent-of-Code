using System;
using System.Collections.Generic;

class Day21
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
        int lastRoll = 0;
        int nDiceRolls = 0;
        int posP1 = input[0] - 1;
        int posP2 = input[1] - 1;
        int scoreP1 = 0, scoreP2 = 0;
        while (true)
        {
            RollDeterministic(ref nDiceRolls, ref lastRoll, ref posP1, ref scoreP1);
            if(scoreP1 >= 1000)
                break;
            RollDeterministic(ref nDiceRolls, ref lastRoll, ref posP2, ref scoreP2);
            if(scoreP2 >= 1000)
                break;
        }
        return nDiceRolls * Math.Min(scoreP1, scoreP2);
    }

    static void RollDeterministic(ref int nDiceRolls, ref int lastRoll, ref int pos, ref int score)
    {
        nDiceRolls += 3;
        int move = (lastRoll+1 + lastRoll+2 + lastRoll+3) % 10;
        lastRoll = (lastRoll + 3) % 100;
        pos = (pos + move) % 10;
        score += pos + 1;
    }

    static long CalculatePartB(int[] input)
    {
        Dictionary<(int, int, int, int, int), long> dp;
        int posP1 = input[0] - 1;
        int posP2 = input[1] - 1;
        dp = new Dictionary<(int, int, int, int, int), long>();
        long nP1Wins = PlayQuantum(posP1, posP2, 0, 0, 0, dp);
        dp = new Dictionary<(int, int, int, int, int), long>();
        long nP2Wins = PlayQuantum(posP2, posP1, 0, 0, 3, dp);
        return Math.Max(nP1Wins, nP2Wins);
    }

    static long PlayQuantum(int posP1, int posP2, int scoreP1, int scoreP2, int rollIndex, Dictionary<(int, int, int, int, int), long> dp)
    {
        if (dp.ContainsKey((posP1, posP2, scoreP1, scoreP2, rollIndex)))
            return dp[(posP1, posP2, scoreP1, scoreP2, rollIndex)];
        if (scoreP1 >= 21)
            return 1;
        if (scoreP2 >= 21)
            return 0;
        
        int nextRollIndex = (rollIndex + 1) % 6;
        long thisStateScore = 0;
        if (rollIndex % 6 == 0 || rollIndex % 6 == 1)
        {
            // P1 first and second roll
            thisStateScore += PlayQuantum((posP1 + 1) % 10, posP2, scoreP1, scoreP2, nextRollIndex, dp);
            thisStateScore += PlayQuantum((posP1 + 2) % 10, posP2, scoreP1, scoreP2, nextRollIndex, dp);
            thisStateScore += PlayQuantum((posP1 + 3) % 10, posP2, scoreP1, scoreP2, nextRollIndex, dp);
        }
        else if (rollIndex % 6 == 2)
        {
            // P1 third roll
            thisStateScore += PlayQuantum((posP1 + 1) % 10, posP2, scoreP1 + (posP1 + 1) % 10 + 1, scoreP2, nextRollIndex, dp);
            thisStateScore += PlayQuantum((posP1 + 2) % 10, posP2, scoreP1 + (posP1 + 2) % 10 + 1, scoreP2, nextRollIndex, dp);
            thisStateScore += PlayQuantum((posP1 + 3) % 10, posP2, scoreP1 + (posP1 + 3) % 10 + 1, scoreP2, nextRollIndex, dp);
        }
        else if (rollIndex % 6 == 3 || rollIndex % 6 == 4)
        {
            // P2 first and second roll
            thisStateScore += PlayQuantum(posP1, (posP2 + 1) % 10, scoreP1, scoreP2, nextRollIndex, dp);
            thisStateScore += PlayQuantum(posP1, (posP2 + 2) % 10, scoreP1, scoreP2, nextRollIndex, dp);
            thisStateScore += PlayQuantum(posP1, (posP2 + 3) % 10, scoreP1, scoreP2, nextRollIndex, dp);
        }
        else if (rollIndex % 6 == 5)
        {
            // P2 third roll
            thisStateScore += PlayQuantum(posP1, (posP2 + 1) % 10, scoreP1, scoreP2 + (posP2 + 1) % 10 + 1, nextRollIndex, dp);
            thisStateScore += PlayQuantum(posP1, (posP2 + 2) % 10, scoreP1, scoreP2 + (posP2 + 2) % 10 + 1, nextRollIndex, dp);
            thisStateScore += PlayQuantum(posP1, (posP2 + 3) % 10, scoreP1, scoreP2 + (posP2 + 3) % 10 + 1, nextRollIndex, dp);
        }
        dp[(posP1, posP2, scoreP1, scoreP2, rollIndex)] = thisStateScore;
        return thisStateScore;
    }

    static void RollDirac(ref int nDiceRolls, ref int lastRoll, ref int pos, ref int score)
    {
        nDiceRolls += 3;
        int move = (lastRoll+1 + lastRoll+2 + lastRoll+3) % 10;
        lastRoll = (lastRoll + 3) % 100;
        pos = (pos + move) % 10;
        score += pos + 1;
    }

    static int[] ParseInput(string[] input)
    {
        int[] startPos = new int[2];
        startPos[0] = int.Parse(input[0].Split(" ")[4]);
        startPos[1] = int.Parse(input[1].Split(" ")[4]);
        return startPos;
    }
}