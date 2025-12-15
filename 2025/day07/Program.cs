// See https://aka.ms/new-console-template for more information

await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var matrix = lines.Select(s => s.ToCharArray()).ToArray();
    var count = 0;
    for (var index = 0; index < matrix.Length; index++)
    {
        var prev = matrix[index-1];
        var active = matrix[index];
        for (var i = 0; i < active.Length; i++)
        {
            var p = prev[i];
            var c = active[i];
            if (c == '.' && p == 'S') 
                active[i] = 'S';
            if (c == '^' && p == 'S')
            {
                count++;
                if(i-1 >=0) active[i-1] = 'S';
                if(i+1 < active.Length) active[i+1] = 'S';
            }
        }
    }
    Console.WriteLine(count);
    }

async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var matrix = lines.Select(s => s
        .ToCharArray()
        .Select(c => c switch
        {
            '.' => 0L,
            'S' => 1L,
            '^' => -1L,
            _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
        }).ToArray()
    ).ToArray();
    
    for (var row = 1; row < matrix.Length; row++)
    {
        var prev = matrix[row - 1];
        var cur = matrix[row];
        var next = (long[])cur.Clone();   // work on a NEW array

        // zero out all pass-through cells; keep splitters intact
        for (int i = 0; i < next.Length; i++)
            if (next[i] >= 0) next[i] = 0;

        for (var i = 0; i < cur.Length; i++)
        {
            var p = prev[i];
            if (p <= 0) continue;

            if (cur[i] == 0L)          // pass-through
                next[i] += p;

            else if (cur[i] == -1)    // splitter
            {
                if (i - 1 >= 0) next[i - 1] += p;
                if (i + 1 < next.Length) next[i + 1] += p;
            }
        }

        matrix[row] = next; // replace the row AFTER processing
    }
    
    Console.WriteLine(matrix.Last().Sum());
    
}