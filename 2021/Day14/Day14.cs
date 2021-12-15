using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;

class Day14
{
    static void Main()
    {
        foreach (string inputFile in System.IO.Directory.EnumerateFiles(".input"))
        {
            string[] input = System.IO.File.ReadAllLines(inputFile);
            Polymer parsedInput = ParseInput(input);

            long partAResult = CalculatePartA(parsedInput);
            long partBResult = CalculatePartB(parsedInput);

            Console.WriteLine(inputFile);
            Console.WriteLine($"Part A: {partAResult}");
            Console.WriteLine($"Part B: {partBResult}");
        }
    }

    static long CalculatePartA(Polymer polymer)
    {
        for (int step = 1; step <= 10; step++)
            polymer.Step();
        
        return polymer.MaxFreqDiff();
    }

    static long CalculatePartB(Polymer polymer)
    {
        // 10 Steps from Part A, 30 steps here = 40 total
        for (int step = 1; step <= 30; step++)
            polymer.Step();
        
        return polymer.MaxFreqDiff();
    }

    static Polymer ParseInput(string[] input)
    {
        // Set initial counts on each insertion
        // It's better to pass the count through the constructor rather than trying to increment/commit
        string polyTemp = input[0];
        ConcurrentDictionary<string, int> insertionCounts = new ConcurrentDictionary<string, int>();
        for (int i = 0; i < polyTemp.Length - 1; i++)
        {
            string poly = polyTemp.Substring(i,2);
            if (insertionCounts.ContainsKey(poly))
                insertionCounts[poly]++;
            else
                insertionCounts[poly] = 1;
        }

        // Add all the insertions to the dictionary
        ConcurrentDictionary<string, PolymerInsertion> polyInsertions = new ConcurrentDictionary<string, PolymerInsertion>();
        foreach (string line in input.Skip(2))
        {
            string[] splitLine = line.Split(" -> ");
            string adjacency = splitLine[0];
            string insertion = splitLine[1];
            int count;
            if (insertionCounts.ContainsKey(adjacency))
                count = insertionCounts[adjacency];
            else
                count = 0;
            polyInsertions[adjacency] = new PolymerInsertion(adjacency, insertion, count);
        }

        Polymer polymer = new Polymer(polyTemp[0], polyInsertions);
        return polymer;
    }

    class Polymer
    {
        /*
        I should have just made a graph of all the PolyInserts so they know their own left/right
        It's unecessary to have this high level dictionary because it requires me to keep left/right public
        I just thought of it a bit late
        Also, the calculation is wrong if there's not full coverage of the pairs
        Example: ABC, AB->A
        Since we only store the pairs that increment that step, we only increment AB
        The pairs AA, BC are thrown out because there are no rules for them
        */
        private char leftMost;
        private ConcurrentDictionary<string, PolymerInsertion> polyInsertions;
        

        public Polymer(char leftMost, ConcurrentDictionary<string, PolymerInsertion> polyInsertions)
        {
            this.leftMost = leftMost;
            this.polyInsertions = polyInsertions;
        }

        public long MaxFreqDiff()
        {
            ConcurrentDictionary<char, long> counts = new ConcurrentDictionary<char, long>();
            foreach (PolymerInsertion polyInsert in polyInsertions.Values)
                counts.AddOrUpdate(polyInsert.rightMost, polyInsert.count, (key, oldValue) => oldValue + polyInsert.count);

            // Add leftmost because we add rightMost of each PolyInsertion above
            counts.AddOrUpdate(leftMost, 1, (key, oldValue) => oldValue + 1);

            long mostCommon = 0;
            long leastCommon = long.MaxValue;
            foreach (long count in counts.Values)
            {
                mostCommon = Math.Max(mostCommon, count);
                leastCommon = Math.Min(leastCommon, count);
            }

            return mostCommon - leastCommon;
        }

        public void Step()
        {
            IncrementStepValues();
            Commit();
        }

        private void IncrementStepValues()
        {
            foreach (KeyValuePair<string, PolymerInsertion> kvp in polyInsertions)
            {
                PolymerInsertion thisInsertion = kvp.Value;
                if (polyInsertions.ContainsKey(thisInsertion.left))
                    polyInsertions[thisInsertion.left].IncrementNextCount(thisInsertion.count);
                if (polyInsertions.ContainsKey(thisInsertion.right))
                    polyInsertions[thisInsertion.right].IncrementNextCount(thisInsertion.count);
            }
        }

        private void Commit()
        {
            foreach (KeyValuePair<string, PolymerInsertion> kvp in polyInsertions)
                kvp.Value.Commit();
        }
    }

    class PolymerInsertion
    {
        public long count;
        public string left;
        public string right;
        public char rightMost;
        private long nextCount;
        private char insertionRule;
        

        public PolymerInsertion(string ogPoly, string insertionRule, long initialCount)
        {
            this.insertionRule = char.Parse(insertionRule);
            left = ogPoly[0] + insertionRule;
            right = insertionRule + ogPoly[1];
            count = initialCount;
            rightMost = ogPoly[1];
        }

        public void IncrementNextCount(long n)
        {
            nextCount += n;
        }

        public void Commit()
        {
            count = nextCount;
            nextCount = 0;
        }
    }
}

