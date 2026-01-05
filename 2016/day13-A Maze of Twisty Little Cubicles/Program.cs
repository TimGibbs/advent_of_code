// @algorithm Graph Search (A* and BFS)
// @category Pathfinding / Grid Navigation
// @input Single integer seed (favorite number)
// @grid Implicit infinite grid with computed walls
// @state Current position (x, y)
// @movement 4-directional (up, down, left, right)
// @walkability Determined by parity of bit-count formula
// @technique
//   Part1: A* search using Manhattan distance heuristic
//   Part2: Breadth-first search with depth limit
// @features
//   Implicit graph generation
//   Deterministic wall computation via bit population count
// @variant
//   Part1: Shortest path from start to fixed target
//   Part2: Count of reachable locations within 50 steps
// @data-structures
//   PriorityQueue for A*
//   Queue for BFS
//   Dictionary / HashSet for visited tracking
//   Record for Point
// @heuristic Manhattan distance
// @complexity
//   Time: O(V + E) within explored region
//   Space: O(V)

using System.Numerics;

await Part1();
await Part2();
return;

async Task Part1()
{
    var line = await File.ReadAllTextAsync("input.txt");
    var shift = uint.Parse(line);
    var start = new Point(1, 1);
    var target = new Point(31, 39);
    var q = new PriorityQueue<Point, uint>();
    q.Enqueue(start, Dist(start, target));
    var visited = new Dictionary<Point, uint>();
    visited[start] = 0;
    while (q.TryDequeue(out var p, out var _))
    {
        var d = visited[p];
        var spaces = NextSteps(p, shift)
            .Where(x=> !visited.TryGetValue(x, out var val) || val > d + 1 );
        foreach (var space in spaces)
        {
            if (space == target)
            {
                Console.WriteLine(visited[p] + 1);
                return;
            }
            visited[space] = d + 1;
            q.Enqueue(space, Dist(space, target));
        }
    }
}

async Task Part2()
{
    var line = await File.ReadAllTextAsync("input.txt");
    var shift = uint.Parse(line);
    var start = new Point(1, 1);

    var visited = new HashSet<Point>();
    var q = new Queue<(Point,uint)>();
    q.Enqueue((start,0));
    while (q.TryDequeue(out var item))
    {
        var p = item.Item1;
        var i = item.Item2;
        visited.Add(p);
        if(i == 50) continue;
        foreach (var next in NextSteps(p, shift))
        {
            if (!visited.Contains(next)) q.Enqueue((next, i+1));
        }
    }
    Console.WriteLine( visited.Count);
}

uint Dist(Point a, Point b) => (a.X > b.X ? a.X-b.X : b.X - a.X) + (a.Y > b.Y ? a.Y-b.Y : b.Y - a.Y);

bool IsSpace(Point point, uint shift)
{
    var r = point.X * point.X 
            + 3 * point.X 
            + 2 * point.X * point.Y 
            + point.Y 
            + point.Y * point.Y;
    var s = r + shift;
    var t = BitOperations.PopCount(s);
    return t % 2 == 0;
}

IEnumerable<Point> NextSteps(Point point, uint shift)
{
    var d = new List<Point>();
    if (point.X != 0)
    {
        d.Add(point with { X = point.X - 1 });
    }

    if (point.Y != 0)
    {
        d.Add(point with { Y = point.Y - 1 });
    }
    d.Add(point with { X = point.X + 1 });
    d.Add(point with { Y = point.Y + 1 });
    foreach (var p in d.Where(p => IsSpace(p, shift)))
    {
        yield return p;
    }
}

record Point(uint X, uint Y);