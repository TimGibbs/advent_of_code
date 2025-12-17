// @algorithm TreeTraversalAggregation
// @category Parsing / TreeTraversal
// @data-structure JsonObject, JsonArray
// @complexity Time: O(n), Space: O(n)
// @variant Part1: RegexNumberSum, Part2: JsonTreeSumWithExclusion

using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

await Part1();
await Part2();
return;

async Task Part1()
{
    var line = await File.ReadAllTextAsync("input.txt");
    var regex = new Regex(@"[-]?\d+");
    var all = regex.Matches(line).Select(m => int.Parse(m.Value)).Sum();
    Console.WriteLine(all);
}

async Task Part2()
{
    var line = await File.ReadAllTextAsync("input.txt");
    var job = JsonNode.Parse(line);
    var sum = Value(job);
    Console.WriteLine(sum);
}

int? Value(JsonNode node)
{
    if (node is JsonObject o)
    {
        var s = 0;
        foreach (var (key, value) in o)
        {
            var r = Value(value);
            if (r is null) return 0;
            s += r.Value;
        }
        return s;
    }

    if (node is JsonArray a)
    {
        return a.Select(Value).Where(x=> x is not null).Sum();
    }

    if (node is JsonValue v)
    {
        return int.TryParse(v.ToString(), out var i) ? i : v.ToString()=="red" ? null : 0;
    }
    
    throw new ArgumentException("not a valid json node");
}