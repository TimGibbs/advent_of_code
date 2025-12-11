// See https://aka.ms/new-console-template for more information

await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    
    var dict = lines.Select(s=>s.Split(' ')).ToDictionary(k=>k[0][..^1],v=>v[1..]);

    var c = FindOut("you", dict);
    Console.WriteLine(c);
}

int FindOut(string place, Dictionary<string, string[]> dict)
{
    if(dict.TryGetValue(place, out var outs) && outs.Contains("out")) return 1;

    if (outs != null) return outs.Sum(o => FindOut(o, dict));
    throw new ArgumentException("device not in list");
}

async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    
        
    var dict = lines.Select(s=>s.Split(' ')).ToDictionary(k=>k[0][..^1],v=>v[1..]);

    var c = FindOutViaDacFft("svr", dict, false,false);
    Console.WriteLine(c);
    
}

long FindOutViaDacFft(string place, Dictionary<string, string[]> dict, bool dac, bool fft, 
    Dictionary<(string,bool,bool),long>? memo = null)
{
    memo ??= new Dictionary<(string,bool,bool), long>();
    if (memo.TryGetValue((place, dac, fft), out var mem)) return mem;
    switch (place)
    {
        case "out":
            memo[(place, dac, fft)] = 0; return 0;
        case "fft": fft = true; break;
        case "dac": dac = true; break;
    }

    if (dict.TryGetValue(place, out var outs) && outs.Contains("out") && fft && dac)
    {
        memo[(place, dac, fft)] = 1;
        return 1;
    }

    if (outs == null) throw new ArgumentException("device not in list");
    var l = outs.Sum(o => FindOutViaDacFft(o, dict, dac, fft, memo));
    memo[(place, dac, fft)] = l;
    return l;
}