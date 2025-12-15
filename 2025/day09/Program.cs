// See https://aka.ms/new-console-template for more information

await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var redTiles = lines
        .Select(s => s.Split(',').Select(long.Parse).ToArray())
        .Select(s => new Point(s[0], s[1]))
        .ToArray();

    long max = 0;
    for (var i = 0; i < redTiles.Length; i++)
    {
        for (var j = i + 1; j < redTiles.Length; j++)
        {
            max = Math.Max(max, RectSize(redTiles[i], redTiles[j]));
        }
    }

    Console.WriteLine(max);
}

async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var redTiles = lines
        .Select(s => s.Split(',').Select(long.Parse).ToArray())
        .Select(a => new Point(a[0], a[1]))
        .ToArray();

    var n = redTiles.Length;
    
    var byX = new Dictionary<long, List<long>>();
    var byY = new Dictionary<long, List<long>>();

    foreach (var p in redTiles)
    {
        if (!byX.ContainsKey(p.X)) byX[p.X] = [];
        byX[p.X].Add(p.Y);

        if (!byY.ContainsKey(p.Y)) byY[p.Y] = new List<long>();
        byY[p.Y].Add(p.X);
    }

    var hSegments = new List<Segment>();
    var vSegments = new List<Segment>();

    foreach (var kv in byX)
    {
        var ys = kv.Value.OrderBy(y => y).ToArray();
        for (var i = 0; i + 1 < ys.Length; i += 2)
        {
            vSegments.Add(new Segment(new Point(kv.Key, ys[i]), new Point(kv.Key, ys[i + 1])));
        }
    }

    foreach (var kv in byY)
    {
        var xs = kv.Value.OrderBy(x => x).ToArray();
        for (var i = 0; i + 1 < xs.Length; i += 2)
        {
            hSegments.Add(new Segment(new Point(xs[i], kv.Key), new Point(xs[i + 1], kv.Key)));
        }
    }
    long max = 0;

    for (var i = 0; i < n; i++)
    {
        for (var j = i + 1; j < n; j++)
        {
            var a = redTiles[i];
            var b = redTiles[j];
            
            var valid = RectValid(a, b, hSegments.ToArray(), vSegments.ToArray());
            if (!valid) continue;
            var area = RectSize(redTiles[i], redTiles[j]);
            max = Math.Max(max, area);
        }
    }
    
    Console.WriteLine(max);
}

bool RectValid(Point a, Point b, Segment[] hSegments, Segment[] vSegments)
{
    var minX = Math.Min(a.X, b.X);
    var maxX = Math.Max(a.X, b.X);
    var minY = Math.Min(a.Y, b.Y);
    var maxY = Math.Max(a.Y, b.Y);

    foreach (var seg in hSegments)
    {
        var hy = seg.A.Y;
        var hx1 = Math.Min(seg.A.X, seg.B.X);
        var hx2 = Math.Max(seg.A.X, seg.B.X);

        if (hy > minY && hy < maxY && hx2 > minX && hx1 < maxX)
        {
            return false;
        }
    }
    
    foreach (var seg in vSegments)
    {
        var vx = seg.A.X;
        var vy1 = Math.Min(seg.A.Y, seg.B.Y);
        var vy2 = Math.Max(seg.A.Y, seg.B.Y);

        if (vx > minX && vx < maxX && vy2 > minY && vy1 < maxY)
        {
            return false;
        }
    }

    return true;
}

long RectSize(Point a, Point b)
{ 
    var x = Math.Abs(a.X - b.X) +1;
    var y = Math.Abs(a.Y - b.Y) + 1;
    return x * y;
}

record Point(long X, long Y);
record Segment(Point A, Point B);