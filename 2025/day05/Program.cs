// See https://aka.ms/new-console-template for more information

await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var ranges = new List<(long,long)>();
    var ingredients = new List<long>();
    foreach (var line in lines)
    {
        if(line.Length==0) continue; // skip empty lines ( new group)
        if (line.Contains('-'))
        {
            ranges.Add((long.Parse(line[..line.IndexOf('-')]), long.Parse(line[(line.IndexOf('-')+1)..])));;
        }
        else
        {
            ingredients.Add(long.Parse(line));
        }
    }

   
    var fresh = ingredients.Count(x =>
    {
        return ranges.Any(r => x>=r.Item1 && x<=r.Item2);
    });
    Console.WriteLine(fresh);
}

async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var ranges = new List<(long,long)>();
    foreach (var line in lines)
    {
        if (line.Length == 0) continue; // skip empty lines ( new group)
        if (line.Contains('-'))
        {
            ranges.Add((long.Parse(line[..line.IndexOf('-')]), long.Parse(line[(line.IndexOf('-') + 1)..])));
            ;
        }
    }
    ranges = ranges.OrderBy(x=>x.Item1).ToList();;
    var pt = 0L;
    var sum = 0L;
    foreach (var range in ranges)
    {
        if(range.Item2<pt) continue;
        if (range.Item1 > pt)
        {
            var count = range.Item2 - range.Item1+1;
            sum += count;
        }
        else
        {
            var count = range.Item2 - pt;
            sum += count;
        }

        pt = range.Item2;
    }
    Console.WriteLine(sum);
}

