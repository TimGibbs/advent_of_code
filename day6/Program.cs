// See https://aka.ms/new-console-template for more information

await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var blah = new List<string>[lines.Length].Select(item=>new List<string>()).ToArray();
    var position = 0;
    for (var j = 0; j < lines[0].Length; j++)
    {
        if(lines.All(l=>l[j]==' '))
        {
            for (var q = 0; q < blah.Length; q++)
            {
                blah[q].Add(lines[q][position .. j]);
            }
            position = j+1;
        };
    }
    for (var q = 0; q < blah.Length; q++)
    {
        blah[q].Add(lines[q][position ..]);
    }
    var sum = 0L;
    for (var i = 0; i < blah[0].Count; i++)
    {
        var nums = blah[..^1].Select(x => long.Parse(x[i])).ToArray();
        var opt = blah[^1][i].Trim();
        var val = opt switch
        {
            "+" => nums.Sum(),
            "*" => Product(nums.ToArray()),
            _ => throw new Exception()
        };
        sum += val;
    }
    Console.WriteLine(sum);
}
static long Product(long[] ar)
{
    return ar.Aggregate(1L, (current, t) => current * t);
}


async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var matrix = lines.Select(o=>o.ToCharArray()).ToArray();
    var nums = new List<long>();
    var opt = ' ';
    var sum = 0L;
    for (var i = 0; i < matrix[0].Length; i++)
    {
        var i1 = i;
        var g = string.Concat(matrix[..^1].Select(s => s[i1]));
        if (!g.IsWhiteSpace())
        {
            nums.Add(long.Parse(g));
            if(matrix[^1][i1]!=' ') opt = matrix[^1][i1];
        }

        if (!g.IsWhiteSpace() && i != matrix[0].Length - 1) continue;
        var val = opt switch
        {
            '+' => nums.Sum(),
            '*' => Product(nums.ToArray()),
            _ => throw new Exception()
        };
        sum += val;
            
        nums.Clear();
        opt = ' ';
    }
    Console.WriteLine(sum);
}

