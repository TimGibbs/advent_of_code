// See https://aka.ms/new-console-template for more information

string[] _banned = { "ab", "cd", "pq", "xy" };

char[] _vowels = { 'a', 'e', 'i', 'o', 'u' };

await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");

    var result = lines.Sum(Check);
    Console.WriteLine(result);
}

int Check(string str)
{
    var vowelCount = 0;
    var hasDouble = false;

    if (_vowels.Contains(str[0]))
    {
        vowelCount++;
    }

    for (var i = 1; i < str.Length; i++)
    {
        var c = str[i];

        if (_vowels.Contains(c))
        {
            vowelCount++;
        }

        if (c == str[i - 1])
        {
            hasDouble = true;
        }

        foreach (var s in _banned)
        {
            if (str[i - 1] == s[0] && str[i] == s[1]) return 0;
        }
    }

    return vowelCount >= 3 && hasDouble ? 1 : 0;
}

async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");

    var result = lines.Sum(Check2);
    Console.WriteLine(result);
}

int Check2(string str)
{
    var a = false;
    var b = false;

    for (var i = 0; i < str.Length - 1; i++)
    {
        if (i < str.Length - 2 && str[i] == str[i + 2])
            a = true;

        for (var j  = i + 2; j < str.Length - 1; j++)
        {
            if (str[i] == str[j] && str[i + 1] == str[j + 1])
            {
                b = true;
                break;
            }
        }

        if (a && b) return 1;
    }

    return 0;
}
