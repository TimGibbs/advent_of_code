// See https://aka.ms/new-console-template for more information

await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var points = lines.Select(x =>
    {
        var split = x.Split(',').Select(int.Parse).ToArray();
        return new Point3d(split[0], split[1], split[2]);
    }).ToArray();

    var circuits = points.Select(o => new List<Point3d>() { o }).ToList();
    
    var distances =
        points
            .SelectMany((a, i) => points
                .Skip(i + 1)                     // ensures each pair is used once
                .Select(b => (Distance: EuclideanDistance(a, b), A: a, B: b)))
            .OrderBy(x => x.Distance)
            .ToList();

    var counter = 0;
    var limit = 1000;
    foreach (var (dist, a, b) in distances)
    {
        
        if(counter++==limit) break;
        if(circuits.Any(c=>c.Contains(a) && c.Contains(b))) continue;
        var c1 = circuits.First(c=>c.Contains(a));
        var c2 = circuits.First(c=>c.Contains(b));
        c1.AddRange(c2);
        circuits.Remove(c2);
        
        
    }

    var lenths = circuits.Select(c => c.Count).OrderByDescending(o => o).Take(3).ToList();
    Console.WriteLine(lenths.Aggregate((a, b) => a * b));

}

async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var points = lines.Select(x =>
    {
        var split = x.Split(',').Select(int.Parse).ToArray();
        return new Point3d(split[0], split[1], split[2]);
    }).ToArray();

    var circuits = points.Select(o => new List<Point3d>() { o }).ToList();
    
    var distances =
        points
            .SelectMany((a, i) => points
                .Skip(i + 1)                     // ensures each pair is used once
                .Select(b => (Distance: EuclideanDistance(a, b), A: a, B: b)))
            .OrderBy(x => x.Distance)
            .ToList();
    
    foreach (var (dist, a, b) in distances)
    {
        if(circuits.Any(c=>c.Contains(a) && c.Contains(b))) continue;
        var c1 = circuits.First(c=>c.Contains(a));
        var c2 = circuits.First(c=>c.Contains(b));
        c1.AddRange(c2);
        circuits.Remove(c2);
        if (circuits.Count == 1)
        {
            Console.WriteLine(a.X*b.X);
            break;
        }
        
    }
}

decimal EuclideanDistance(Point3d p1, Point3d p2)
{
    return (decimal)Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2) + Math.Pow(p1.Z - p2.Z, 2));
}

public record Point3d(int X, int Y, int Z);