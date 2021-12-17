using System;
using System.Collections.Generic;
using System.Diagnostics;

class Day16
{
    static void Main()
    {
        foreach (string inputFile in System.IO.Directory.EnumerateFiles(".input"))
        {
            string[] input = System.IO.File.ReadAllLines(inputFile);
            Node parsedInput = ParseInput(input);

            long partAResult = CalculatePartA(parsedInput);
            long partBResult = CalculatePartB(parsedInput);

            Console.WriteLine(inputFile);
            Console.WriteLine($"Part A: {partAResult}");
            Console.WriteLine($"Part B: {partBResult}");
        }
    }

    static long CalculatePartA(Node node)
    {
        long sum = node.version;

        if (node is OperatorNode opNode)
            foreach (Node n in opNode.subNodes)
                sum += CalculatePartA(n);
        
        return sum;
    }

    static long CalculatePartB(Node node)
    {
        if (node is LiteralNode litNode)
            return litNode.value;
        
        OperatorNode opNode = node as OperatorNode;
        
        if (opNode.typeId == 0)
        {
            // Sum
            Debug.Assert(opNode.subNodes.Count != 0, $"Sum has 0 nodes!");
            long sum = 0;
            foreach(Node n in opNode.subNodes)
                sum += CalculatePartB(n);
            return sum;
        }
        else if (opNode.typeId == 1)
        {
            // Product
            Debug.Assert(opNode.subNodes.Count != 0, $"Prod has 0 nodes!");
            long prod = 1;
            foreach(Node n in opNode.subNodes)
                prod *= CalculatePartB(n);
            return prod;
        }
        else if (opNode.typeId == 2)
        {
            // Min
            Debug.Assert(opNode.subNodes.Count != 0, $"Min has 0 nodes!");
            long min = long.MaxValue;
            foreach(Node n in opNode.subNodes)
                min = Math.Min(min, CalculatePartB(n));
            return min;
        }
        else if (opNode.typeId == 3)
        {
            // Max
            Debug.Assert(opNode.subNodes.Count != 0, $"Max has 0 nodes!");
            long max = long.MinValue;
            foreach(Node n in opNode.subNodes)
                max = Math.Max(max, CalculatePartB(n));
            return max;
        }
        else if (opNode.typeId == 5)
        {
            // Greater than
            Debug.Assert(opNode.subNodes.Count == 2, $"> has {opNode.subNodes.Count} nodes!");
            long n1 = CalculatePartB(opNode.subNodes[0]);
            long n2 = CalculatePartB(opNode.subNodes[1]);
            if (n1 > n2)
                return 1;
            else
                return 0;
        }
        else if (opNode.typeId == 6)
        {
            // Less than
            Debug.Assert(opNode.subNodes.Count == 2, $"< has {opNode.subNodes.Count} nodes!");
            long n1 = CalculatePartB(opNode.subNodes[0]);
            long n2 = CalculatePartB(opNode.subNodes[1]);
            if (n1 < n2)
                return 1;
            else
                return 0;
        }
        else if (opNode.typeId == 7)
        {
            // Equal to
            Debug.Assert(opNode.subNodes.Count == 2, $"= has {opNode.subNodes.Count} nodes!");
            long n1 = CalculatePartB(opNode.subNodes[0]);
            long n2 = CalculatePartB(opNode.subNodes[1]);
            if (n1 == n2)
                return 1;
            else
                return 0;
        }

        throw new ArgumentException("Something bad");
    }

    static long ConvertBinary(int[] bits, int start, int len)
    {
        long pow = 1;
        long number = 0;
        for (int i = start + len - 1; i >= start; i--)
        {
            number += bits[i] * pow;
            pow *= 2;
        }
        return number;
    }

    static Node ParseInput(string[] input)
    {
        Dictionary<char, int[]> hexMap = new Dictionary<char, int[]>
        {
            {'0', new int[4] {0,0,0,0}},
            {'1', new int[4] {0,0,0,1}},
            {'2', new int[4] {0,0,1,0}},
            {'3', new int[4] {0,0,1,1}},
            {'4', new int[4] {0,1,0,0}},
            {'5', new int[4] {0,1,0,1}},
            {'6', new int[4] {0,1,1,0}},
            {'7', new int[4] {0,1,1,1}},
            {'8', new int[4] {1,0,0,0}},
            {'9', new int[4] {1,0,0,1}},
            {'A', new int[4] {1,0,1,0}},
            {'B', new int[4] {1,0,1,1}},
            {'C', new int[4] {1,1,0,0}},
            {'D', new int[4] {1,1,0,1}},
            {'E', new int[4] {1,1,1,0}},
            {'F', new int[4] {1,1,1,1}},
        };
        char[] charArr = input[0].ToCharArray();
        int[] bits = new int[charArr.Length * 4];
        for(int i = 0; i < charArr.Length; i++)
        {
            bits[i*4 + 0] = hexMap[charArr[i]][0];
            bits[i*4 + 1] = hexMap[charArr[i]][1];
            bits[i*4 + 2] = hexMap[charArr[i]][2];
            bits[i*4 + 3] = hexMap[charArr[i]][3];
        }

        (Node head, int lastIndex) = ParseBits(bits, 0, bits.Length);
        return head;
    }

