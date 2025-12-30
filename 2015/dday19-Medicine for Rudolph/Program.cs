// @algorithm StringRewriting / TokenCounting
// @category String Processing / Combinatorics
// @data-structure Dictionary, HashSet, List
// @complexity 
//   Part1: Time O(r · n · k), Space O(m)   // replacements × molecule length × variants
//   Part2: Time O(n), Space O(n)
// @variant 
//   Part1: SingleReplacementEnumeration
//   Part2: GrammarReductionByCounting

await Part1();
await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var replacements = new Dictionary<string, List<string>>();
    
    foreach (var line in lines)
    {
        if (string.IsNullOrWhiteSpace(line)) break;
        var split = line.Split(' ');
        if (!replacements.TryAdd(split[0], [split[2]]))
        {
            replacements[split[0]].Add(split[2]);
        }
    }

    var start = lines[^1];
    var results = new HashSet<string>();
    foreach (var (key, value) in replacements)
    {
        var l = key.Length;
        foreach (var position in AllIndexesOf(start,key))
        {
            foreach (var v in value)
            {
                var s = start[..position] + v + start[(position + l)..];
                results.Add(s);
            }
        }
    }
    Console.WriteLine(results.Count);


}
async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var molecule = lines[^1];

    // Tokenize atoms
    var atoms = new List<string>();
    for (var i = 0; i < molecule.Length; i++)
    {
        if (!char.IsUpper(molecule[i])) continue;
        if (i + 1 < molecule.Length && char.IsLower(molecule[i + 1]))
        {
            atoms.Add(molecule.Substring(i, 2));
            i++;
        }
        else
        {
            atoms.Add(molecule[i].ToString());
        }
    }

    var total = atoms.Count;
    var rn = atoms.Count(a => a == "Rn");
    var ar = atoms.Count(a => a == "Ar");
    var y = atoms.Count(a => a == "Y");

    var steps = total - rn - ar - 2 * y - 1;

    Console.WriteLine(steps);
}


IEnumerable<int> AllIndexesOf(string str, string searchstring)
{
    var minIndex = str.IndexOf(searchstring, StringComparison.Ordinal);
    while (minIndex != -1)
    {
        yield return minIndex;
        minIndex = str.IndexOf(searchstring, minIndex + searchstring.Length, StringComparison.Ordinal);
    }
}