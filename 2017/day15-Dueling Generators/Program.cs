await Part1();
await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var a =  long.Parse(lines[0].Split(" ").Last());
    var b = long.Parse(lines[1].Split(" ").Last());
    var count = 0;
    for (int i = 0; i < 40_000_000; i++)
    {
        a = GenA(a);
        b = GenB(b);
        if ((a & 0xFFFF) == (b & 0xFFFF))
            count++;
    }
    Console.WriteLine(count);
    
}
const long div = 2147483647;
long GenA(long val) => (val * 16807) % div;
long GenB(long val) => (val * 48271) % div;


async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var a =  long.Parse(lines[0].Split(" ").Last());
    var b = long.Parse(lines[1].Split(" ").Last());
    var count = 0;
    for (int i = 0; i < 5_000_000; i++)
    {
        a = GenA2(a);
        b = GenB2(b);
        if ((a & 0xFFFF) == (b & 0xFFFF))
            count++;
    }
    Console.WriteLine(count);
}

long GenA2(long val)
{
    var temp = val;
    do
    {
        temp = (temp * 16807) % div;
    } while (temp % 4 != 0);
    return temp;
}
long GenB2(long val)
{
    var temp = val;
    do
    {
        temp = (temp * 48271) % div;
    } while (temp % 8 != 0);
    return temp;
}