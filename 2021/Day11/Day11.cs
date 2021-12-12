using System;
using System.Collections.Generic;
using System.Linq;

class Day11
{
    static void Main()
    {
        foreach (string inputFile in System.IO.Directory.EnumerateFiles(".input"))
        {
            string[] input = System.IO.File.ReadAllLines(inputFile);
            Graph parsedInput = ParseInput(input);

            int partAResult = CalculatePartA(parsedInput);
            int partBResult = CalculatePartB(parsedInput);

            Console.WriteLine(inputFile);
            Console.WriteLine($"Part A: {partAResult}");
            Console.WriteLine($"Part B: {partBResult}");
        }
    }

    static int CalculatePartA(Graph graph)
    {
        graph.Init();

        int numFlashes = 0;
        for (int i = 0; i < 100; i++)
            numFlashes += graph.CountFlashes();

        return numFlashes;
    }

    static int CalculatePartB(Graph graph)
    {
        graph.Init();

        int step = 1;
        int nNodes = graph.Size();
        while (graph.CountFlashes() != nNodes)
            step++;

        return step;
    }

    static Graph ParseInput(string[] input)
    {
        int nRows = input.Length;
        int nCols = input[0].ToList().Count;
        int[,] data = new int[nRows, nCols];

        for (int i = 0; i < nRows; i++)
        {
            int[] line = input[i].ToList().Select(c => c - '0').ToArray();
            for (int j = 0; j < nCols; j++)
                data[i, j] = line[j];
        }

        Graph graph = new Graph(data);
        return graph;
    }

    class Graph
    {
        private List<Node> nodes;
        private int[,] initialGridValues;

        public Graph(int[,] initialGridValues)
        {
            this.initialGridValues = initialGridValues;

            int nRows = initialGridValues.GetLength(0);
            int nCols = initialGridValues.GetLength(1);

            // Add Nodes
            nodes = new List<Node>();
            for (int row = 0; row < nRows; row++)
            {
                for (int col = 0; col < nCols; col++)
                    nodes.Add(new Node());
            }

            // Add Connections
            for (int row = 0; row < nRows; row++)
            {
                for (int col = 0; col < nCols; col++)
                {
                    int thisIndex = row * nCols + col;
                    Node thisNode = nodes[thisIndex];
                    TryAddConnection(thisNode, nRows, nCols, row-1, col-1);
                    TryAddConnection(thisNode, nRows, nCols, row-1, col);
                    TryAddConnection(thisNode, nRows, nCols, row-1, col+1);
                    TryAddConnection(thisNode, nRows, nCols, row, col-1);
                    TryAddConnection(thisNode, nRows, nCols, row, col+1);
                    TryAddConnection(thisNode, nRows, nCols, row+1, col-1);
                    TryAddConnection(thisNode, nRows, nCols, row+1, col);
                    TryAddConnection(thisNode, nRows, nCols, row+1, col+1);
                }
            }
        }

        public void Init()
        {
            // Set all nodes to initial value and reset counters
            int nRows = initialGridValues.GetLength(0);
            int nCols = initialGridValues.GetLength(1);

            for (int row = 0; row < nRows; row++)
            {
                for (int col = 0; col < nCols; col++)
                    nodes[row * nRows + col].SetValue(initialGridValues[row, col]);
            }
        }

        private void TryAddConnection(Node node, int maxRow, int maxCol, int row, int col)
        {
            if (row >= 0 && row < maxRow && col >= 0 && col < maxCol)
                node.Connect(nodes[row * maxRow + col]);
        }

        public int CountFlashes()
        {
            // Clear counters from previous, update node vals
            foreach (Node node in nodes)
                node.Commit();
            
            // Tell the node to add 1 to itself and flash if necessary
            foreach (Node node in nodes)
                node.Step();

            // Count the number of nodes that flashed
            int flashes = 0;
            foreach (Node node in nodes)
                if (node.HasFlashed())
                    flashes += 1;
            return flashes;
        }

        public int Size()
        {
            return nodes.Count;
        }
    }

    class Node
    {
        private int val;
        private int counter;
        private List<Node> connections = new List<Node>();

        public void Connect(Node other)
        {
            connections.Add(other);
        }
        
        public void SetValue(int val)
        {
            this.val = val;
            counter = 0;
        }

        public bool HasFlashed()
        {
            return val + counter > 9;
        }

        public void Step()
        {
            counter += 1;
            if (val + counter == 10) // Just flashed
            {
                foreach (Node node in connections)
                    node.Step();
            }
        }

        public void Commit()
        {
            val += counter;
            if (val > 9)
                val = 0;
            counter = 0;
        }
    }
}
