// @algorithm GridSimulation
// @category Simulation / ArrayManipulation
// @data-structure Array
// @complexity Time: O(n*m), Space: O(n*m)  // n=m=1000
// @variant Part1: BooleanLightsToggle, Part2: IncrementalBrightness

await Part1();
await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    
    var state = new bool[1000 * 1000];
    foreach (var line in lines)
    {
        HandleLine(line, ref state);
    }
    Console.WriteLine(state.Count(x => x));
    

}

async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var state = new int[1000 * 1000];
    foreach (var line in lines)
    {
        HandleLine2(line, ref state);
    }
    Console.WriteLine(state.Sum());
}

static void HandleLine(string str, ref bool[] state)
{
    string[] split = str.Split(' ');
    if (split[0] == "toggle")
    {
        int[] a = split[1].Split(',').Select(int.Parse).ToArray();
        int[] b = split[3].Split(',').Select(int.Parse).ToArray();

        for (int i = a[0]; i <= b[0]; i++)
        {
            for (int j = a[1]; j <= b[1]; j++)
            {
                state[i * 1000 + j] = !state[i * 1000 + j];
            }
        }
    }

    if (split[0] == "turn")
    {
        bool val = split[1] == "on";
        int[] a = split[2].Split(',').Select(int.Parse).ToArray();
        int[] b = split[4].Split(',').Select(int.Parse).ToArray();

        for (int i = a[0]; i <= b[0]; i++)
        {
            for (int j = a[1]; j <= b[1]; j++)
            {
                state[i * 1000 + j] = val;
            }
        }
    }
}

static void HandleLine2(string str, ref int[] state)
{
    string[] split = str.Split(' ');
    if (split[0] == "toggle")
    {
        int[] a = split[1].Split(',').Select(int.Parse).ToArray();
        int[] b = split[3].Split(',').Select(int.Parse).ToArray();
        
        for(int x = a[0]; x<=b[0]; x++)
            for(int y = a[1]; y<=b[1]; y++) 
                state[y*1000+x]+=2;
    }

    if (split[0] == "turn")
    {
        bool val = split[1] == "on";
        int[] a = split[2].Split(',').Select(int.Parse).ToArray();
        int[] b = split[4].Split(',').Select(int.Parse).ToArray();
        
        for(int x = a[0]; x<=b[0]; x++)
            for(int y = a[1]; y<=b[1]; y++) 
                state[y*1000+x]  = Math.Max(0, state[y*1000+x] + (val?1:-1));
    }
}
