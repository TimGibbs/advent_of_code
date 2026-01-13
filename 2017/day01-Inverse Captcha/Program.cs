// @algorithm Circular Digit Comparison (Captcha Solver)
// @category String Processing / Iterative Comparison
// @input Circular string of digits
// @problem
//   Sum digits that match another digit at a fixed offset
// @rules
//   Part1: Compare each digit with the next (circular)
//   Part2: Compare each digit with the digit halfway around the list
// @technique
//   - Modular indexing for circular access
//   - ASCII-to-int conversion for digit values
// @features
//   - Wraparound comparison (last ↔ first)
//   - Single-pass summation
// @variant
//   Part1: Offset = 1
//   Part2: Offset = length / 2
// @data-structures
//   String for digit sequence
// @complexity
//   Time: O(n)
//   Space: O(1)

await Part1();
await Part2();
return;

async Task Part1()
{
    var line = await File.ReadAllTextAsync("input.txt");
    var sum = 0;
    for (int i = 0; i < line.Length-1; i++)
    {
        if (line[i] == line[i + 1]) sum += line[i] - '0';
    }
    if(line[^1] == line[0]) sum += line[^1] - '0';
    Console.WriteLine(sum);
}

async Task Part2()
{
    var line = await File.ReadAllTextAsync("input.txt");
    var l = line.Length;
    var offset = l / 2;
    var sum = 0;
    for (int i = 0; i < line.Length-1; i++)
    {
        if (line[i] == line[(i + offset)%l]) sum += line[i] - '0';
    }
    if(line[^1] == line[offset]) sum += line[^1] - '0';
    Console.WriteLine(sum);
}