// @algorithm BruteForceHashing
// @category Cryptography / Search
// @input
//   Door ID string
// @technique
//   MD5 Hashing
//   Prefix-Zero Matching (leading hex zeroes)
//   Incremental Brute Force
// @variant
//   Part1: Sequential Password Construction
//   Part2: Position-Based Password Construction
// @data-structure
//   byte[]
//   char[]
// @bitwise
//   Nibble Masking (0xF0, 0x0F)
// @complexity
//   Time: O(k) where k = hashes tested
//   Space: O(1)

await Part1();
await Part2();
return;

async Task Part1()
{
    var line = await File.ReadAllTextAsync("input.txt");
    var i = 0;
    var found = 0;
    var pass = "";
    while (found < 8)
    {
        var step = System.Text.Encoding.ASCII.GetBytes($"{line}{++i}");
        var hash = System.Security.Cryptography.MD5.HashData(step);
        if (hash[0] == 0x00
            && hash[1] == 0x00
            && (hash[2] & 0xF0) == 0x00)
        {
            var sixthHexValue = hash[2] & 0x0F;   // value 0–15
            var sixthHexChar = sixthHexValue.ToString("x")[0];
            pass += sixthHexChar;
            found++;
        }

    }
    Console.WriteLine(pass);
}

async Task Part2()
{
    var line = await File.ReadAllTextAsync("input.txt");
    var i = 0;
    var found = 0;
    var pass = new char[8];
    Array.Fill(pass,'z');
    while (pass.Any(x=>x=='z'))
    {
        var step = System.Text.Encoding.ASCII.GetBytes($"{line}{++i}");
        var hash = System.Security.Cryptography.MD5.HashData(step);
        if (hash[0] == 0x00
            && hash[1] == 0x00
            && (hash[2] & 0xF0) == 0x00)
        {
            var sixthHexValue = hash[2] & 0x0F;   // value 0–15
            if (sixthHexValue < 8 && pass[sixthHexValue] == 'z')
            {
                var seventhHexValue = hash[3] & 0xF0;   // value 0–15
                var seventhHexChar = seventhHexValue.ToString("x")[0];
                pass[sixthHexValue] = seventhHexChar;
            }
        }

    }
    Console.WriteLine(string.Join("",pass));
}
