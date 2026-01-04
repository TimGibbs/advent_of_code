// @algorithm StringDecompression
// @category Parsing / Compression
// @input
//   Single compressed string with (AxB) markers
// @technique
//   Sequential Scan
//   Recursive Expansion (Part 2)
// @operations
//   Marker Parsing
//   Length Accumulation
// @data-structure
//   ReadOnlySpan<char>
// @paradigm
//   Iterative Parsing (Part 1)
//   Divide-and-Conquer Recursion (Part 2)
// @variant
//   Part1: Non-recursive decompressed length
//   Part2: Recursive decompressed length
// @complexity
//   Part1 Time: O(n)
//   Part2 Time: O(n * nesting)
//   Space: O(nesting depth)


await Part1();
await Part2();
return;

async Task Part1()
{
    var line = await File.ReadAllTextAsync("input.txt");
    var count = 0;
    for (int i = 0; i < line.Length; i++)
    {
        if (line[i] != '(')
        {
            count++; 
            continue;
        }

        var end = line.IndexOf(')', i);
        var slice = line[(i + 1) .. end]
            .Split('x')
            .Select(int.Parse)
            .ToArray();
        var reach = slice[0];
        var times = slice[1];
        count += reach * times;
        i = end + reach;
    }
    Console.WriteLine(count);
}

async Task Part2()
{
    var line = await File.ReadAllTextAsync("input.txt");
    var length = DecompressedLength(line);
    Console.WriteLine(length);
}


static long DecompressedLength(ReadOnlySpan<char> span)
{
    long count = 0;

    for (int i = 0; i < span.Length; i++)
    {
        if (span[i] != '(')
        {
            count++;
            continue;
        }

        var end = span[i..].IndexOf(')') + i;
        var marker = span[(i + 1)..end]
            .ToString()
            .Split('x')
            .Select(int.Parse)
            .ToArray();

        var reach = marker[0];
        var times = marker[1];

        var sectionStart = end + 1;
        var section = span.Slice(sectionStart, reach);

        count += DecompressedLength(section) * times;
        i = sectionStart + reach - 1;
    }

    return count;
}
