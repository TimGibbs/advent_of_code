// @algorithm Permutation Dance Simulation
// @category Permutation / Cycle Detection
// @input Comma-separated dance instructions
// @programs Initial state: "abcdefghijklmnop"
// @moves
//   sX  → Spin: rotate right by X
//   xA/B → Exchange: swap positions A and B
//   pA/B → Partner: swap programs by name
// @parts
//   Part1: Apply one full dance sequence
//   Part2: Apply dance 1,000,000,000 times (cycle optimized)
// @model
//   - State represented as char[16]
//   - Dance operations are permutations of positions/labels
// @technique
//   - Direct simulation for one round
//   - Cycle detection using Dictionary<string, int>
//   - Fast-forward using cycle length and modular arithmetic
// @data-structures
//   - char[] for program order
//   - Dictionary<string, int> for seen states
//   - Temporary buffer for spin operation
// @complexity
//   Part1: O(m)
//   Part2: O(cycle_length * m)
//   Space: O(cycle_length)
//   (m = number of instructions)
// @notes
//   - Entire dance sequence forms a permutation cycle
//   - Once a repeated state is found, remaining iterations are computed via modulo

await Part1();
await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllTextAsync("input.txt");
    var instructions = lines.Split(',');

    var alpha = "abcdefghijklmnop".ToCharArray();
    var buffer = new char[16];
    foreach (var instruction in instructions)
    {
        switch (instruction[0])
        {
            case 's':
                var num = int.Parse(instruction[1..]) % 16;
                Array.Copy(alpha, 16 - num, buffer, 0, num);
                Array.Copy(alpha, 0, buffer, num, 16 - num);
                Array.Copy(buffer, alpha, 16);

                break;
            case 'x':
                var x = instruction[1..].Split('/').Select(int.Parse).ToArray();
                (alpha[x[0]], alpha[x[1]]) = (alpha[x[1]], alpha[x[0]]);
                break;
            case 'p': 
                var p = instruction[1..].Split('/');
                var a = alpha.IndexOf(p[0]);
                var b = alpha.IndexOf(p[1]);
                (alpha[a], alpha[b]) = (alpha[b], alpha[a]);
                break;
        }
    }
    Console.WriteLine(new string(alpha));
}

async Task Part2()
{
    var lines = await File.ReadAllTextAsync("input.txt");
    var instructions = lines.Split(',');

    var seen = new Dictionary<string, int>();
    var alpha = "abcdefghijklmnop".ToCharArray();
    var buffer = new char[16];

    var i = 0;
    while (true)
    {
        var key = new string(alpha);
        if (seen.TryGetValue(key, out var firstSeen))
        {
            var cycleLength = i - firstSeen;
            var remaining = (1_000_000_000 - firstSeen) % cycleLength;

            alpha = seen
                .First(kvp => kvp.Value == firstSeen + remaining)
                .Key
                .ToCharArray();
            break;
        }

        seen[key] = i;

        // apply one full dance
        foreach (var instruction in instructions)
        {
            switch (instruction[0])
            {
                case 's':
                    var num = int.Parse(instruction[1..]) % 16;
                    Array.Copy(alpha, 16 - num, buffer, 0, num);
                    Array.Copy(alpha, 0, buffer, num, 16 - num);
                    Array.Copy(buffer, alpha, 16);
                    break;

                case 'x':
                    var x = instruction[1..].Split('/').Select(int.Parse).ToArray();
                    (alpha[x[0]], alpha[x[1]]) = (alpha[x[1]], alpha[x[0]]);
                    break;

                case 'p':
                    var p = instruction[1..].Split('/');
                    var a = Array.IndexOf(alpha, p[0][0]);
                    var b = Array.IndexOf(alpha, p[1][0]);
                    (alpha[a], alpha[b]) = (alpha[b], alpha[a]);
                    break;
            }
        }

        i++;
    }

    Console.WriteLine(new string(alpha));
}