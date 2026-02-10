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