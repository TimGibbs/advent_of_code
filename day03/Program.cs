// See https://aka.ms/new-console-template for more information

await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var sum = 0;
    foreach (var line in lines)
    {
        var firstDigit = 0;
        var firstPos = 0;
        var secondDigit = 0;
        for (int i = 0; i < line.Length-1; i++)
        {
            var x = line[i]-'0';
            if (x == 9)
            {
                firstDigit = x;
                firstPos = i;
                break;
            }
            else if(x > firstDigit)
            {
                firstDigit = x;
                firstPos = i;
            }
        }

        for (var i = firstPos + 1; i < line.Length; i++)
        {
            var x = line.ToArray()[i]-'0';
            if (x == 9)
            {
                secondDigit = 9;
                break;
            }
            else if(x> secondDigit)
            {
                secondDigit = x;
            }
        }
        var val = firstDigit*10 +secondDigit;;
        sum += val;
    }
    Console.WriteLine(sum);
}

async Task Part2()
{
    var n = 12;
    var lines = await File.ReadAllLinesAsync("input.txt");
    long sum = 0;
    foreach (var line in lines)
    {
        var digits = new int[n];
        var pos = -1;
        for (var k = 0; k < n; k++)
        {
            for (var i = pos + 1; i <= line.Length - (n - k); i++)
            {
                var x = line[i]-'0';
                if (x == 9)
                {
                    digits[k] = x;
                    pos = i;
                    break;
                }
                else if(x > digits[k])
                {
                    digits[k] = x;
                    pos = i;
                }
            }
        }
        var val = long.Parse(string.Concat(digits));
        sum += val;
    }
    Console.WriteLine(sum);
}