namespace AdventOfCode2022;

public class Day17 : ISolver
{
    public (string, string) ExpectedResult => ("3081", "");

    public (string, string) Solve(string[] input)
    {
        var rockGrid = new RockGrid(input[0]);
        rockGrid.Run(2022);
        var part1 = rockGrid.HighestEmptyRow;
        return ($"{part1}", "0");
    }

    public class RockGrid
    {
        private readonly HashSet<Coord> rockPositions = new HashSet<Coord>();
        public int HighestEmptyRow { get; private set; }
        public string JetPattern { get; }
        public int DroppedRocks { get; private set; }

        private int currentShape;
        private int jetPatternIndex;
        private Coord currentShapeBottomLeft;

        // shape 0: ####
        private static readonly Coord[] Shape0 = new Coord[] { (0, 0), (1, 0), (2, 0), (3, 0) };

        // shape 1: .#.
        //          ###
        //          .#.
        private static readonly Coord[] Shape1 = new Coord[] { (1, 0), (0, 1), (1, 1), (2, 1), (1, 2) };

        // shape 2: ..#
        //          ..#
        //          ###
        private static readonly Coord[] Shape2 = new Coord[] { (0, 0), (1, 0), (2, 0), (2, 1), (2, 2) };

        // shape 3: #
        //          #
        //          #
        //          #
        private static readonly Coord[] Shape3 = new Coord[] { (0, 0), (0, 1), (0, 2), (0, 3) };

        // shape 4: ##
        //          ##
        private static readonly Coord[] Shape4 = new Coord[] { (0, 0), (0, 1), (1, 0), (1, 1) };

        private static readonly Coord[][] Shapes = new Coord[][]
        {
            Shape0,Shape1,Shape2,Shape3,Shape4
        };



        public RockGrid(string jetPattern)
        {
            JetPattern = jetPattern;
            currentShapeBottomLeft = (2, 3);
        }

        private IEnumerable<Coord> GetCurrentShapeCoords()
        {
            return Shapes[currentShape].Select(c => c + currentShapeBottomLeft);
        }
        
        public void Run(int droppedRocks)
        {
            while(DroppedRocks < droppedRocks)
            {
                PushRock();
                DropRock();
            }
        }

        public void PushRock()
        {
            Coord direction = JetPattern[jetPatternIndex] == '>' ? (1, 0) : (-1, 0);
            var testPos = GetCurrentShapeCoords().Select(c => c + direction).ToList();
            if(testPos.Any(c => c.X < 0 || c.X >= 7) || testPos.Any(c => rockPositions.Contains(c)))
            {
                // rock cannot move in this direction - blocked by a wall or another rock
            }
            else
            {
                currentShapeBottomLeft += direction;
            }
            jetPatternIndex++;
            jetPatternIndex %= JetPattern.Length;
        }

        public void DropRock()
        {
            var currentShapePoints = GetCurrentShapeCoords().ToList();
            var testPos = currentShapePoints.Select(c => c + (0,-1)).ToList();
            if (testPos.Any(c => c.Y < 0) || testPos.Any(c => rockPositions.Contains(c)))
            {
                // shape is blocked from falling
                foreach (var p in currentShapePoints) rockPositions.Add(p);
                HighestEmptyRow = Math.Max(HighestEmptyRow, currentShapePoints.Max(p => p.Y + 1));
                DroppedRocks++;
                currentShape++;
                currentShape %= Shapes.Length;
                currentShapeBottomLeft = (2, HighestEmptyRow+3);
            }
            else
            {
                currentShapeBottomLeft += (0, -1);
            }

        }


        public override string ToString()
        {
            var currentShapeCoords = GetCurrentShapeCoords().ToList();
            var highestRow = Math.Max(HighestEmptyRow, currentShapeCoords.Max(c => c.Y));

            var l = new List<string>();
            l.Add("+-------+");
            for(var row = 0; row <= highestRow; row++)
            {
                var rocks = new string(Enumerable.Range(0, 7).Select(x => rockPositions.Contains((x, row)) ? '#' : currentShapeCoords.Contains((x,row)) ? '@' : '.').ToArray());
                l.Add($"|{rocks}|");
            }
            l.Reverse();
            return string.Join("\r\n", l);
        }
    }

}
