// @algorithm LogicCircuitSimulation
// @category Simulation / DependencyResolution
// @data-structure Dictionary
// @complexity Time: O(n^2), Space: O(n)
// @variant Part1: ComputeInitialValues, Part2: OverrideBAndRecompute

await Part1();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var parts = lines.Select(s => new Line(s)).ToArray();
    
    var values = Process(parts);
    Console.WriteLine("Part1:"+values["a"]);

    var b = parts.Single(b => b.Output == "b");
    b.Input1Val = values["a"];
    values = Process(parts);
    Console.WriteLine("Part2:"+values["a"]);
}


Dictionary<string, ushort> Process(Line[] parts1)
{
    var dictionary = new Dictionary<string, ushort>();
    foreach (var part in parts1.Where(x => x.StarterInput))
    {
        dictionary[part.Output] = part.Input1Val ?? throw new ArgumentException($"invalid line {part}");
    }

    while (true)
    {
        var toGo = parts1.Where(x =>
                !dictionary.ContainsKey(x.Output)
                && (dictionary.ContainsKey(x.Input1) || x.Input1Val is not null)
                && (x.Operation is "NOT" || x.Operation is null ||
                    dictionary.ContainsKey(x.Input2) || x.Input2Val is not null))
            .ToArray();

        if (!toGo.Any())
            break;
        foreach (var line in toGo)
        {
            dictionary[line.Output] = line.Operation switch
            {
                "AND" => (ushort)((line.Input1Val ?? dictionary[line.Input1]) & dictionary[line.Input2]),
                "OR" => (ushort)((line.Input1Val ?? dictionary[line.Input1]) | dictionary[line.Input2]),
                "LSHIFT" => (ushort)(dictionary[line.Input1] << line.Input2Val),
                "RSHIFT" => (ushort)(dictionary[line.Input1] >> line.Input2Val),
                "NOT" => (ushort)(~dictionary[line.Input1]),
                _ => dictionary[line.Input1]
            };
        }
    }

    return dictionary;
}

record Line
{
    public string Output { get; }
    public string Input1 { get; }
    public ushort? Input1Val { get; set; }
    public string? Input2 { get; }
    public ushort? Input2Val { get; }
    public string? Operation { get; }
    public bool StarterInput { get; }
    public Line(string str)
    {
        var s = str.Split(" -> ");
        Output = s[1];
        var lhs = s[0].Split(' ');

        switch (lhs.Length)
        {
            case 3:
                (Input1, Operation, Input2) = (lhs[0], lhs[1], lhs[2]);
                Input1Val = ushort.TryParse(Input1, out var val) ? val : (ushort?)null;
                Input2Val = ushort.TryParse(Input2, out var val2) ? val2 : (ushort?)null;
                return;
            case 2:
                (Operation, Input1) = (lhs[0], lhs[1]);
                Input1Val = ushort.TryParse(Input1, out var val3) ? val3 : (ushort?)null;
                return;
            case 1:
                Input1 = lhs[0];
                Input1Val = ushort.TryParse(Input1, out var val4) ? val4 : (ushort?)null;
                StarterInput = ushort.TryParse(lhs[0], out _);
                return;
        }
        throw new ArgumentException($"invalid line {str}");
    }
}