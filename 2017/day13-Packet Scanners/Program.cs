// @algorithm Firewall Scanner Simulation
// @category Simulation / Modular Arithmetic
// @input Layer definitions: "depth: range"
// @problem
//   Simulate packet traversal through firewall layers with moving scanners
// @parts
//   Part1: Compute total severity for immediate traversal (delay = 0)
//   Part2: Find minimum delay to pass without being caught
// @model
//   - Scanner motion modeled as periodic back-and-forth cycle
//   - Period = (range * 2 - 2)
// @technique
//   - Modulo arithmetic to detect scanner at position 0
//   - Brute-force delay search with early exit
// @data-structures
//   - int[] for layer (depth, range)
// @complexity
//   Part1:
//     Time: O(n)
//     Space: O(1)
//   Part2:
//     Time: O(n * delay)
//     Space: O(1)
// @notes
//   - A packet is caught at layer d if (d + delay) % period == 0
//   - Severity = depth * range

await Part1();
await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var details = lines
        .Select(s => s.Split(": ").Select(int.Parse).ToArray())
        .Select(s => s[0] % (s[1] * 2 - 2) == 0 ? s[0] * s[1] : 0);
    Console.WriteLine(details.Sum());
}

async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var details = lines
        .Select(s => s.Split(": ").Select(int.Parse).ToArray()).ToArray();
    var i = 0;
    while (true)
    {
        if (details.All(s => (s[0] + i) % (s[1] * 2 - 2) != 0))
        {
            Console.WriteLine(i);
            break;
        }
        i++;
    }

}

