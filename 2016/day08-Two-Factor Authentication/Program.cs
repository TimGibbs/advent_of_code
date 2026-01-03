// @algorithm GridSimulation
// @category Cellular Automata / Display Emulation
// @input
//   Instruction list from text file (rect, rotate row, rotate column)
// @technique
//   2D Grid Flattened to 1D Array
//   Modular Indexing (wrap-around shifts)
// @operations
//   Rectangle Fill
//   Row Rotation
//   Column Rotation
// @data-structure
//   bool[] (width * height framebuffer)
//   Temporary buffers for shifts
// @paradigm
//   Imperative Simulation
//   Instruction Dispatch
// @variant
//   Part1: Count Lit Pixels
//   Part2: Render ASCII Display
// @complexity
//   Time: O(n * max(width, height))
//   Space: O(width * height)

const int width = 50;
const int height = 6;

await Part1();
await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var screen = new bool[width * height];
    foreach (var line in lines)
    {
        ApplyInstruction(ref screen, line);
    }
    Console.WriteLine(screen.Count(x=>x));
}

void Rect(ref bool[] screen, int x, int y)
{
    for (var i = 0; i < x; i++)
    {
        for (var j = 0; j < y; j++)
        {
            screen[i + j * width] = true;
        }
    }
}

void Row(ref bool[] screen, int row, int offset)
{
    offset %= width;
    if (offset == 0) return;

    var rowStart = row * width;
    var temp = new bool[width];
    
    for (var x = 0; x < width; x++)
    {
        var srcIndex = (x - offset + width) % width;
        temp[x] = screen[rowStart + srcIndex];
    }
    
    for (var x = 0; x < width; x++)
    {
        screen[rowStart + x] = temp[x];
    }
}

void Column(ref bool[] screen, int column, int offset)
{
    offset %= height;
    if (offset == 0) return;

    var temp = new bool[height];
    
    for (var y = 0; y < height; y++)
    {
        var srcRow = (y - offset + height) % height;
        temp[y] = screen[srcRow * width + column];
    }
    
    for (var y = 0; y < height; y++)
    {
        screen[y * width + column] = temp[y];
    }
}

void ApplyInstruction(ref bool[] screen, string line)
{
    if (line.StartsWith("rect "))
    {
        var span = line.AsSpan(5);
        var xPos = span.IndexOf('x');

        var rectWidth = int.Parse(span[..xPos]);
        var rectHeight = int.Parse(span[(xPos + 1)..]);

        Rect(ref screen, rectWidth, rectHeight);
        return;
    }

    if (line.StartsWith("rotate "))
    {
        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        var offset = int.Parse(parts[4]);

        if (parts[1] == "row")
        {
            var row = int.Parse(parts[2].AsSpan(2));
            Row(ref screen, row, offset);
            return;
        }

        if (parts[1] == "column")
        {
            var column = int.Parse(parts[2].AsSpan(2));
            Column(ref screen, column, offset);
            return;
        }
    }

    throw new ArgumentException($"Invalid instruction: {line}");
}

async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var screen = new bool[width * height];
    foreach (var line in lines)
    {
        ApplyInstruction(ref screen, line);
    }

    for (int j = 0; j < height; j++)
    {
        for (int i = 0; i < width; i++)
        {
            Console.Write((screen[i + j * width] ? "█" : " "));
        }
        Console.WriteLine();
    }
}