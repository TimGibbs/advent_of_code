// @algorithm Spinlock Simulation
// @category Circular Buffer / Modular Arithmetic
// @input Single integer step size
// @problem
//   Insert sequential integers into a circular buffer after stepping forward
// @parts
//   Part1: After inserting 2017 values, return value following 2017
//   Part2: After 50,000,000 insertions, return value following 0
// @model
//   - Circular structure with current position
//   - Insertion index computed via modular arithmetic
// @technique
//   - Part1: Explicit List<int> buffer with insertions
//   - Part2: Optimized tracking without full buffer
//       * Only track value at position 1 (after 0)
//       * Avoid O(n²) insert cost
// @formula
//   nextPosition = ((currentPosition + step) % currentLength) + 1
// @data-structures
//   - List<int> (Part1)
//   - Integer counters only (Part2)
// @complexity
//   Part1: O(n²) due to list insertions
//   Part2: O(n) time, O(1) space
// @notes
//   - Position 0 always contains value 0
//   - Part2 relies on invariant that only insertions at index 1 affect answer

await Part1();
await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllTextAsync("input.txt");
    var buffer = new List<int>(2018) {0};
    var currentPosition = 0;
    var currentLength = 1;
    var step = int.Parse(lines);
    
    for (int i = 1; i < 2018; i++)
    {
        var np = NextPosition(currentPosition, step, currentLength);
        buffer.Insert(np,i);
        currentPosition = np;
        currentLength++;
    }
    Console.WriteLine(buffer[buffer.IndexOf(2017)+1]);
}


async Task Part2()
{
    var lines = await File.ReadAllTextAsync("input.txt");
    int currentPosition = 0;
    int valueAfterZero = 0;
    int currentLength = 1;
    var step = int.Parse(lines);

    for (int i = 1; i <= 50_000_000; i++)
    {
        var np = NextPosition(currentPosition, step, currentLength);
        currentPosition = np;
        currentLength++;
        if (currentPosition == 1)
        {
            valueAfterZero = i;
        }
    }

    Console.WriteLine(valueAfterZero);
}

int NextPosition(int currentPosition, int step, int currentLength) 
    => ((currentPosition + step) % currentLength) + 1;
