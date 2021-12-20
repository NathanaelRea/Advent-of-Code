using System;

class Day17
{
    static void Main()
    {
        foreach (string inputFile in System.IO.Directory.EnumerateFiles(".input"))
        {
            string[] input = System.IO.File.ReadAllLines(inputFile);
            Rect parsedInput = ParseInput(input);

            int partAResult = CalculatePartA(parsedInput);
            int partBResult = CalculatePartB(parsedInput);

            Console.WriteLine(inputFile);
            Console.WriteLine($"Part A: {partAResult}");
            Console.WriteLine($"Part B: {partBResult}");
        }
    }

    static int CalculatePartA(Rect target)
    {
        // Assume the target is completely below (0,0)
        // eg assume both y's are negative
        int maxHeight = 0;
        for (int testXVel = 0; testXVel <= target.c2.x; testXVel++)
        {
            int maxX = testXVel * (testXVel + 1) / 2; // x + (x-1) + ...
            // not possible to get to target with this shootXVel
            if (maxX < target.c1.x)
                continue;

            // Shoot up
            // what comes up must go down
            // ... with the same velocity
            for (int testYVel = 0; testYVel >= target.c1.y; testYVel--)
            {
                if (ShootUp(maxX, 0, 0, testYVel, target))
                {
                    // shooting up, not down
                    int tmpHeight = -testYVel * (-testYVel + 1) / 2;
                    if (tmpHeight > maxHeight)
                        maxHeight = tmpHeight;
                }
            }
        }
        return maxHeight;
    }

    static int CalculatePartB(Rect target)
    {
        // We only need to check velocities inside c2.y && c1.y because
        // if it's larger than that our first shot will be outside bounds
        // if we shoot up we can just reflect the y vel, so same rule applies +/-
        int nHits = 0;
        for (int testXVel = 0; testXVel <= target.c2.x; testXVel++)
        {
            int maxX = testXVel * (testXVel + 1) / 2; // x + (x-1) + ...
            // not possible to get to target with this shootXVel
            if (maxX < target.c1.x)
                continue;

            // Shoot up
            for (int testYVel = target.c1.y; testYVel <= -target.c1.y; testYVel++)
            {
                if (ShootGeneral(0, 0, testXVel, testYVel, target))
                    nHits++;
            }
        }
        return nHits;
    }

    static bool ShootUp(int x, int y, int xVel, int yVel, Rect target)
    {
        while (y >= target.c1.y)
        {
            yVel -= 1;
            x += xVel;
            y += yVel;
            if (target.IsInside(x, y))
                return true;
        }
        return false;
    }

    static bool ShootGeneral(int x, int y, int xVel, int yVel, Rect target)
    {
        while (y >= target.c1.y)
        {    
            x += xVel;
            y += yVel;
            if (target.IsInside(x, y))
                return true;
            if (xVel > 0)
                xVel -= 1;
            yVel -= 1;
        }
        return false;
    }

    static Rect ParseInput(string[] input)
    {
        //target area: x=20..30, y=-10..-5
        string line = input[0];
        string[] splitLine = line.Split(" ");
        string[] xStr = splitLine[2].Split("..");
        string[] yStr = splitLine[3].Split("..");
        int x1 = int.Parse(xStr[0].Substring(2));
        int x2 = int.Parse(xStr[1].Substring(0, xStr[1].Length - 1));
        int y1 = int.Parse(yStr[0].Substring(2));
        int y2 = int.Parse(yStr[1].Substring(0, yStr[1].Length));
        Coord c1 = new Coord(x1, y1);
        Coord c2 = new Coord(x2, y2);
        return new Rect(c1, c2);
    }

    class Rect
    {
        public Coord c1;
        public Coord c2;

        public Rect(Coord c1, Coord c2)
        {
            this.c1 = c1;
            this.c2 = c2;
        }

        public bool IsInside(int x, int y)
        {
            return x >= c1.x && x <= c2.x && y >= c1.y && y <= c2.y;
        }
    }

    class Coord
    {
        public int x;
        public int y;

        public Coord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
