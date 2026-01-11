// @algorithm Scramble / Unscramble String
// @category String Manipulation / Virtual Machine
// @input List of instructions in "swap", "rotate", "reverse", "move" formats
// @state Character array representing string
// @execution-model Sequential / reversible
// @technique
//   Part1: Apply instructions in order
//   Part2: Apply inverse instructions in reverse order
// @features
//   - Swap by position or letter
//   - Rotate left/right or based on letter position
//   - Reverse substring
//   - Move position
// @data-structures
//   char[] for string state
//   Regex for instruction parsing
// @complexity
//   Time: O(L * N) where L = length of string, N = number of instructions
//   Space: O(L)

using System.Text.RegularExpressions;

string regex1 = @"swap position (\d+) with position (\d+)";
string regex2 = @"swap letter ([a-z]) with letter ([a-z])";
string regex3 = @"rotate\s+(left|right)\s+(\d+)\s+step[s]?";
string regex4 = @"rotate based on position of letter ([a-z])";
string regex5 = @"reverse positions (\d+) through (\d+)";
string regex6 = @"move position (\d+) to position (\d+)";


await Part1();
await Part2();
return;


async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var initial = "abcdefgh".ToCharArray();
    foreach (var line in lines)
    {
        if (Regex.TryMatch(line, regex1, out var swapP))
        {
            int x = int.Parse(swapP.Groups[1].Value);
            int y = int.Parse(swapP.Groups[2].Value);
            (initial[x], initial[y]) = (initial[y], initial[x]);
            continue;
        }
        if (Regex.TryMatch(line, regex2, out var swapL))
        {
            char x = swapL.Groups[1].Value.ToCharArray()[0];
            char y = swapL.Groups[2].Value.ToCharArray()[0];
            for (var index = 0; index < initial.Length; index++)
            {
                if (initial[index] == x)
                    initial[index] = y;
                else if (initial[index] == y)
                    initial[index] = x;
            }
            continue;
        }
        if (Regex.TryMatch(line, regex3, out var rotate))
        {
            string direction = rotate.Groups[1].Value; 
            int steps = int.Parse(rotate.Groups[2].Value);
            switch (direction)
            {
                case "left":
                    RotateLeft(initial,steps);
                    continue;
                case "right":
                    RotateRight(initial, steps);
                    continue;
            }
        }
        if (Regex.TryMatch(line, regex4, out var rotateP))
        {
            char p = rotateP.Groups[1].Value[0];
            int index = Array.IndexOf(initial, p);
            int rotations = 1 + index;
            if (index >= 4) rotations += 1;
            RotateRight(initial, rotations);
            continue;
        }
        if (Regex.TryMatch(line, regex5, out var reverse))
        {
            int x = int.Parse(reverse.Groups[1].Value);
            int y = int.Parse(reverse.Groups[2].Value);
            var length = Math.Abs(y - x) + 1;
            var start = Math.Min(x, y);
            initial.AsSpan(start, length).Reverse();
            continue;
        }
        if (Regex.TryMatch(line, regex6, out var move))
        {
            int x = int.Parse(move.Groups[1].Value);
            int y = int.Parse(move.Groups[2].Value);

            var a = initial[x];

            initial = [..initial[..x], ..initial[(x + 1)..]];

            initial = [..initial[..y], a, ..initial[y..]];
            continue;
        }
    }
    Console.WriteLine(string.Join("",initial));
}

async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var initial = "fbgdceah".ToCharArray();

    for (int lineIndex = lines.Length - 1; lineIndex >= 0; lineIndex--)
    {
        var line = lines[lineIndex];

        if (Regex.TryMatch(line, regex1, out var swapP))
        {
            int x = int.Parse(swapP.Groups[1].Value);
            int y = int.Parse(swapP.Groups[2].Value);
            (initial[x], initial[y]) = (initial[y], initial[x]);
            continue;
        }

        if (Regex.TryMatch(line, regex2, out var swapL))
        {
            char x = swapL.Groups[1].Value[0];
            char y = swapL.Groups[2].Value[0];
            for (int index = 0; index < initial.Length; index++)
            {
                if (initial[index] == x)
                    initial[index] = y;
                else if (initial[index] == y)
                    initial[index] = x;
            }
            continue;
        }

        if (Regex.TryMatch(line, regex3, out var rotate))
        {
            string direction = rotate.Groups[1].Value;
            int steps = int.Parse(rotate.Groups[2].Value);

            // invert left/right
            switch (direction)
            {
                case "left":
                    RotateRight(initial, steps);
                    continue;
                case "right":
                    RotateLeft(initial, steps);
                    continue;
            }
        }

        if (Regex.TryMatch(line, regex4, out var rotateP))
        {
            char p = rotateP.Groups[1].Value[0];

            // brute-force all left rotations to invert
            var found = false;
            for (int i = 0; i < initial.Length; i++)
            {
                char[] candidate = (char[])initial.Clone();
                RotateLeft(candidate, i); // try left rotation

                // simulate forward rotate-based-on-position
                int idx = Array.IndexOf(candidate, p);
                int rotations = 1 + idx;
                if (idx >= 4) rotations += 1;
                RotateRight(candidate, rotations);

                if (candidate.SequenceEqual(initial))
                {
                    // this is the correct inversion
                    RotateLeft(initial, i);
                    found = true;
                    break;
                }
            }

            if (!found) throw new Exception("Failed to invert rotate based on position");
            continue;
        }

        if (Regex.TryMatch(line, regex5, out var reverse))
        {
            int x = int.Parse(reverse.Groups[1].Value);
            int y = int.Parse(reverse.Groups[2].Value);
            int length = Math.Abs(y - x) + 1;
            int start = Math.Min(x, y);
            initial.AsSpan(start, length).Reverse();
            continue;
        }

        if (Regex.TryMatch(line, regex6, out var move))
        {
            int y = int.Parse(move.Groups[1].Value);
            int x = int.Parse(move.Groups[2].Value); 

            var a = initial[x];
            initial = [..initial[..x], ..initial[(x + 1)..]];
            initial = [..initial[..y], a, ..initial[y..]];
            continue;
        }
    }

    Console.WriteLine(string.Join("", initial));
}

static void RotateLeft(char[] arr, int n)
{
    int len = arr.Length;
    n %= len; // in case n > length
    if (n == 0) return;

    char[] temp = new char[n];
    Array.Copy(arr, 0, temp, 0, n);           // save first n elements
    Array.Copy(arr, n, arr, 0, len - n);     // shift rest left
    Array.Copy(temp, 0, arr, len - n, n);    // put saved at end
}

static void RotateRight(char[] arr, int n)
{
    int len = arr.Length;
    n %= len;
    if (n == 0) return;

    char[] temp = new char[n];
    Array.Copy(arr, len - n, temp, 0, n);    // save last n elements
    Array.Copy(arr, 0, arr, n, len - n);     // shift rest right
    Array.Copy(temp, 0, arr, 0, n);          // put saved at start
}

public static class RegexStaticExtensions
{
    extension(Regex)
    {
        public static bool TryMatch(string input, string pattern, out Match match)
        {
            match = Regex.Match(input, pattern);
            return match.Success;
        }
    }
}


