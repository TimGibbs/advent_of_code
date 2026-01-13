// @algorithm Spreadsheet Checksum
// @category Numeric Processing / Grid Reduction
// @input Rows of integers (whitespace- or tab-separated)
// @problem
//   Compute checksums from rows of numbers
// @rules
//   Part1: For each row, take (max − min), then sum
//   Part2: For each row, find the only evenly divisible pair, divide, then sum
// @technique
//   - Per-row aggregation
//   - Pairwise search for divisibility
// @features
//   - Input normalization (tabs → spaces)
//   - Early exit on valid divisible pair
// @variant
//   Part1: Range-based checksum
//   Part2: Division-based checksum
// @data-structures
//   int[] for rows
//   int[][] for full spreadsheet
// @complexity
//   Part1 Time: O(n · m)
//   Part2 Time: O(n · m²)
//   Space: O(n · m)
//   (n = rows, m = columns)

await Part1();
await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var vals = lines.Select(s => s.Replace("\t"," ").Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray())
        .ToArray();
    var result = vals.Select(x => x.Max() - x.Min()).Sum();
    Console.WriteLine(result);
}

async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var vals = lines.Select(s => s.Replace("\t"," ").Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray())
        .ToArray();
    var result = vals.Select(x =>
    {
        for (int i = 0; i < x.Length; i++)
        {
            for (int j = 0; j < x.Length; j++)
            {
                if(i==j) continue;
                if (x[i] % x[j] == 0) return x[i] / x[j];
            }
        }

        throw new Exception("this shouldnt happen");
    }).Sum();
    Console.WriteLine(result);
}