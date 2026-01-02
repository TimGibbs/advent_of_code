// @algorithm TriangleValidation
// @category Geometry / Parsing
// @input
//   Text file lines with integer triples
// @technique
//   Constraint Checking (Triangle Inequality)
//   Data Re-orientation
// @variant
//   Part1: RowWiseTriangles
//   Part2: ColumnWiseTriangles (Grouped by 3 rows)
// @data-structure
//   int[]
//   IEnumerable
//   Grouping (batch-of-3)
// @complexity
//   Time: O(n)
//   Space: O(1) (excluding input storage)

await Part1();
await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var tri = lines.Select(x => x.Split(' ',StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray()).Count(x=>
        x[0]+x[1] > x[2]
        && x[1]+x[2] > x[0]
        && x[2]+x[0] > x[1]);
    Console.WriteLine(tri);
}

async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var tri = lines.Index()
        .GroupBy(x => x.Index / 3,
            x => x.Item.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray())
        .SelectMany(x =>
        {
            var q = x.ToArray();
            var r = new List<int[]>();
            r.Add([q[0][0], q[1][0], q[2][0]]);
            r.Add([q[0][1], q[1][1], q[2][1]]);
            r.Add([q[0][2], q[1][2], q[2][2]]);
            return r.ToArray();
        })
        .Count(x=>
            x[0]+x[1] > x[2]
            && x[1]+x[2] > x[0]
            && x[2]+x[0] > x[1]);
    Console.WriteLine(tri);

}