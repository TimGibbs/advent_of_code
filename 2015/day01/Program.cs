// See https://aka.ms/new-console-template for more information

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