
await Part1();
await Part2();
return;

async Task Part1()
{
    var line = await File.ReadAllTextAsync("input.txt");
    var bools = line.Select(x => x == '^').ToArray();
    var l = new List<bool[]> { bools };
    for (int i = 0; i < 39; i++)
    {
        var b = l.Last();
        var n = new bool[b.Length];
        for (int j = 0; j < b.Length; j++)
        {
            if (j == 0) {n[j] = IsTrap(false, b[j], b[j + 1]); continue;}
            if (j == b.Length-1) {n[j] = IsTrap(b[j-1], b[j], false); continue;}
            n[j] = IsTrap(b[j-1], b[j], b[j+1]);
        }
        l.Add(n);
    }

    Console.WriteLine(l.Count);
    var result = l.SelectMany(x => x).Count(x => !x);
    Console.WriteLine(result);

}

bool IsTrap(bool left, bool center, bool right)
{
    if (left && center && !right) return true;
    if (!left && center && right) return true;
    if (left && !center && !right) return true;
    if (!left && !center && right) return true;
    return false;
}


async Task Part2()
{
    var line = await File.ReadAllTextAsync("input.txt");
    var bools = line.Select(x => x == '^').ToArray();
    var l = new List<bool[]> { bools };
    for (int i = 0; i < 400000-1; i++)
    {
        var b = l.Last();
        var n = new bool[b.Length];
        for (int j = 0; j < b.Length; j++)
        {
            if (j == 0) {n[j] = IsTrap(false, b[j], b[j + 1]); continue;}
            if (j == b.Length-1) {n[j] = IsTrap(b[j-1], b[j], false); continue;}
            n[j] = IsTrap(b[j-1], b[j], b[j+1]);
        }
        l.Add(n);
    }

    Console.WriteLine(l.Count);
    var result = l.SelectMany(x => x).Count(x => !x);
    Console.WriteLine(result);

}
