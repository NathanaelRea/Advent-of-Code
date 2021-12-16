using System;
using System.Linq;
using System.Collections.Generic;

class Day15
{
    static void Main()
    {
        foreach (string inputFile in System.IO.Directory.EnumerateFiles(".input"))
        {
            string[] input = System.IO.File.ReadAllLines(inputFile);
            (Graph partAGraph, Graph partBGraph) = ParseInput(input);

            Console.WriteLine(inputFile);
            Console.WriteLine($"Part A: {partAGraph.Dijkstra()}");
            Console.WriteLine($"Part B: {partBGraph.Dijkstra()}");
        }
    }

    static (Graph, Graph) ParseInput(string[] input)
    {
        int nOgRows = input.Length;
        int nOgCols = input[0].Length;
        int nRows = 5 * nOgRows;
        int nCols = 5 * nOgCols;

        // Original n x n grid
        int[,] grid = new int[nOgRows, nOgCols];
        for (int i = 0; i < nOgRows; i++)
        {
            char[] vals = input[i].ToCharArray();
            for (int j = 0; j < nOgCols; j++)
                grid[i, j] = vals[j] - '0';
        }

        // Extended 5n x 5n grid
        int[,] extendedGrid = new int[nRows, nCols];
        // Populate the top rows
        for (int row = 0; row < nOgRows; row++)
        {
            for (int col = 0; col < nCols; col++)
            {
                if (col < nOgCols)
                    extendedGrid[row, col] = grid[row, col];
                else
                    extendedGrid[row, col] = 1 + (extendedGrid[row, col - nOgCols] % 9);
            }
        }
        // Populate the rest of the grid
        for (int row = nOgRows; row < nRows; row++)
        {
            for (int col = 0; col < nCols; col++)
                extendedGrid[row, col] = 1 + (extendedGrid[row - nOgRows, col] % 9);
        }

        Graph partAGraph = new Graph(grid);
        Graph partBGraph = new Graph(extendedGrid);
        return (partAGraph, partBGraph);
    }

    class Graph
    {
        private Node source;
        private Node end;
        private List<Node> nodeList;

        public Graph(int[,] grid)
        {
            int nRows = grid.GetLength(0);
            int nCols = grid.GetLength(1);
            Dictionary<(int, int), Node> nodesDict = new Dictionary<(int, int), Node>();

            for (int i = 0; i < nRows; i++)
            {
                for (int j = 0; j < nCols; j++)
                {
                    nodesDict[ (i, j) ] = new Node(grid[i, j]);
                }
            }

            Node thisNode;
            Node otherNode;
            for (int i = 0; i < nRows; i++)
            {
                for (int j = 0; j < nCols; j++)
                {
                    thisNode = nodesDict[ (i, j) ];
                    if (nodesDict.ContainsKey((i - 1, j)))
                    {
                        otherNode = nodesDict[(i - 1, j)];
                        thisNode.Add(otherNode);
                    }
                    if (nodesDict.ContainsKey((i + 1, j)))
                    {
                        otherNode = nodesDict[(i + 1, j)];
                        thisNode.Add(otherNode);
                    }
                    if (nodesDict.ContainsKey((i, j - 1)))
                    {
                        otherNode = nodesDict[(i, j - 1)];
                        thisNode.Add(otherNode);
                    }
                    if (nodesDict.ContainsKey((i, j + 1)))
                    {
                        otherNode = nodesDict[(i, j + 1)];
                        thisNode.Add(otherNode);
                    }
                }
            }

            nodeList = nodesDict.Values.ToList();
            source = nodesDict[ (0, 0) ];
            end = nodesDict[ (nRows - 1, nCols - 1) ];
        }

        public int Dijkstra()
        {
            // Yea. this kinda sucks
            // My first implementation of Dijkstra (using just the wiki article)
            // But it works!
            // Putting all the nodes in Q had terrible performance, so I needed to modify it a bit
            HashSet<Node> Q = new HashSet<Node>();
            Q.Add(source);
            HashSet<Node> visitedNodes = new HashSet<Node>();

            Dictionary<Node, int> dist = new Dictionary<Node, int>();
            foreach (Node node in nodeList)
                dist[node] = int.MaxValue;
            dist[source] = 0;

            while (Q.Count > 0)
            {
                Node minNode = null;
                int minDist = int.MaxValue;
                foreach (Node n in Q)
                {
                    if (dist[n] < minDist)
                    {
                        minNode = n;
                        minDist = dist[n];
                    }
                }
                Node u = minNode;
                Q.Remove(u);
                visitedNodes.Add(u);

                foreach (Node v in u.neighbors)
                {
                    if (visitedNodes.Contains(v))
                        continue;

                    int alt = dist[u] + v.risk;
                    if (alt < dist[v])
                    {
                        dist[v] = alt;
                        Q.Add(v);
                    }
                }
            }

            return dist[end];
        }
    }

    class Node
    {
        public int risk;
        public List<Node> neighbors;

        public Node(int risk)
        {
            this.risk = risk;
            this.neighbors = new List<Node>();
        }

        public void Add(Node other)
        {
            neighbors.Add(other);
        }
    }
}