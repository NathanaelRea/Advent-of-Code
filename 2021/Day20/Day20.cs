using System;


class Day20
{
    static void Main()
    {
        foreach (string inputFile in System.IO.Directory.EnumerateFiles(".input"))
        {
            string[] input = System.IO.File.ReadAllLines(inputFile);
            (int[], int[,]) parsedInput = ParseInput(input);

            int partAResult = CalculatePartAB(parsedInput, 2);
            int partBResult = CalculatePartAB(parsedInput, 50);

            Console.WriteLine(inputFile);
            Console.WriteLine($"Part A: {partAResult}");
            Console.WriteLine($"Part B: {partBResult}");
        }
    }

    static int CalculatePartAB((int[] enhanceAlg, int[,] image) input, int nSteps)
    {
        // "Infinite"
        // lol this whole thing is garbage
        int extraPixels = nSteps*2; // cut off first/last row/col each setep
        int nRow = input.image.GetLength(0);
        int nCol = input.image.GetLength(1);
        int[,] image = new int[nRow + extraPixels*2, nCol + extraPixels*2];
        for(int row = 0; row < nRow; row++)
        {
            for(int col = 0; col < nCol; col++)
            {
                image[row+extraPixels, col+extraPixels] = input.image[row, col];
            }
        }

        for(int step = 0; step < nSteps; step ++)
        {
            nRow = image.GetLength(0);
            nCol = image.GetLength(1);
            int[,] newImage = new int[nRow-2, nCol-2];
            for(int row = 0; row < nRow-2; row++)
            {
                for(int col = 0; col < nCol-2; col++)
                {
                    newImage[row, col] = Enhance(image, input.enhanceAlg, row, col);
                }
            }
            image = newImage;
        }

        int count = 0;
        for(int i = 0; i < image.GetLength(0); i++)
        {
            for(int j = 0; j < image.GetLength(1); j++)
            {
                count += image[i,j];
            }
        }

        return count;
    }

    static int Enhance(int[,] image, int[] enhancement, int row, int col)
    {
        int val = 0;
        int pow = 8;
        for(int i = row; i < row + 3; i++)
        {
            for(int j = col; j < col + 3; j++)
            {
                val += image[i,j] * (int)Math.Pow(2, pow);
                pow--;
            }
        }
        return enhancement[val];
    }

    static (int[], int[,]) ParseInput(string[] input)
    {
        string enhanceStr = input[0];
        int[] enhanceAlg = new int[enhanceStr.Length];
        for(int i = 0; i < enhanceStr.Length; i++)
            if(enhanceStr[i] == '#')
                enhanceAlg[i] = 1;

        int nRow = input.Length - 2;
        int nCol = input[2].Length;
        int[,] image = new int[nRow, nCol];

        for(int i = 0; i < nRow; i++)
        {
            for(int j = 0; j < nCol; j++)
            {
                if (input[i+2][j] == '#')
                    image[i,j] = 1;
            }
        }

        return (enhanceAlg, image);
    }
}
