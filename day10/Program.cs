// See https://aka.ms/new-console-template for more information

using Google.OrTools.Sat;

await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var machines = lines.Select(x=>x.Split(' ')).Select(s =>
    {
        var goal = GoalMask(s[0][1..^1]);
        var buttons = s[1..^1].Select(t=>t[1..^1].Split(',').Select(int.Parse).ToArray()).ToArray();
        var joltage = s[^1][1..^1].Split(',').Select(int.Parse).ToArray();
        return new Machine(goal, buttons, joltage);
    }).ToArray();
    var sum = machines.Sum(Solve1);
    Console.WriteLine(sum);
}
int Solve1(Machine machine)
{
    var m = machine.Buttons.Length;
    var buttons = machine.Buttons.Select(Mask).ToArray();
    // determine the number of lights (n): largest bit position in goal or any button
    var maxBit = 0;
    var allMasks = machine.Goal;
    foreach (var bt in buttons) allMasks |= bt;
    while ((1 << maxBit) <= allMasks) maxBit++;
    var n = Math.Max(1, maxBit); // at least 1

    // Build n rows (one per light). Each row is an m-bit mask: bit j set if button j toggles this light.
    var rows = new int[n];
    var rhs = new int[n];
    for (var i = 0; i < n; i++)
    {
        var rowMask = 0;
        for (var j = 0; j < m; j++)
        {
            if (((buttons[j] >> i) & 1) != 0)
                rowMask |= (1 << j);
        }
        rows[i] = rowMask;
        rhs[i] = (machine.Goal >> i) & 1;
    }

    // where[col] = row index where this column has pivot, -1 if free
    var where = Enumerable.Repeat(-1, m).ToArray();
    var row = 0;

    // Gaussian elimination to (reduced) row echelon form, eliminating all other rows for convenience
    for (var col = 0; col < m && row < n; col++)
    {
        // find pivot row with bit 'col' set
        var sel = -1;
        for (var r = row; r < n; r++)
            if (((rows[r] >> col) & 1) != 0) { sel = r; break; }

        if (sel == -1) continue;

        // swap selected row into current 'row'
        (rows[row], rows[sel]) = (rows[sel], rows[row]);
        (rhs[row], rhs[sel]) = (rhs[sel], rhs[row]);

        where[col] = row;

        // eliminate this column from all other rows to get RREF
        for (var r = 0; r < n; r++)
        {
            if (r != row && (((rows[r] >> col) & 1) != 0))
            {
                rows[r] ^= rows[row];
                rhs[r] ^= rhs[row];
            }
        }

        row++;
    }

    // check for inconsistency: 0 = 1
    for (var r = row; r < n; r++)
    {
        if (rows[r] == 0 && rhs[r] == 1)
            return int.MaxValue; // no solution; shouldn't happen for valid inputs
    }

    // one particular solution when all free vars = 0:
    for (var col = 0; col < m; col++)
    {
        if (where[col] != -1)
        {
            var r = where[col];
            // pivot is at col, and row r has 1 at col; when free vars = 0, solution bit = rhs[r]
            if (rhs[r] != 0)
            {
            }
        }
    }

    // collect free variable columns
    var freeCols = new List<int>();
    for (var col = 0; col < m; col++)
        if (where[col] == -1) freeCols.Add(col);

    // Try all combinations of free variables (m is typically small)
    var best = int.MaxValue;
    var freeCount = freeCols.Count;
    var combos = 1 << freeCount;
    for (var mask = 0; mask < combos; mask++)
    {
        // build bitmask for free variables (buttons) set to 1 in this combination
        var freeButtonsMask = 0;
        for (var k = 0; k < freeCount; k++)
            if (((mask >> k) & 1) != 0)
                freeButtonsMask |= (1 << freeCols[k]);

        // compute dependent (pivot) variable values taking into account free vars:
        var candidate = 0;
        // first set free columns directly
        candidate |= freeButtonsMask;

        // for each pivot column, compute its value = rhs[row] ^ parity(rowMask & freeButtonsMask)
        for (var col = 0; col < m; col++)
        {
            if (where[col] != -1)
            {
                var r = where[col];
                // parity of overlap between row r and freeButtonsMask
                var overlap = rows[r] & freeButtonsMask;
                var parity = System.Numerics.BitOperations.PopCount((uint)overlap) & 1;
                var val = rhs[r] ^ parity;
                if (val != 0) candidate |= (1 << col);
            }
        }

        // count presses (bit set)
        var presses = System.Numerics.BitOperations.PopCount((uint)candidate);
        if (presses < best) best = presses;
    }

    return best;
}

int GoalMask(string s)
{
    var mask = 0;
    for (var i = 0; i < s.Length; i++)
        if (s[i] == '#') mask |= (1 << i);
    return mask;
}
int Mask(int[] bits)
{
    return bits.Aggregate(0, (current, b) => current | 1 << b);
}
async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var machines = lines.Select(x=>x.Split(' ')).Select(s =>
    {
        var goal = GoalMask(s[0][1..^1]);
        var buttons = s[1..^1].Select(t=>t[1..^1].Split(',').Select(int.Parse).ToArray()).ToArray();
        var joltage = s[^1][1..^1].Split(',').Select(int.Parse).ToArray();
        return new Machine(goal, buttons, joltage);
    }).ToArray();
    var sum = machines.Sum(Solve2);
    Console.WriteLine(sum);
}

int Solve2(Machine machine)
{
    var m = machine.Buttons.Length;
    var n = machine.Joltage.Length;
    var target = machine.Joltage;

    // Build coefficient matrix A[n][m]
    var a = new int[n, m];
    for (var j = 0; j < m; j++)
        foreach (var c in machine.Buttons[j])
            a[c, j] = 1;

    var model = new CpModel();

    // Decision variables: presses[j] ≥ 0
    var presses = new IntVar[m];
    for (var j = 0; j < m; j++)
        presses[j] = model.NewIntVar(0, 1_000_000, $"x{j}");

    // Constraints: A * presses = target
    for (var i = 0; i < n; i++)
    {
        var sum = LinearExpr.Sum(
            Enumerable.Range(0, m).Select(j => presses[j] * a[i, j])
        );

        model.Add(sum == target[i]);
    }

    // Objective: minimize total presses
    model.Minimize(LinearExpr.Sum(presses));

    // Solve
    var solver = new CpSolver();
    solver.StringParameters = "num_search_workers:8";  // parallel solve

    var status = solver.Solve(model);

    if (status != CpSolverStatus.Optimal &&
        status != CpSolverStatus.Feasible)
    {
        throw new InvalidOperationException("No integer solution found.");
    }

    long result = 0;
    for (var j = 0; j < m; j++)
        result += solver.Value(presses[j]);

    return (int)result;
}



record Machine(int Goal, int[][] Buttons, int[] Joltage);