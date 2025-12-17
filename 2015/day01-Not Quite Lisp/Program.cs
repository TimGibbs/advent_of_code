// @algorithm PrefixSum
// @category StringProcessing
// @data-structure Counter
// @complexity Time: O(n), Space: O(1)
// @variant Part1: TotalFloor, Part2: FirstNegativeFloor

await Part1();
await Part2();
return;

async Task Part1()
{
    var line = await File.ReadAllTextAsync("input.txt");

    var floor = 0;
    for (var i = 0; i < line.Length; i++)
    {
        switch (line[i])
        {
            case '(': floor++; break;
            case ')': floor--; break;
        }
    }

    Console.WriteLine(floor);
}

async Task Part2()
{
    var line = await File.ReadAllTextAsync("input.txt");

    var floor = 0;
    for (var i = 0; i < line.Length; i++)
    {
        switch (line[i])
        {
            case '(': floor++; break;
            case ')': floor--; break;
        }

        if (floor < 0)
        {
            Console.WriteLine(i+1);
            break;
        }
    }
}