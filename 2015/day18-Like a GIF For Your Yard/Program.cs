
await Part1();
await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var state = new bool[100, 100]; 
    
    for (var y = 0; y < lines.Length; y++)
    {
        var line = lines[y];
        for (var x = 0; x < line.Length; x++)
        {
            var ch = line[x];
            state[x, y] = ch == '#';
        }
    }
    
    for (int i = 0; i < 100; i++)
    {
        var nextState = new bool[100, 100];

        for (int x = 0; x < 100; x++)
        {
            for (int y = 0; y < 100; y++)
            {
                var n = Neighbours(x, y);
                nextState[x, y] =
                    StateUpdate(state[x, y], n.Select(t => state[t.x, t.y]));
            }
        }

        state = nextState;

    }

    var count = 0;
    foreach (var b in state)
    {
        if (b) count++;
    }
    Console.WriteLine(count);
}

IEnumerable<(int x, int y)> Neighbours(int x, int y)
{
    const int size = 100;

    for (int dx = -1; dx <= 1; dx++)
    for (int dy = -1; dy <= 1; dy++)
    {
        if (dx == 0 && dy == 0)
            continue;

        int nx = x + dx;
        int ny = y + dy;

        if (nx >= 0 && nx < size && ny >= 0 && ny < size)
            yield return (nx, ny);
    }
}


async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var state = new bool[100, 100];
    
    for (var y = 0; y < lines.Length; y++)
    {
        var line = lines[y];
        for (var x = 0; x < line.Length; x++)
        {
            var ch = line[x];
            state[x, y] = ch == '#';
        }
    }
    
    state[0, 0] = true;
    state[0, 99] = true;
    state[99, 0] = true;
    state[99, 99] = true;
    
    for (int i = 0; i < 100; i++)
    {
        var nextState = new bool[100, 100];

        for (int x = 0; x < 100; x++)
        {
            for (int y = 0; y < 100; y++)
            {
                if (x is 0 or 99 && y is 0 or 99) {nextState[x, y] = true; continue;}
                var n = Neighbours(x, y);
                nextState[x, y] =
                    StateUpdate(state[x, y], n.Select(t => state[t.x, t.y]));
            }
        }

        state = nextState;

    }

    var count = 0;
    foreach (var b in state)
    {
        if (b) count++;
    }
    Console.WriteLine(count);
}

bool StateUpdate(bool currentState, IEnumerable<bool> neighbours)
{
    var neighboursCount = neighbours.Count(x => x);
    if (currentState)
    {
        return neighboursCount is 2 or 3;
    }

    return neighboursCount is 3;
}