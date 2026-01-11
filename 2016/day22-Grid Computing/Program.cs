// @algorithm Grid Analysis + BFS State Movement
// @category Graph Traversal / Grid Simulation
// @input Node list describing a storage grid (x,y,size,used,avail)
// @state
//   Part1: Independent node pairs (A, B)
//   Part2: Grid with movable empty node and fixed walls
// @technique
//   Part1: Brute-force pair comparison
//   Part2: Breadth-First Search + problem-specific move counting
// @features
//   - Parsing structured text input
//   - Viable-pair detection
//   - BFS shortest-path on grid
//   - Problem reduction using observed movement pattern
// @data-structures
//   IEnumerable / arrays for nodes
//   Queue for BFS
//   HashSet for visited tracking
//   Record for node representation
// @complexity
//   Part1:
//     Time: O(N²)
//     Space: O(1)
//   Part2:
//     Time: O(GridSize)
//     Space: O(GridSize)


await Part1();
await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var nodes = lines.Skip(2).Select(x =>
    {
        var s = x.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        var l = s[0].Split('-');
        return new Line(int.Parse(l[1][1..]),int.Parse(l[2][1..]),int.Parse(s[1][..^1]), int.Parse(s[2][..^1]), int.Parse(s[3][..^1]));
    });

    var count = 0;
    foreach (var A in nodes)
    {
        if(A.Used <=0) continue;
        foreach (var B in nodes)
        {
            if(A==B) continue;
            if (A.Used < B.Avail) count++;
        }
    }
    Console.WriteLine(count);
}

async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var nodes = lines.Skip(2).Select(x =>
    {
        var s = x.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        var l = s[0].Split('-');
        return new Line(int.Parse(l[1][1..]),int.Parse(l[2][1..]),int.Parse(s[1][..^1]), int.Parse(s[2][..^1]), int.Parse(s[3][..^1]));
    }).ToArray();

    int maxX = nodes.Max(n => n.X);
    int maxY = nodes.Max(n => n.Y);
    
    var empty = nodes.First(n => n.Used == 0);
    
    var goal = nodes.Where(n => n.Y == 0).MaxBy(n => n.X)!;
    
    bool IsWall(int x, int y)
        => nodes.Any(n => n.X == x && n.Y == y && n.Used > 100);
    
    var target = (X: goal.X - 1, Y: goal.Y);

    var queue = new Queue<(int X, int Y, int Steps)>();
    var visited = new HashSet<(int, int)>();

    queue.Enqueue((empty.X, empty.Y, 0));
    visited.Add((empty.X, empty.Y));

    int bfsDistance = -1;

    while (queue.Count > 0)
    {
        var (x, y, steps) = queue.Dequeue();

        if ((x, y) == target)
        {
            bfsDistance = steps;
            break;
        }

        foreach (var (nx, ny) in new[]
                 {
                     (x - 1, y),
                     (x + 1, y),
                     (x, y - 1),
                     (x, y + 1)
                 })
        {
            if (nx < 0 || ny < 0 || nx > maxX || ny > maxY)
                continue;

            if (visited.Contains((nx, ny)))
                continue;

            if (IsWall(nx, ny))
                continue;

            visited.Add((nx, ny));
            queue.Enqueue((nx, ny, steps + 1));
        }
    }

    if (bfsDistance < 0)
        throw new Exception("No path found for empty node");
    
    int totalMoves =
        bfsDistance
        + 1 
        + 5 * (goal.X - 1);
    Console.WriteLine(totalMoves);
}
record Line(int X, int Y, int Size, int Used, int Avail);