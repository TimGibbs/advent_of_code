// @algorithm CombinatorialSearch / Optimization
// @category Combinatorics / Number Theory
// @data-structure Array
// @complexity
//   Time: Exponential (combinations search, worst-case O(C(n, k)))
//   Space: O(k) per combination (recursive stack + combination array)
// @technique Backtracking, Combinations, PruningByTargetSum
// @variant
//   Part1: PartitionInto3Groups (min-size group, minimal quantum entanglement)
//   Part2: PartitionInto4Groups (min-size group, minimal quantum entanglement)

await Part1();
await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var weights = lines.Select(long.Parse).ToArray();
    
    var total = weights.Sum();
    var target = total / 3;

    long[][] candidates = [];
    for (int n = 0; n < weights.Length/3; n++)
    {
        var combos = Combinations(weights, n).Where(x=>x.Sum()==target).ToArray();
        if (combos.Length == 0) continue;
        candidates = combos;
        break;
    }

    var result = candidates.Select(x => x.Aggregate(1L,(a, b) => a * b));
    Console.WriteLine(result.Min());
}

async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var weights = lines.Select(long.Parse).ToArray();
    
    var total = weights.Sum();
    var target = total / 4;

    long[][] candidates = [];
    for (int n = 0; n < weights.Length/4; n++)
    {
        var combos = Combinations(weights, n).Where(x=>x.Sum()==target).ToArray();
        if (combos.Length == 0) continue;
        candidates = combos;
        break;
    }

    var result = candidates.Select(x => x.Aggregate(1L,(a, b) => a * b));
    Console.WriteLine(result.Min());
}

static IEnumerable<T[]> Combinations<T>(T[] source, int n)
{
    if (n == 0)
    {
        yield return [];
        yield break;
    }

    for (int i = 0; i <= source.Length - n; i++)
    {
        foreach (var tail in Combinations(source[(i + 1)..], n - 1))
        {
            var result = new T[n];
            result[0] = source[i];
            tail.CopyTo(result, 1);
            yield return result;
        }
    }
}