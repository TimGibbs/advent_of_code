await Part1();
async Task Part1()
{
    var line = await File.ReadAllTextAsync("input.txt");
    var split = line.Split(',');
    var q = 0;
    var r = 0;
    var s = 0;
    var max = 0;
    foreach (var s1 in split)
    {
        switch (s1)
        {
            case "n": s++; r--; break;
            case "s": s--; r++; break;
            case "ne": r--; q++; break;
            case "sw": r++; q--; break;
            case "nw": q--; s++; break;
            case "se": q++; s--; break;
        }

        var m = ((int[])[Math.Abs(q), Math.Abs(r), Math.Abs(s)]).Max();
        max = Math.Max(max, m);
    }
    Console.WriteLine(((int[])[Math.Abs(q), Math.Abs(r), Math.Abs(s)]).Max());
    Console.WriteLine(max);
}
