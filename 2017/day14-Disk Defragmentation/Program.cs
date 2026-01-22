// @algorithm Disk Defragmentation Analysis
// @category Hashing / Grid Traversal
// @input Base key string
// @hash
//   Knot Hash (hexadecimal, 128-bit)
// @problem
//   Build a 128x128 binary grid from hashed rows
// @parts
//   Part1: Count total used bits in the grid
//   Part2: Count connected regions of used bits
// @model
//   - Each row = KnotHash($"{key}-{row}")
//   - Each hex digit expands to 4 bits (MSB → LSB)
// @technique
//   - Bit counting via nibble lookup table
//   - Flood fill (DFS using stack) for region counting
// @adjacency
//   - 4-directional (up, down, left, right)
// @data-structures
//   - bool[128,128] grid
//   - bool[128,128] visited
//   - Stack<(int r, int c)> for flood fill
// @complexity
//   Time:
//     Part1: O(128 * hash_length)
//     Part2: O(128 * 128)
//   Space: O(128 * 128)
// @notes
//   - Regions are maximal connected components of set bits
//   - Flood fill avoids recursion to prevent stack overflow

await Part1();
await Part2();
return;

async Task Part1()
{
    var line = await File.ReadAllTextAsync("input.txt");
    var key = line.Trim();
    var used = 0;
    
    for (int i = 0; i < 128; i++)
    {
        var hash = Logic.KnotHash($"{key}-{i}");

        foreach (var c in hash)
        {
            var v = c <= '9' ? c - '0' : c - 'a' + 10;
            used += new[] { 0,1,1,2,1,2,2,3,1,2,2,3,2,3,3,4 }[v];
        }
    }

    Console.WriteLine(used);
}

async Task Part2()
{
    var key = (await File.ReadAllTextAsync("input.txt")).Trim();

    var grid = new bool[128, 128];

    for (int row = 0; row < 128; row++)
    {
        var hash = Logic.KnotHash($"{key}-{row}");

        var col = 0;
        foreach (var c in hash)
        {
            var value = c <= '9' ? c - '0' : c - 'a' + 10;
            
            for (int bit = 3; bit >= 0; bit--)
            {
                grid[row, col++] = ((value >> bit) & 1) == 1;
            }
        }
    }

    Console.WriteLine(CountRegions(grid));
}

static int CountRegions(bool[,] grid)
{
    var rows = grid.GetLength(0);
    var cols = grid.GetLength(1);
    var visited = new bool[rows, cols];

    var regions = 0;

    for (int r = 0; r < rows; r++)
    {
        for (int c = 0; c < cols; c++)
        {
            if (grid[r, c] && !visited[r, c])
            {
                FloodFill(grid, visited, r, c);
                regions++;
            }
        }
    }

    return regions;
}

static void FloodFill(bool[,] grid, bool[,] visited, int sr, int sc)
{
    var rows = grid.GetLength(0);
    var cols = grid.GetLength(1);

    var stack = new Stack<(int r, int c)>();
    stack.Push((sr, sc));
    visited[sr, sc] = true;

    while (stack.Count > 0)
    {
        var (r, c) = stack.Pop();

        foreach (var (nr, nc) in new[]
                 {
                     (r - 1, c),
                     (r + 1, c),
                     (r, c - 1),
                     (r, c + 1)
                 })
        {
            if (nr < 0 || nr >= rows || nc < 0 || nc >= cols)
                continue;

            if (!visited[nr, nc] && grid[nr, nc])
            {
                visited[nr, nc] = true;
                stack.Push((nr, nc));
            }
        }
    }
}
