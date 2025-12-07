// See https://aka.ms/new-console-template for more information

await Part2();
return;
async Task Part1()
{
    var text = await File.ReadAllTextAsync("input.txt");
    var ranges =  text.Split(',').Select(x => x.Split('-').Select(long.Parse).ToArray()).ToArray();
    long sum = 0;

    foreach (var range in ranges)
    {
        for (var i = range[0]; i <= range[1]; i++)
        {
            if(IsInvalid(i)) sum += i;
        }
    }
    Console.WriteLine(sum);
}

bool IsInvalid(long value)
{
    var str = value.ToString();
    if(str.Length%2 ==1) return false;
    var mid = str.Length / 2;
    for (var i = 0; i < str.Length / 2; i++)
    {
        if(str[i] != str[mid+i]) return false;
    }

    return true;
}

async Task Part2()
{
    var text = await File.ReadAllTextAsync("input.txt");
    var ranges =  text.Split(',').Select(x => x.Split('-').Select(long.Parse).ToArray()).ToArray();
    long sum = 0;

    foreach (var range in ranges)
    {
        for (var i = range[0]; i <= range[1]; i++)
        {
            if(IsInvalid2(i)) sum += i;
        }
    }
    Console.WriteLine(sum);
}

bool IsInvalid2(long value)
{
    var str = value.ToString();
    for (var i = 1; i < str.Length; i++)
    {
        if(str.Length%i !=0) continue;
        var chunks = str.Chunk(i).ToArray();
        for (var j = 0; j < i; j++)
        {
            if (chunks.Skip(1).All(chunk => chunk.SequenceEqual(chunks[0])))
                return true;
        }
    }

    return false;
}