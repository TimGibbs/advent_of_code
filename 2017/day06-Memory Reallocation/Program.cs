// @algorithm Memory Reallocation Detection
// @category State Simulation / Cycle Detection
// @input Tab-separated list of memory bank block counts
// @problem
//   Reallocate blocks among memory banks until a configuration repeats
// @rules
//   - Find the bank with the most blocks (lowest index wins ties)
//   - Set it to zero
//   - Distribute its blocks one-by-one to subsequent banks (wrapping)
// @parts
//   Part1: Count redistribution cycles until a configuration is seen again
//   Part2: Determine the size of the loop between repeated configurations
// @technique
//   - In-place array mutation
//   - State fingerprinting via string key
// @data-structures
//   int[]
//   HashSet<string> / Dictionary<string,int>
// @complexity
//   Time: O(cycles * banks)
//   Space: O(cycles)
// @notes
//   - Uses string-joined memory state as a stable hash key
//   - Dictionary in Part2 tracks first occurrence index to compute loop length

await Part1();
await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllTextAsync("input.txt");
    var memory = lines.Split('\t').Select(int.Parse).ToArray();
    var seen = new HashSet<string> { Key(memory) };
    var count = 0;
    while (true)
    {
        count++;
        var max = memory.Index().MaxBy(x => x.Item);
        memory[max.Index] = 0;
        for (int i = 0; i < max.Item; i++)
        {
            var j = (max.Index + 1 + i) % memory.Length;
            memory[j]++;
        }

        if (!seen.Add(Key(memory))) break;
    }
    Console.WriteLine(count);
}

string Key(int[] memory)
{
    return string.Join("", memory);
}
async Task Part2()
{
    var lines = await File.ReadAllTextAsync("input.txt");
    var memory = lines.Split('\t').Select(int.Parse).ToArray();
    var seen = new Dictionary<string, int>();
    seen[Key(memory)] = 0;
    var count = 0;
    while (true)
    {
        count++;
        var max = memory.Index().MaxBy(x => x.Item);
        memory[max.Index] = 0;
        for (int i = 0; i < max.Item; i++)
        {
            var j = (max.Index + 1 + i) % memory.Length;
            memory[j]++;
        }

        if (!seen.TryAdd(Key(memory),count))
        {
            var p = seen[Key(memory)];
            Console.WriteLine(count-p);
            break;
        };
    }
}