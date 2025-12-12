using System.Numerics;

await Part1();

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var (shapes, trees) = ParseShapesAndTrees(lines);

    // Precompute permutation masks
    foreach (var shape in shapes)
        shape.PermutationMasks = shape.Permutations.Select(ToMask).ToArray();

    var answer = trees.Sum(tree => SolveTree(tree, shapes) ? 1 : 0);
    Console.WriteLine(answer);
}

(Shape[] shapes, Tree[] trees) ParseShapesAndTrees(string[] strings)
{
    var list = new List<Shape>();
    var trees = new List<Tree>();
    var shape = new Shape();
    var x = 0;

    foreach (var line in strings)
    {
        if (string.IsNullOrEmpty(line))
        {
            shape.Permutations = Extensions.GetAllTransformations(shape.InitialShape).ToArray();
            list.Add(shape);
            shape = new Shape();
            x = 0;
            continue;
        }

        if (line[1] == ':')
        {
            shape.Position = int.Parse(line[..1]);
            continue;
        }

        if (line[0] is '.' or '#')
        {
            if (line.Length > 3) throw new Exception("Invalid shape");
            for (var i = 0; i < line.Length; i++)
                shape.InitialShape[x * 3 + i] = line[i] == '#';
            x++;
            continue;
        }

        // Tree line
        var q = line.Split(' ');
        var size = q[0][..^1].Split('x');
        trees.Add(new Tree(int.Parse(size[0]), int.Parse(size[1]), q[1..].Select(int.Parse).ToArray()));
    }

    return (list.ToArray(), trees.ToArray());
}

static bool SolveTree(Tree tree, Shape[] shapes)
{
    var grid = new ulong[tree.Y];

    // Flatten required shapes
    var requiredShapes = new List<Shape>();
    for (var i = 0; i < tree.Requirement.Length; i++)
        for (var j = 0; j < tree.Requirement[i]; j++)
            requiredShapes.Add(shapes[i]);

    // Sort descending by number of blocks (dense first)
    requiredShapes = requiredShapes
        .Select(s => (s, CountBits1(s.InitialShape)))
        .OrderByDescending(x => x.Item2)
        .Select(x => x.s)
        .ToList();

    var totalBlocks = requiredShapes.Sum(s => CountBits1(s.InitialShape));

    return Solve(grid, tree.X, tree.Y, requiredShapes.ToArray(), 0, totalBlocks);
}

static bool Solve(ulong[] grid, int width, int height, Shape[] shapes, int index, int remainingBlocks)
{
    // Prune if remaining blocks cannot fit
    if (remainingBlocks > width * height - CountGrid(grid))
        return false;

    if (index == shapes.Length)
        return true;

    var shape = shapes[index];

    foreach (var mask in shape.PermutationMasks)
    {
        for (var y = 0; y <= height - 3; y++)
        {
            for (var x = 0; x <= width - 3; x++)
            {
                if (!CanPlace(grid, mask, x, y)) continue;

                Place(grid, mask, x, y, true);

                if (Solve(grid, width, height, shapes, index + 1, remainingBlocks - CountBits(mask)))
                    return true;

                Place(grid, mask, x, y, false);
            }
        }
    }

    return false;
}

static int CountBits1(bool[] shape)
{
    var count = 0;
    foreach (var b in shape)
        if (b) count++;
    return count;
}

static int CountBits(ulong mask)
{
    return BitOperations.PopCount(mask);
}

static int CountGrid(ulong[] grid)
{
    var count = 0;
    foreach (var row in grid)
        count += BitOperations.PopCount(row);
    return count;
}

// Check if mask can be placed at (ox, oy)
static bool CanPlace(ulong[] grid, ulong mask, int ox, int oy)
{
    for (var dy = 0; dy < 3; dy++)
    {
        var rowMask = ((mask >> (dy * 3)) & 0b111UL) << ox;
        if ((grid[oy + dy] & rowMask) != 0) return false;
    }
    return true;
}

// Place or remove mask at (ox, oy)
static void Place(ulong[] grid, ulong mask, int ox, int oy, bool value)
{
    for (var dy = 0; dy < 3; dy++)
    {
        var rowMask = ((mask >> (dy * 3)) & 0b111UL) << ox;
        if (value) grid[oy + dy] |= rowMask;
        else grid[oy + dy] &= ~rowMask;
    }
}

// Convert bool[9] to 9-bit mask
static ulong ToMask(bool[] shape)
{
    ulong mask = 0;
    for (var i = 0; i < 9; i++) if (shape[i]) mask |= 1UL << i;
    return mask;
}

record Shape()
{
    public int Position { get; set; }
    public bool[] InitialShape { get; set; } = new bool[9];

    public bool[][] Permutations { get; set; }
    public ulong[] PermutationMasks { get; set; }
}

record Tree(int X, int Y, int[] Requirement);

public static class Extensions
{
    public static IEnumerable<bool[]> GetAllTransformations(bool[] shape)
    {
        if (shape.Length != 9) throw new ArgumentException("Shape must be a bool[9].");

        var seen = new HashSet<string>();

        foreach (var flip in new Func<bool[], bool[]>[] { Identity, FlipHorizontal })
        {
            var current = flip(shape);

            var key = Serialize(current);
            if (seen.Add(key))
                yield return (bool[])current.Clone();

            for (var i = 0; i < 3; i++)
            {
                current = Rotate90(current);
                key = Serialize(current);
                if (seen.Add(key))
                    yield return (bool[])current.Clone();
            }
        }
    }

    private static string Serialize(bool[] arr) => string.Concat(arr.Select(b => b ? '1' : '0'));
    private static bool[] Identity(bool[] s) => (bool[])s.Clone();
    private static bool[] FlipHorizontal(bool[] s) => new[]
    {
        s[2], s[1], s[0],
        s[5], s[4], s[3],
        s[8], s[7], s[6]
    };
    private static bool[] Rotate90(bool[] s) => new[]
    {
        s[6], s[3], s[0],
        s[7], s[4], s[1],
        s[8], s[5], s[2]
    };
}
