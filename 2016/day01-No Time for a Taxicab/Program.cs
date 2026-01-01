// @algorithm GridNavigation / DirectionalSimulation
// @category Geometry / Pathfinding
// @data-structure
//   Tuple (2D coordinates)
//   HashSet<(int,int)> (visited positions)
// @complexity
//   Part1: Time O(n), Space O(1)
//   Part2: Time O(n·k) worst-case (step-by-step movement), Space O(n·k)
// @technique
//   StateMachine (facing direction)
//   Incremental Movement
//   Manhattan Distance
//   Cycle / Revisit Detection
// @variant
//   Part1: FinalPositionDistance
//   Part2: FirstRevisitedLocation


await Part1();
await Part2();
return;

async Task Part1()
{
    var line = await File.ReadAllTextAsync("input.txt");
    var directions = line.Split(',').Select(x => x.TrimStart());
    var position = (0, 0);
    var facing = Facing.North;

    foreach (var direction in directions)
    {
        var d = direction[0];
        var a = int.Parse(direction[1..]);
        facing = Turn(facing, d);
        switch (facing)
        {
            case Facing.North:
                position = (position.Item1, position.Item2 + a);
                break;
            case Facing.East:
                position = (position.Item1 + a, position.Item2 );
                break;
            case Facing.South:
                position = (position.Item1, position.Item2 - a);
                break;
            case Facing.West:
                position = (position.Item1 - a, position.Item2 );
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    Console.WriteLine(position);
    Console.WriteLine(Math.Abs(position.Item1) + Math.Abs(position.Item2));
}

Facing Turn(Facing facing, char direction)
{
    return facing switch
    {
        Facing.North => direction == 'L' ? Facing.West : Facing.East,
        Facing.East => direction == 'L' ? Facing.North : Facing.South,
        Facing.South => direction == 'L' ? Facing.East : Facing.West,
        Facing.West => direction == 'L' ? Facing.South : Facing.North,
        _ => throw new ArgumentOutOfRangeException(nameof(facing), facing, null)
    };
}

async Task Part2()
{
    var line = await File.ReadAllTextAsync("input.txt");
    var directions = line.Split(',').Select(x => x.TrimStart());
    var position = (0, 0);
    var facing = Facing.North;
    var positions = new HashSet<(int, int)>();
    var stop = false;
    foreach (var direction in directions)
    {
        if (stop) break;
        var d = direction[0];
        var a = int.Parse(direction[1..]);
        facing = Turn(facing, d);
        switch (facing)
        {
            case Facing.North:
                for (int i = 0; i < a; i++)
                {
                    position = (position.Item1, position.Item2 + 1);
                    if (!positions.Add(position))
                    {
                        stop = true;
                        break;
                    }
                }
                break;
            case Facing.East:
                for (int i = 0; i < a; i++)
                {
                    position = (position.Item1 +1, position.Item2);
                    if (!positions.Add(position))
                    {
                        stop = true;
                        break;
                    }
                }
                break;
            case Facing.South:
                for (int i = 0; i < a; i++)
                {
                    position = (position.Item1, position.Item2 -1);
                    if (!positions.Add(position))
                    {
                        stop = true;
                        break;
                    }
                }
                break;
            case Facing.West:
                for (int i = 0; i < a; i++)
                {
                    position = (position.Item1 -1, position.Item2);
                    if (!positions.Add(position))
                    {
                        stop = true;
                        break;
                    }
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
    }
    Console.WriteLine(position);
    Console.WriteLine(Math.Abs(position.Item1) + Math.Abs(position.Item2));
}

enum Facing
{
    North,
    East,
    South,
    West,
}