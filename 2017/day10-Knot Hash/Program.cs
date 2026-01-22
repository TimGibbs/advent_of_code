// @algorithm Knot Hash
// @category Hashing / Circular List Manipulation
// @input
//   Part1: Comma-separated integers
//   Part2: Raw string (ASCII codes + fixed suffix)
// @problem
//   Part1: Perform one round of knotting and output product of first two values
//   Part2: Compute full Knot Hash and output hex digest
// @parts
//   Part1: Single-round circular reversal
//   Part2: 64-round knotting + dense hash via XOR
// @model
//   - Circular array with current position and skip size
//   - Segment reversal with wraparound
// @technique
//   - Span-based in-place reversal
//   - Temporary buffer for wrapped segments
//   - Chunked XOR reduction (dense hash)
// @data-structures
//   - Fixed-size int array (256 elements)
//   - Stack-allocated buffer for reversals
// @complexity
//   Time: O(n * rounds)
//   Space: O(n)
// @notes
//   - Lengths larger than list size are ignored
//   - Dense hash is formed by XOR-ing blocks of 16 values

await Part1();
await Part2();
return;

async Task Part1()
{
    var line = await File.ReadAllTextAsync("input.txt");
    var lengths = line.Split(',').Select(int.Parse).ToArray();

    var loop = Enumerable.Range(0, 256).ToArray();
    var position = 0;
    var skipSize = 0;

    Logic.RunKnotRound(loop, lengths, ref position, ref skipSize);

    Console.WriteLine(loop[0] * loop[1]);
}

async Task Part2()
{
    var line = await File.ReadAllTextAsync("input.txt");
    Console.WriteLine(Logic.KnotHash(line.Trim()));
}

public static class Logic
{
    public static void RunKnotRound(
        int[] loop,
        ReadOnlySpan<int> lengths,
        ref int position,
        ref int skipSize)
    {
        Span<int> buffer = stackalloc int[loop.Length];

        foreach (var length in lengths)
        {
            if (length > loop.Length)
                continue;

            if (position + length <= loop.Length)
            {
                loop.AsSpan(position, length).Reverse();
            }
            else
            {
                var firstPart = loop.Length - position;
                var secondPart = length - firstPart;

                var temp = buffer[..length];

                loop.AsSpan(position, firstPart).CopyTo(temp);
                loop.AsSpan(0, secondPart).CopyTo(temp[firstPart..]);

                temp.Reverse();

                temp[..firstPart].CopyTo(loop.AsSpan(position, firstPart));
                temp[firstPart..].CopyTo(loop.AsSpan(0, secondPart));
            }

            position = (position + length + skipSize) % loop.Length;
            skipSize++;
        }
    }

    public static string ComputeDenseHash(int[] sparseHash)
    {
        return string.Concat(
            sparseHash
                .Chunk(16)
                .Select(chunk => chunk.Aggregate((a, b) => a ^ b))
                .Select(x => x.ToString("x2"))
        );
    }

    public static string KnotHash(string input)
    {
        var lengths = input
            .Select(c => (int)c)
            .Concat([17, 31, 73, 47, 23])
            .ToArray();

        var loop = Enumerable.Range(0, 256).ToArray();
        var position = 0;
        var skipSize = 0;

        for (var round = 0; round < 64; round++)
        {
            RunKnotRound(loop, lengths, ref position, ref skipSize);
        }

        return ComputeDenseHash(loop);
    }
}

