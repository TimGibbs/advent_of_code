// @algorithm Traveling Salesman (Grid-Based)
// @category Graph Search / Combinatorial Optimization
// @input 2D grid with walls (#) and numbered targets (0–N)
// @state Current position + visited target order
// @execution-model
//   - Precompute all-pairs shortest paths between targets (BFS)
//   - Brute-force permutation search over target visit orders
// @technique
//   - BFS on pruned grid graph for distance computation
//   - Permutation generation via in-place swapping
// @features
//   - Dead-end pruning in grid graph
//   - Distance matrix reuse
// @variant
//   Part1: Visit all targets starting from 0
//   Part2: Visit all targets and return to 0
// @data-structures
//   Dictionary<Point, List<Direction>> as graph
//   int[,] distance matrix
//   int[] permutations for visit order
// @complexity
//   Time:
//     - BFS: O(V + E) per target
//     - Permutations: O((N−1)!)
//   Space:
//     - O(V + E + N²)


await Part1();
await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var targets = FindDistances(lines, out var distances);

    var others = Enumerable.Range(1, targets.Length - 1).ToArray();
    var best = int.MaxValue;

    foreach (var perm in Permute(others))
    {
        var sum = 0;
        var cur = 0;

        foreach (var next in perm)
        {
            sum += distances[cur, next];
            cur = next;
        }

        best = Math.Min(best, sum);
    }

    Console.WriteLine(best);
    
    
}

async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var targets = FindDistances(lines, out var distances);

    var others = Enumerable.Range(1, targets.Length - 1).ToArray();
    var best = int.MaxValue;

    foreach (var perm in Permute(others))
    {
        var sum = 0;
        var cur = 0;

        foreach (var next in perm)
        {
            sum += distances[cur, next];
            cur = next;
        }
        sum += distances[cur, 0];
        best = Math.Min(best, sum);
    }

    Console.WriteLine(best);
}

IEnumerable<int[]> Permute(int[] items)
{
    return PermuteInner(items, 0);
}

IEnumerable<int[]> PermuteInner(int[] items, int start)
{
    if (start == items.Length - 1)
    {
        yield return items.ToArray();
        yield break;
    }

    for (int i = start; i < items.Length; i++)
    {
        (items[start], items[i]) = (items[i], items[start]);

        foreach (var p in PermuteInner(items, start + 1))
            yield return p;

        (items[start], items[i]) = (items[i], items[start]);
    }
}

Point[] FindDistances(string[] strings, out int[,] ints)
{
    {
        var xLimit = strings[0].Length-1;
        var yLimit = strings.Length-1;
        var targets1 = new Point[8];

        var dict = new Dictionary<Point, List<Direction>>();
    
        for (int y = 0; y < yLimit; y++)
        {
            for (int x = 0; x < xLimit; x++)
            {
                var c = strings[y][x];
                if(c=='#') continue;
            
                var p = new Point(x, y);
                if (char.IsNumber(c))
                {
                    var n = c - '0';
                    targets1[n] = p;
                }

                var dirs = new List<Direction>();
                if (y != 0 && strings[y - 1][x] != '#')
                {
                    dirs.Add(Direction.U);
                }
                if (y != yLimit && strings[y + 1][x] != '#')
                {
                    dirs.Add(Direction.D);
                }
                if (x != 0 && strings[y][x - 1] != '#')
                {
                    dirs.Add(Direction.L);
                }
                if (x != xLimit && strings[y][x + 1] != '#')
                {
                    dirs.Add(Direction.R);
                }
                dict.Add(p,dirs);

            }
        }

        var loop = true;
        while (loop)
        {
            loop = false;
            var points = dict.Keys;
            foreach (var point in points)
            {
                if (!targets1.Contains(point) && dict[point].Count == 1) //dead end
                {
                    var d = dict[point].Single();
                    var f = Move(point, d);
                    dict[f].Remove(Opposite(d));
                    dict.Remove(point);
                    loop = true;
                }
            }
        }
    
        ints = new int[targets1.Length, targets1.Length];

        for (int i = 0; i < targets1.Length; i++)
        {
            var distMap = BfsFrom(targets1[i], dict);
            for (int j = 0; j < targets1.Length; j++)
            {
                if (i == j) continue;
                ints[i, j] = distMap[targets1[j]];
            }
        }

        return targets1;
    }

    Point Move(Point p, Direction d)
    {
        return d switch
        {
            Direction.U => p with { Y = p.Y - 1 },
            Direction.D => p with { Y = p.Y + 1 },
            Direction.L => p with { X = p.X - 1 },
            Direction.R => p with { X = p.X + 1 },
            _ => throw new ArgumentOutOfRangeException(nameof(d), d, null)
        };
    }

    Direction Opposite(Direction d)
    {
        return d switch
        {
            Direction.U => Direction.D,
            Direction.D => Direction.U,
            Direction.L => Direction.R,
            Direction.R => Direction.L,
            _ => throw new ArgumentOutOfRangeException(nameof(d), d, null)
        };
    }

    Dictionary<Point, int> BfsFrom(
        Point start,
        Dictionary<Point, List<Direction>> graph)
    {
        var q = new Queue<Point>();
        var dist = new Dictionary<Point, int>
        {
            [start] = 0
        };

        q.Enqueue(start);

        while (q.Count > 0)
        {
            var cur = q.Dequeue();
            var d = dist[cur];

            foreach (var dir in graph[cur])
            {
                var next = Move(cur, dir);
                if (!graph.ContainsKey(next)) continue;
                if (dist.ContainsKey(next)) continue;

                dist[next] = d + 1;
                q.Enqueue(next);
            }
        }

        return dist;
    }
}

enum Direction
{
    U,
    D,
    L,
    R,
}

record struct Point(int X, int Y);