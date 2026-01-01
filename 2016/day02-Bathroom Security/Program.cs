// @algorithm KeypadNavigation / DirectionalSimulation
// @category GridTraversal / StateSimulation
// @data-structure
//   Integer (key position as state)
//   Array<int> (result code)
// @technique
//   Finite State Machine
//   Deterministic Transitions
//   Instruction Replay
// @variant
//   Part1: RectangularKeypad
//   Part2: IrregularKeypad
// @complexity
//   Time: O(n · m)  (n lines, m moves per line)
//   Space: O(n)


await Part1();
await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var position = 5;
    var code = new int[lines.Length];
    for (int i = 0; i < lines.Length; i++)
    {
        var line = lines[i];
        foreach (var c in line)
        {
            position = Move(position, c);
        }

        code[i] = position;
    }
    Console.WriteLine(string.Join("",code));
}


int Move(int cur, char dir)
{
    return cur switch
    {
        1 => dir switch
        {
            'L' => 1,
            'R' => 2,
            'U' => 1,
            'D' => 4,
            _ => cur
        },
        2 => dir switch
        {
            'L' => 1,
            'R' => 3,
            'U' => 2,
            'D' => 5,
            _ => cur
        },
        3 => dir switch
        {
            'L' => 2,
            'R' => 3,
            'U' => 3,
            'D' => 6,
            _ => cur
        },
        4 => dir switch
        {
            'L' => 4,
            'R' => 5,
            'U' => 1,
            'D' => 7,
            _ => cur
        },
        5 => dir switch
        {
            'L' => 4,
            'R' => 6,
            'U' => 2,
            'D' => 8,
            _ => cur
        },
        6 => dir switch
        {
            'L' => 5,
            'R' => 6,
            'U' => 3,
            'D' => 9,
            _ => cur
        },
        7 => dir switch
        {
            'L' => 7,
            'R' => 8,
            'U' => 4,
            'D' => 7,
            _ => cur
        },
        8 => dir switch
        {
            'L' => 7,
            'R' => 9,
            'U' => 5,
            'D' => 8,
            _ => cur
        },
        9 => dir switch
        {
            'L' => 8,
            'R' => 9,
            'U' => 6,
            'D' => 9,
            _ => cur
        },
        _ => cur
    };
}


async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var position = 5;
    var code = new int[lines.Length];
    for (int i = 0; i < lines.Length; i++)
    {
        var line = lines[i];
        foreach (var c in line)
        {
            position = Move2(position, c);
        }

        code[i] = position;
    }
    Console.WriteLine(string.Join("",code.Select(x=>x switch
    {
        10 => "A",
        11 => "B",
        12 => "C",
        13 => "D",
        _ => x.ToString() 
    })));
}

int Move2(int cur, char dir)
{
    return cur switch
    {
        1 => dir switch
        {
            'U' => 1,
            'D' => 3,
            'L' => 1,
            'R' => 1,
            _ => cur
        },

        2 => dir switch
        {
            'U' => 2,
            'D' => 6,
            'L' => 2,
            'R' => 3,
            _ => cur
        },

        3 => dir switch
        {
            'U' => 1,
            'D' => 7,
            'L' => 2,
            'R' => 4,
            _ => cur
        },

        4 => dir switch
        {
            'U' => 4,
            'D' => 8,
            'L' => 3,
            'R' => 4,
            _ => cur
        },

        5 => dir switch
        {
            'U' => 5,
            'D' => 5,
            'L' => 5,
            'R' => 6,
            _ => cur
        },

        6 => dir switch
        {
            'U' => 2,
            'D' => 10,
            'L' => 5,
            'R' => 7,
            _ => cur
        },

        7 => dir switch
        {
            'U' => 3,
            'D' => 11,
            'L' => 6,
            'R' => 8,
            _ => cur
        },

        8 => dir switch
        {
            'U' => 4,
            'D' => 12,
            'L' => 7,
            'R' => 9,
            _ => cur
        },

        9 => dir switch
        {
            'U' => 9,
            'D' => 9,
            'L' => 8,
            'R' => 9,
            _ => cur
        },

        10 => dir switch // A
        {
            'U' => 6,
            'D' => 10,
            'L' => 10,
            'R' => 11,
            _ => cur
        },

        11 => dir switch // B
        {
            'U' => 7,
            'D' => 13,
            'L' => 10,
            'R' => 12,
            _ => cur
        },

        12 => dir switch // C
        {
            'U' => 8,
            'D' => 12,
            'L' => 11,
            'R' => 12,
            _ => cur
        },

        13 => dir switch // D
        {
            'U' => 11,
            'D' => 13,
            'L' => 13,
            'R' => 13,
            _ => cur
        },

        _ => cur
    };
}
