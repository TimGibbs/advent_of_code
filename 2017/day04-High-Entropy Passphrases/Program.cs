// @algorithm Passphrase Validation
// @category String Processing / Constraint Checking
// @input List of passphrases (space-separated words per line)
// @problem
//   Validate passphrases under different uniqueness rules
// @rules
//   Part1: No duplicate words allowed
//   Part2: No two words may be anagrams
// @technique
//   - Set-based uniqueness checking
//   - Character frequency normalization for anagram detection
// @features
//   - Per-line independent validation
//   - Fixed-size letter histogram (a–z) for fast comparison
// @variant
//   Part1: Exact string uniqueness
//   Part2: Anagram-equivalence uniqueness
// @data-structures
//   String[]
//   int[26] character frequency arrays
// @complexity
//   Part1 Time: O(W) per line
//   Part2 Time: O(W² · L)
//   Space: O(W · L)
//   (W = words per line, L = word length)

await Part1();
await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var a = lines.Select(x => x.Split(' ')).Count(x => x.Distinct().Count() == x.Length);
    Console.WriteLine(a);
}
async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var a = lines.Select(x => x.Split(' ').Select(y =>
    {
        var arr = new int[26];
        foreach (var c in y)
        {
            arr[c - 'a']++;
        }
        return arr;
    }).ToArray()).Count(x =>
    {
        for (int i = 0; i < x.Count(); i++)
        {
            var a = x[i];
            for (int j = 0; j < x.Count(); j++)
            {
                if(i==j) continue;
                var b = x[j];
                if (a.SequenceEqual(b)) return false;
            }
        }

        return true;
    });
    Console.WriteLine(a);
}