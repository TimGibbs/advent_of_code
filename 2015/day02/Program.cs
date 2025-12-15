// See https://aka.ms/new-console-template for more information

await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");

    var result = lines
        .Select(s => s.Split('x').Select(int.Parse).ToArray())
        .Sum(a =>
        {
            var x = a[0] * a[1];
            var y = a[1] * a[2];
            var z = a[0] * a[2];
            return 2 * (x + y + z) + Math.Min(x, Math.Min(y, z));
        });
    Console.WriteLine(result);
}

async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");

    var result = lines
        .Select(s => s.Split('x').Select(int.Parse).ToArray())
        .Sum(a =>
        {
            var min1 = Math.Min(a[0], a[1]);
            var max1 = Math.Max(a[0], a[1]);
            var min2 = Math.Min(a[2], max1);

            return 2* min1 + 2*min2 + a[0]*a[1]*a[2];
        });
    Console.WriteLine(result);
}