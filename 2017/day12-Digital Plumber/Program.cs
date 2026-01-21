await Part1();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var dict = lines.Select(s =>
    {
        var split = s.Split("<->");
        var k = int.Parse(split[0].Trim());
        var vs = split[1].Split(',').Select(x => int.Parse(x.Trim())).ToArray();
        return (k, vs);
    }).ToDictionary(k => k.k, v => v.vs);
    var groups = new List<HashSet<int>>();
    var visited = new HashSet<int>();
    foreach (var key in dict.Keys)
    {
        if (visited.Contains(key))
            continue;
        var group = new HashSet<int>();
        var toVisit = new Stack<int>();
        toVisit.Push(key);
        while (toVisit.Count > 0)
        {
            var current = toVisit.Pop();
            if (!visited.Add(current))
                continue;
            group.Add(current);
            foreach (var neighbor in dict[current])
            {
                if (!visited.Contains(neighbor))
                {
                    toVisit.Push(neighbor);
                }
            }
        }
        groups.Add(group);
    }
    Console.WriteLine(groups[0].Count);
    Console.WriteLine(groups.Count);
}
