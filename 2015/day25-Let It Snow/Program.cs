// @algorithm DiagonalIndexing / ModularSequence
// @category Math / Number Theory
// @data-structure None (scalar computation)
// @complexity
//   Time: O(n) where n is the computed diagonal index
//   Space: O(1)
// @technique ArithmeticSeries, CoordinateMapping, ModularExponentiation (iterative)
// @variant
//   Part1: GridCodeLookup (row/column to diagonal index)

await Part1();
return;

async Task Part1()
{
    var line = await File.ReadAllTextAsync("input.txt");
    var split = line.Split(' ');
    var row = int.Parse(split[16].TrimEnd(','));
    var col = int.Parse(split[18].TrimEnd('.'));
    var i = 20151125L;

    var b = RowStart(row);
    var g = ColCount(row, col);
    var x = b + g;

    for (int j = 2; j <= x; j++)
    {
        i = Update(i);
    }
    Console.WriteLine(i);
}


int RowStart(int n)
{
    return Enumerable.Range(1, n-1).Sum() + 1;
}

int ColCount(int row, int col)
{
    var c = (col-1) * row;
    var steps = Enumerable.Range(1, col-1).Sum();
    return c + steps;
}

long Update(long input)
{
    return (input * 252533) % 33554393;
}