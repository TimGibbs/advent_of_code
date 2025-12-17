// @algorithm LookAndSaySequence
// @category SequenceSimulation / StringProcessing
// @data-structure Array, Span
// @complexity Time: O(n * iterations), Space: O(n)
// @variant Part1: 40Iterations, Part2: 50Iterations

await Part1(40);
await Part1(50);
return;

async Task Part1(int iterations)
{
    var line = await File.ReadAllTextAsync("input.txt");
    ReadOnlySpan<char> start = line.AsSpan();
    
    char[] bufferA = new char[10_000_000];
    char[] bufferB = new char[10_000_000];

    int len = start.Length;
    start.CopyTo(bufferA);

    char[] srcBuf = bufferA;
    char[] dstBuf = bufferB;

    for (int i = 0; i < iterations; i++)
    {
        ReadOnlySpan<char> src = srcBuf.AsSpan(0, len);
        Span<char> dst = dstBuf;

        len = LookAndSay(src, dst);
        
        (srcBuf, dstBuf) = (dstBuf, srcBuf);
    }

    Console.WriteLine(len);
}

static int LookAndSay(
    ReadOnlySpan<char> input,
    Span<char> output)
{
    int outPos = 0;

    char current = input[0];
    int count = 1;

    for (int i = 1; i < input.Length; i++)
    {
        char c = input[i];
        if (c == current)
        {
            count++;
        }
        else
        {
            outPos += WriteCount(output[outPos..], count);
            output[outPos++] = current;
            current = c;
            count = 1;
        }
    }

    outPos += WriteCount(output.Slice(outPos), count);
    output[outPos++] = current;

    return outPos; // length written
}

static int WriteCount(Span<char> dst, int count)
{
    int written = 0;
    dst[written++] = (char)('0' + count);
    return written;
}
