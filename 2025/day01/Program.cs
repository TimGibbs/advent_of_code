// See https://aka.ms/new-console-template for more information

await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    
    var pos = 50;
    var count = 0;
    foreach (var line in lines)
    {
        var dir = line[0] == 'R' ? 1 : -1;
        var steps = int.Parse(line[1..]);
        pos += steps * dir;
        while (pos is < 0 or > 99)
        {
            if (pos < 0) pos = 100 + pos;
            if(pos > 99) pos -= 100;
        }
        //Console.WriteLine(pos);
        if(pos == 0) count++;
    }
    Console.WriteLine(count);
}

async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    
    var pos = 50;
    var count = 0;
    foreach (var line in lines)
    {
        bool ignoreFirstMove = pos == 0;
        var dir = line[0] == 'R' ? 1 : -1;
        var steps = int.Parse(line[1..]);
        pos += steps * dir;
        while (pos is < 0 or > 99)
        {
            if (pos < 0)
            {
                pos = 100 + pos;
                if (ignoreFirstMove)
                { 
                    ignoreFirstMove = false;
                }
                else
                {
                    count++;
                }
            }

            if (pos > 99)
            {
                pos -= 100;
                if(pos!=0) count++;
            }
        }
        //Console.WriteLine(pos);
        if(pos == 0) count++;
        //Console.WriteLine(count);
    }
    Console.WriteLine(count);
}