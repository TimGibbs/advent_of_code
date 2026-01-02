// @algorithm FrequencyAnalysis + CaesarCipher
// @category Parsing / Cryptography
// @input
//   Encrypted room strings: name-sectorID[checksum]
// @technique
//   Character Frequency Counting
//   Sorting with Tie-Break (count desc, char asc)
//   Caesar Shift (rotation cipher)
// @variant
//   Part1: ValidRoomDetection + SectorID Sum
//   Part2: Decryption of Valid Rooms
// @data-structure
//   Dictionary<char,int>
//   ReadOnlySpan<char>
//   StringBuilder
// @complexity
//   Part1 Time: O(n * k log k)   // k = distinct letters
//   Part2 Time: O(n * m)         // m = line length
//   Space: O(k)


using System.Text;

await Part1();
await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    Console.WriteLine(lines.Where(x=>IsValid(x)).Sum(x=>GetShift(x)));
}

int GetShift(ReadOnlySpan<char> line)
{
    var j = int.Parse(line[(line.LastIndexOf('-')+1) .. (line.IndexOf('['))]);
    return j;
}

bool IsValid(ReadOnlySpan<char> line)
{
    var count = new Dictionary<char, int>();
    foreach (var c in line)
    {
        if(c == '-') continue;
        if (c is >= 'a' and <= 'z')
        {
            count[c] = count.TryGetValue(c, out var x) ? ++x : 1;
            continue;
        }
        break;
    }

    var check = line[(line.IndexOf('[')+1) .. ^1];
    var val = count.OrderByDescending(x => x.Value).ThenBy(x=>x.Key).Take(5).Select(x => x.Key).ToArray().AsSpan();
    for (var index = 0; index < val.Length; index++)
    {
        var c = val[index];
        var d = check[index];
        if (c != d) return false;
    }

    return true;
}

async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var valid = lines
        .Where(x => IsValid(x))
        .Select(x =>
        {
            var shift = GetShift(x);
            var s = new StringBuilder();

            foreach (var c in x)
            {
                s.Append(Shift(c, shift));
            }

            return s.ToString();
        })
        .ToArray();
    foreach (var s in valid)
    {
        Console.WriteLine(s);
    }
}

char Shift(char c, int shift)
{
    if (c == '-') return ' ';
    if (c is >= '1' and <= '9') return c;
    var a = c - 'a';
    var b = (a + shift) % 26;
    return (char)(b + 'a');
}