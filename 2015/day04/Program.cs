// See https://aka.ms/new-console-template for more information

await Part2();
return;

async Task Part1()
{
    var line = await File.ReadAllTextAsync("input.txt");

    var i = -1;
    var stop = false;
    using var md5 = System.Security.Cryptography.MD5.Create();
    while (!stop)
    {
        var step = System.Text.Encoding.ASCII.GetBytes($"{line}{++i}");
        var hash = md5.ComputeHash(step);

        if (hash[0] == 0x00
            && hash[1] == 0x00
            && (hash[2] & 0xF0) == 0x00)
        {
            stop = true;
        }
    }
    Console.WriteLine(i);
}

async Task Part2()
{
    var line = await File.ReadAllTextAsync("input.txt");

    var i = -1;
    var stop = false;
    using var md5 = System.Security.Cryptography.MD5.Create();
    while (!stop)
    {
        var step = System.Text.Encoding.ASCII.GetBytes($"{line}{++i}");
        var hash = md5.ComputeHash(step);

        if (hash[0] == 0x00
            && hash[1] == 0x00
            && hash[2] == 0x00)
        {
            stop = true;
        }
    }
    Console.WriteLine(i);

}