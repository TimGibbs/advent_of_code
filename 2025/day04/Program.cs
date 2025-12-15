// See https://aka.ms/new-console-template for more information

await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var matrix = lines.Select(x=>x.ToCharArray()).ToArray();
    var accessible = 0;
    for (var y = 0; y < matrix.Length; y++)
    {
        for (var x = 0; x < matrix[y].Length; x++)
        {
            if(matrix[y][x] != '@') continue;
            var adj = Adjacents(x, y, matrix[y].Length, matrix.Length);
            var count = adj.Sum(p =>
            {
                var (xi, yi) = p;
                return matrix[yi][xi] == '@' ? 1 : 0;
            });
            if(count<4) accessible++;
        }
    }
    Console.WriteLine(accessible);
}

async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var matrix = lines.Select(x=>x.ToCharArray()).ToArray();
    var accessible = 0;
    bool cont = true;
    while (cont)
    {
        cont = false;
        for (var y = 0; y < matrix.Length; y++)
        {
            for (var x = 0; x < matrix[y].Length; x++)
            {
                if(matrix[y][x] != '@' ) continue;
                var adj = Adjacents(x, y, matrix[y].Length, matrix.Length);
                var count = adj.Sum(p =>
                {
                    var (xi, yi) = p;
                    return matrix[yi][xi] == '@' ? 1 : 0;
                });
                if (count >= 4) continue;
                accessible++;
                matrix[y][x] = '#';
                cont = true;
            }
        }
    }
    
    Console.WriteLine(accessible);
}

(int,int)[] Adjacents(int x, int y, int xBound, int yBound)
{
    (int, int)[] a = [
        (x - 1, y - 1), (x - 1, y), (x - 1, y + 1),
        (x, y - 1), (x, y + 1),
        (x + 1, y - 1), (x + 1, y), (x + 1, y + 1)
    ];
    return a.Where(z=> 
        z.Item1>=0 && z.Item1 < xBound &&
        z.Item2>=0 && z.Item2 < yBound
                       ).ToArray();
}