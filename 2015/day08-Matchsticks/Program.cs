// @algorithm StringEncodingSimulation
// @category StringProcessing / Simulation
// @data-structure None
// @complexity Time: O(n), Space: O(1)
// @variant Part1: CountInMemoryCharacters, Part2: CountEncodedCharacters

await Part1();
await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var result = lines.Aggregate((0, 0), (tuple, s) => HandleLine(s, tuple));
    Console.WriteLine(result.Item1 - result.Item2);
}

(int, int) HandleLine(string line, (int,int) previous)
{
    var a = previous.Item1 + line.Length;
    var count = 0;
    for (var index = 0; index < line.Length-1; index++)
    {
        var c = line[index];
        if (c == '\\')
        {
            var next = line[index + 1];
            if (next is '\\' or '"') {index++;}
            if (next == 'x') index += 3;
        }
        count++;
    }
    var b = previous.Item2 + count;
    return (a,b);
}

async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var result = lines.Aggregate((0, 0), (tuple, s) => HandleLine2(s, tuple));
    Console.WriteLine(result.Item2-result.Item1);
}

(int, int) HandleLine2(string line, (int,int) previous)
{
    var a = previous.Item1 + line.Length;
    var count = 0;
    for (var index = 0; index < line.Length; index++)
    {
        var c = line[index];
        if (c is '\\' or '\"')
        {
            count++;
        }
        count++;
    }
    var b = previous.Item2 + count+2;
    return (a,b);
}