    static (Node, int) ParseBits(int[] bits, int startIndex, int endIndex)
    {
        if (endIndex - startIndex < 11)
            return (null, startIndex);

        int versionSize = 3;
        int packetTypeSize = 3;

        int lengthTypeSize = 1;
        int operatorSubpacketSizeType0 = 15;
        int operatorSubpacketSizeType1 = 11;
        
        int version = (int)ConvertBinary(bits, startIndex, versionSize);
        int packetTypeId = (int)ConvertBinary(bits, startIndex + versionSize, packetTypeSize);

        if (packetTypeId == 4)
        {
            // Packet is literal value
            (long value, int indexFollowingPacket) = ParseLiteralValue(bits, startIndex + versionSize + packetTypeSize, endIndex);
            LiteralNode litNode = new LiteralNode(version, packetTypeId, value);
            return (litNode, indexFollowingPacket);
        }
        else
        {
            int subPacketStartIndex;
            OperatorNode opNode = new OperatorNode(version, packetTypeId);
            int lengthTypeId = (int)ConvertBinary(bits, startIndex + versionSize + packetTypeSize, lengthTypeSize);
            if (lengthTypeId == 0)
            {
                // Length is a 15-bit number representing number of bits of the subpackets
                int subPacketSize = (int)ConvertBinary(bits, startIndex + versionSize + packetTypeSize + lengthTypeSize, operatorSubpacketSizeType0);
                subPacketStartIndex = startIndex + versionSize + packetTypeSize + lengthTypeSize + operatorSubpacketSizeType0;
                int subPacketEndIndex = subPacketStartIndex + subPacketSize;
                while(true)
                {
                    (Node subNode, int lastIndex) = ParseBits(bits, subPacketStartIndex, subPacketEndIndex);
                    if (subNode == null)
                        break;
                    opNode.AddSubNode(subNode);
                    subPacketStartIndex = lastIndex;
                }
            }
            else
            {
                // Length is an 11-bit number representing the number of subpackets
                int numberOfSubpackets = (int)ConvertBinary(bits, startIndex + versionSize + packetTypeSize + lengthTypeSize, operatorSubpacketSizeType1);
                subPacketStartIndex = startIndex + versionSize + packetTypeSize + lengthTypeSize + operatorSubpacketSizeType1;
                while(numberOfSubpackets > 0)
                {
                    (Node subNode, int lastIndex) = ParseBits(bits, subPacketStartIndex, endIndex);
                    opNode.AddSubNode(subNode);
                    numberOfSubpackets--;
                    subPacketStartIndex = lastIndex;
                }
            }
            return (opNode, subPacketStartIndex);
        }
    }

    static (long, int) ParseLiteralValue(int[] bits, int startIndex, int endIndex)
    {
        int blockStart = startIndex;
        while (bits[blockStart] == 1)
            blockStart += 5;
        int indexFollowing = blockStart + 5;

        long pow = 1;
        long value = 0;
        for (int blockIndex = blockStart; blockIndex >= startIndex; blockIndex -= 5)
        {
            value += ConvertBinary(bits, blockIndex + 1, 4) * pow;
            pow *= 16;
        }
        return (value, indexFollowing);
    }

    class LiteralNode : Node
    {
        public long value { get; private set; }

        public LiteralNode(int version, int typeId, long value)
        {
            this.version = version;
            this.typeId = typeId;
            this.value = value;
        }
    }

    class OperatorNode : Node
    {
        public List<Node> subNodes { get; private set; }

        public OperatorNode(int version, int typeId)
        {
            this.version = version;
            this.typeId = typeId;
            this.subNodes = new List<Node>();
        }
        
        public void AddSubNode(Node subnode)
        {
            subNodes.Add(subnode);
        }
    }

    class Node
    {
        public int version { get; protected set; }
        public int typeId { get; protected set; }
    }
}