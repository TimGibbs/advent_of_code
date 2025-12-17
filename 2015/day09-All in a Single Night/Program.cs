// @algorithm TravelingSalesmanBruteForce
// @category Graph / Combinatorics
// @data-structure Dictionary, HashSet, Array
// @complexity Time: O(n!), Space: O(n^2)
// @variant Part1: ShortestRoute, Part2: LongestRoute

await Part1();
await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var distances = new Dictionary<(string, string), int>();
    var cities = new HashSet<string>();

    foreach (var line in lines)
    {
        var parts = line.Split(" ");
        var a = parts[0];
        var b = parts[2];
        var d = int.Parse(parts[4]);

        distances[(a, b)] = d;
        distances[(b, a)] = d;

        cities.Add(a);
        cities.Add(b);
    }
    var cityList = cities.ToArray();
    var shortest = int.MaxValue;

    foreach (var route in Permute(cityList))
    {
        var sum = 0;
        for (int i = 0; i < route.Length - 1; i++)
        {
            sum += distances[(route[i], route[i + 1])];
        }

        shortest = Math.Min(shortest, sum);
    }

    Console.WriteLine(shortest);

}

static IEnumerable<T[]> Permute<T>(T[] items)
{
    if (items.Length == 1)
        yield return items;

    for (int i = 0; i < items.Length; i++)
    {
        var rest = items.Where((_, index) => index != i).ToArray();
        foreach (var perm in Permute(rest))
        {
            yield return new[] { items[i] }.Concat(perm).ToArray();
        }
    }
}

async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var distances = new Dictionary<(string, string), int>();
    var cities = new HashSet<string>();

    foreach (var line in lines)
    {
        var parts = line.Split(" ");
        var a = parts[0];
        var b = parts[2];
        var d = int.Parse(parts[4]);

        distances[(a, b)] = d;
        distances[(b, a)] = d;

        cities.Add(a);
        cities.Add(b);
    }
    var cityList = cities.ToArray();
    var longest = int.MinValue;

    foreach (var route in Permute(cityList))
    {
        var sum = 0;
        for (int i = 0; i < route.Length - 1; i++)
        {
            sum += distances[(route[i], route[i + 1])];
        }

        longest = Math.Max(longest, sum);
    }

    Console.WriteLine(longest);

}

