using System;
using System.Collections.Generic;
using System.Numerics;

class Day22
{
    static void Main()
    {
        foreach (string inputFile in System.IO.Directory.EnumerateFiles(".input"))
        {
            string[] input = System.IO.File.ReadAllLines(inputFile);
            Cuboid parsedInput = ParseInput(input);

            int partAResult = CalculatePartA(parsedInput);
            int partBResult = CalculatePartB(parsedInput);

            Console.WriteLine(inputFile);
            Console.WriteLine($"Part A: {partAResult}");
            Console.WriteLine($"Part B: {partBResult}");
        }
    }

    static int CalculatePartA(Cuboid input)
    {
        return input.CountBounded();
    }

    static int CalculatePartB(Cuboid input)
    {
        return input.CountUnbounded();
    }

    static Cuboid ParseInput(string[] input)
    {
        Cuboid cuboid = new Cuboid();

        for(int lineIndex = 0; lineIndex < input.Length; lineIndex++)
        {
            bool on = input[lineIndex].StartsWith("on");
            
            string[] splitLine;
            if (on)
                splitLine = input[lineIndex].Substring(3).Split(",");
            else
                splitLine = input[lineIndex].Substring(4).Split(",");

            string[] xStr = splitLine[0].Split("..");
            int x1 = int.Parse(xStr[0].Substring(2));
            int x2 = int.Parse(xStr[1]);
            string[] yStr = splitLine[1].Split("..");
            int y1 = int.Parse(yStr[0].Substring(2));
            int y2 = int.Parse(yStr[1]);
            string[] zStr = splitLine[2].Split("..");
            int z1 = int.Parse(zStr[0].Substring(2));
            int z2 = int.Parse(zStr[1]);
            AABB aabb = new AABB(x1, x2, y1, y2, z1, z2);
        }

        return cuboid;
    }

    class BVH
    {
        AABB head;
        List<AABB> nodes;

        public void InsertLeaf(AABB box)
        {
            if (head == null)
            {
                head = box;
                return;
            }

            // Find best sibling
            AABB bestSibling = head;
            foreach(AABB node in nodes)
            {
                bestSibling = PickBest(bestSibling, node);
            }

            // Create new parent
            AABB oldParent = bestSibling.Parent;
            AABB newParent;
            box = Union(box, bestSibling);
            if (oldParent != null)
            {
                // sibling was not root
                if (oldParent.leftChild == sibling)
                    oldParent.leftChild = newParent;
                else
                    oldParent.rightChild = newParent;
            }
            else
                head = newParent;

            newParent.leftChild = bestSibling;
            newParent.rightChild = leafIndex?;
            bestSibling.Parent = newParent;
            box.Parent = newParent;            


            // Walk back up

        }

        public AABB Union(AABB a, AABB b)
        {
            Vector3 lower = Vector3.Min(a.Lower, b.Lower);
            Vector3 upper = Vector3.Max(a.Upper, b.Upper);
            return new AABB(lower, upper);
        }
    }

    class AABB
    {
        private Vector3 lower;
        private Vector3 upper;

        public Vector3 Lower
        {
            get {return lower; }
        }

        public Vector3 Upper
        {
            get {return upper; }
        }

        public AABB(int x1, int x2, int y1, int y2, int z1, int z2) : this(new Vector3(x1, y1, z1), new Vector3(x2, y2, z2))
        {
        }

        public AABB(Vector3 v1, Vector3 v2)
        {
            lower = Vector3.Min(v1, v2);
            upper = Vector3.Max(v1, v2);
        }

        public double SurfaceArea()
        {
            Vector3 d = upper - lower;
            return 2.0 * (d.X * d.Y + d.Y * d.Z + d.Z * d.X);
        }

        public double Volume()
        {
            Vector3 d = upper - lower;
            return d.X * d.Y * d.Z;
        }
    }
}
