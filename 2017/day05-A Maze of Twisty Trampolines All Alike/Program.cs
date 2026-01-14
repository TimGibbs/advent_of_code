// @algorithm Jump Offset Simulation
// @category Array Simulation / Control Flow
// @input List of integer jump offsets
// @problem
//   Execute a self-modifying jump program until the instruction pointer exits the list
// @rules
//   Part1: Increment the current offset after each jump
//   Part2: Increment if offset < 3, otherwise decrement
// @technique
//   - In-place array mutation
//   - Instruction pointer simulation
// @features
//   - Counts total executed steps
//   - Terminates when IP leaves bounds
// @variant
//   Part1: Always increment
//   Part2: Conditional increment/decrement
// @data-structures
//   int[]
// @complexity
//   Time: O(steps)
//   Space: O(n)
//   (n = number of offsets)


await Part1();
await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var jumps = lines.Select(int.Parse).ToArray();
    var count = 0;
    for (int i = 0; i < jumps.Length;)
    {
        count++;
        var inst = jumps[i]++;
        i += inst;
    }
    Console.WriteLine(count);
}

async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var jumps = lines.Select(int.Parse).ToArray();
    var count = 0;
    for (int i = 0; i < jumps.Length;)
    {
        count++;
        var inst = jumps[i];
        if (inst >= 3)
        {
            jumps[i]--;
        }
        else
        {
            jumps[i]++;
        }
        i += inst;
    }
    Console.WriteLine(count);
}