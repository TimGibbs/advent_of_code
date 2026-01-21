// @algorithm Hex Grid Distance Tracking
// @category Geometry / Grid Navigation
// @input Comma-separated directions on a hex grid (n, s, ne, nw, se, sw)
// @problem
//   Track movement on a hex grid using cube coordinates
// @parts
//   Part1: Distance from origin after all moves
//   Part2: Maximum distance from origin at any point during the walk
// @model
//   - Cube coordinates (q, r, s) with invariant q + r + s = 0
// @technique
//   - Incremental coordinate updates per direction
//   - Distance computed as max(|q|, |r|, |s|)
// @data-structures
//   - Integers for cube coordinates
// @complexity
//   Time: O(n)
//   Space: O(1)
// @notes
//   - Cube-coordinate distance avoids axial/offset edge cases
//   - Tracking max distance requires checking after every step

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
