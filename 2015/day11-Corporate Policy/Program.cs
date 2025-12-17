// @algorithm PasswordIncrementSimulation
// @category StringProcessing / ConstraintChecking
// @data-structure HashSet, Span
// @complexity Time: O(n), Space: O(n)
// @variant Part1: NextValidPassword1, Part2: NextValidPassword2

await Part1();
return;

async Task Part1()
{
    var line = await File.ReadAllTextAsync("input.txt");
    
    var z = Increment(line);
    while (!(Check1(z) && Check2(z) && Check3(z))) z = Increment(z);
    Console.WriteLine(z);
    z = Increment(z);
    while (!(Check1(z) && Check2(z) && Check3(z))) z = Increment(z);
    Console.WriteLine(z);
}

string Increment(ReadOnlySpan<char> input)
{
    Span<char> buffer = stackalloc char[input.Length + 1];

    input.CopyTo(buffer.Slice(1));

    int i = buffer.Length - 1;

    while (i > 0)
    {
        if (buffer[i] != 'z')
        {
            buffer[i]++;
            return new string(buffer.Slice(1));
        }

        buffer[i] = 'a';
        i--;
    }

    buffer[0] = 'a';
    return new string(buffer);
}


bool Check1(ReadOnlySpan<char> pass)
{
    var count = 0;
    for (var i = 1; i < pass.Length; i++)
    {
        if (pass[i] == pass[i - 1] + 1)
        {
            count++;
            if(count >= 2) return true;
            continue;
        }
        count = 0;
    }
    return count >= 3;
}


bool Check2(ReadOnlySpan<char> pass)
{
    foreach (var c in pass)
    {
        if (c is 'i' or 'o' or 'l') return false;
    }
    return true;
}

bool Check3(ReadOnlySpan<char> pass)
{
    var doublesSeen = new HashSet<char>(pass.Length/2);
    for (var i = 0; i < pass.Length - 1; i++)
    {
        if (pass[i] == pass[i + 1])
        {
            if (doublesSeen.Add(pass[i]) && doublesSeen.Count > 1) return true;
        }
    }
    return false;
}