

await Part1();
await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var maps = GenerateMaps(lines.Length);
    var containers = lines.Select(int.Parse).ToArray();
    
    var result = maps.Select(x => x * containers).Count(x => x.Sum() == 150);
    
    Console.WriteLine(result);
    
}

async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var maps = GenerateMaps(lines.Length);
    var containers = lines.Select(int.Parse).ToArray();

    var result = maps
        .Where(x => (x * containers).Sum() == 150)
        .GroupBy(x => x.Sum())
        .OrderBy(x => x.Key)
        .First()
        .Count();

    Console.WriteLine(result);
}

IEnumerable<int[]> GenerateMaps(int length)
{
    if (length is < 0 or > 31)
        throw new ArgumentOutOfRangeException(nameof(length));

    var total = 1 << length;

    for (var mask = 0; mask < total; mask++)
    {
        var arr = new int[length];
        for (var i = 0; i < length; i++)
        {
            arr[i] = (mask & (1 << i)) != 0 ? 1 : 0;
        }
        yield return arr;
    }
}

public static class ArrayExtensions
{
    extension(int[])
    {

        
        // Element-wise multiplication by an integer
        public static int[] operator *(int[] a, int[] b)
        {
            if (a.Length != b.Length) throw new Exception("different lengths");
            
            var result = new int[a.Length];
            for (var i = 0; i < result.Length; i++)
                result[i] = a[i] * b[i];
            return result;
        }

    }

}
