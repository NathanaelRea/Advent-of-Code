using System;
using System.Collections.Generic;
using System.Linq;

class Day8
{
    static void Main()
    {
        foreach (string inputFile in System.IO.Directory.EnumerateFiles(".input"))
        {
            string[] input = System.IO.File.ReadAllLines(inputFile);
            List<(string[], string[])> parsedInput = ParseInput(input);

            int partAResult = CalculatePartA(parsedInput);
            int partBResult = CalculatePartB(parsedInput);

            Console.WriteLine(inputFile);
            Console.WriteLine($"Part A: {partAResult}");
            Console.WriteLine($"Part B: {partBResult}");
        }
    }

    static int CalculatePartA(List<(string[], string[])> input)
    {
        int count = 0;
        foreach ((string[] tenSignals, string[] outputSignal) in input)
        {
            foreach(string outputDigit in outputSignal)
            {
                int numSeg = outputDigit.Length;
                if (numSeg == 2 || numSeg == 3 || numSeg == 4 || numSeg == 7)
                    count += 1;
            }
        }
        return count;
    }

    static int CalculatePartB(List<(string[], string[])> input)
    {
        int outputSignalSum = 0;
        foreach ((string[] tenDistinctSignals, string[] outputSignal) in input)
        {
            Dictionary<int, string> easyDigitsMap = new Dictionary<int, string>();
            Dictionary<string, int> segmentMap = new Dictionary<string, int>();
            List<string> fiveSegDigits = new List<string>();
            List<string> sixSegDigits = new List<string>();
            foreach(string digit in tenDistinctSignals)
            {
                int numSeg = digit.Length;
                string sortedDigit = string.Concat(digit.OrderBy(s=>s));
                switch(numSeg)
                {
                    case 2:
                        segmentMap[sortedDigit] = 1;
                        easyDigitsMap[1] = sortedDigit;
                        break;
                    case 3:
                        segmentMap[sortedDigit] = 7;
                        easyDigitsMap[7] = sortedDigit;
                        break;
                    case 4:
                        segmentMap[sortedDigit] = 4;
                        easyDigitsMap[4] = sortedDigit;
                        break;
                    case 5:
                        fiveSegDigits.Add(sortedDigit);
                        break;
                    case 6:
                        sixSegDigits.Add(sortedDigit);
                        break;
                    case 7:
                        segmentMap[sortedDigit] = 8;
                        easyDigitsMap[8] = sortedDigit;
                        break;
                }
            }

            // Calculate individual segments and non-easy digits
            // The vars ABFG, BD, EG, D reference the nominal segments
            //  AA
            // B  C
            //  DD
            // E  F
            //  GG
            string ABFG, BD, EG, D;
            string zero, two, three, five, six, nine;

            ABFG = StrXor(sixSegDigits[0], StrXor(sixSegDigits[1], sixSegDigits[2])); // 6 ^ 9 ^ 0
            BD = StrXor(easyDigitsMap[1], easyDigitsMap[4]);
            five = StrOr(ABFG, BD);

            D = StrXor(five, ABFG);
            zero = StrXor(easyDigitsMap[8], D);

            EG = StrXor(BD, StrXor(easyDigitsMap[7], easyDigitsMap[8]));
            six = StrOr(five, EG);

            sixSegDigits.Remove(six); // Now only 9 and 0
            sixSegDigits.Remove(zero); // Now only 9
            nine = sixSegDigits[0];

            fiveSegDigits.Remove(five); // Now only 2 and 3
            three = StrOr(easyDigitsMap[1], StrAnd(fiveSegDigits[0], fiveSegDigits[1]));
            fiveSegDigits.Remove(three); // Now only 2
            two = fiveSegDigits[0];

            // Add components to map
            segmentMap[zero] = 0;
            segmentMap[two] = 2;
            segmentMap[three] = 3;
            segmentMap[five] = 5;
            segmentMap[six] = 6;
            segmentMap[nine] = 9;

            // Calculate output signal
            int output = 0;
            for (int i = 0; i < 4; i++)
            {
                string sortedDigit = string.Concat(outputSignal[i].OrderBy(s=>s));
                output += segmentMap[sortedDigit] * (int)Math.Pow(10, 3 - i);
            }
            outputSignalSum += output;
        }

        return outputSignalSum;
    }

    static string StrXor(string a, string b)
    {
        // a = "ab"
        // b = "bc"
        // a ^ b = "ac"
        string stringXor = string.Empty;
        foreach (char letter in a + b)
            if (a.Contains(letter) ^ b.Contains(letter))
                stringXor += letter;
        return string.Concat(stringXor.OrderBy(s=>s));
    }

    static string StrOr(string a, string b)
    {
        // a = "ab"
        // b = "bc"
        // a | b = "abc"
        string stringOr = string.Empty;
        foreach (char letter in a + b)
            if (a.Contains(letter) || b.Contains(letter))
                stringOr += letter;
        return string.Concat(stringOr.Distinct().OrderBy(s=>s));
    }

    static string StrAnd(string a, string b)
    {
        // a = "ab"
        // b = "bc"
        // a & b = "b"
        string stringAnd = string.Empty;
        foreach (char letter in a)
            if (b.Contains(letter))
                stringAnd += letter;
        return string.Concat(stringAnd.OrderBy(s=>s));
    }

    static List<(string[], string[])> ParseInput(string[] input)
    {
        List<(string[], string[])> parsedInput = new List<(string[], string[])>();
        foreach (string line in input)
        {
            string[] splitLine = line.Split(" | ");
            string[] digits = splitLine[0].Split(" ");
            string[] signals = splitLine[1].Split(" ");
            parsedInput.Add((digits, signals));
        }
        return parsedInput;
    }
}