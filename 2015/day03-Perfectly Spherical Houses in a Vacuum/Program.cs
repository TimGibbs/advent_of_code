// @algorithm GridTraversal
// @category Simulation / Hashing
// @data-structure HashSet, Tuple
// @complexity Time: O(n), Space: O(n)
// @variant Part1: SingleMover, Part2: TwoMoversAlternating

await Part1();
await Part2();
return;

async Task Part1()
{
    var line = await File.ReadAllTextAsync("input.txt");
    var visited = new HashSet<(int,int)>();
    var pos = (0,0);
    visited.Add(pos);
    foreach (var c in line)
    {
        pos = c switch
        {
            '>' => (pos.Item1 + 1, pos.Item2),
            '<' => (pos.Item1 - 1, pos.Item2),
            '^' => (pos.Item1, pos.Item2 + 1),
            'v' => (pos.Item1, pos.Item2 - 1),
            _ => pos
        };
        visited.Add(pos);
    }
    Console.WriteLine(visited.Count);
}

async Task Part2()
{
    var line = await File.ReadAllTextAsync("input.txt");
    var visited = new HashSet<(int, int)>();
    var pos = new[] { (0, 0), (0,0)};
    visited.Add(pos[0]);
    for (var i = 0; i < line.Length; i++)
    {
        var c = line[i];
        var a = c switch
        {
            '>' => (pos[i % 2].Item1 + 1, pos[i % 2].Item2),
            '<' => (pos[i % 2].Item1 - 1, pos[i % 2].Item2),
            '^' => (pos[i % 2].Item1, pos[i % 2].Item2 + 1),
            'v' => (pos[i % 2].Item1, pos[i % 2].Item2 - 1),
            _ => pos[i % 2]
        };
        visited.Add(a);
        pos[i % 2] = a;
    }

    Console.WriteLine(visited.Count);
}