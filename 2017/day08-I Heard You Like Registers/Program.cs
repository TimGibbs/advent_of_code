// @algorithm Register Machine Simulation
// @category Interpreter / State Tracking
// @input List of conditional register instructions
// @problem
//   Execute instructions of the form:
//     <reg> inc|dec <n> if <other> <cond> <value>
//   Registers default to 0 if unseen.
// @parts
//   Part1: Find the largest value in any register after execution completes
//   Part2: Track the highest value ever held by any register during execution
// @model
//   - Dictionary<string, int> for register storage
//   - Sequential execution, no jumps
// @technique
//   - Parse instructions into structured records
//   - Evaluate conditions against queried registers
//   - Apply signed deltas for inc/dec
//   - Track running maximum (Part2)
// @data-structures
//   Instruction record (Register, Op, Quantity, Query, Condition, Value)
//   Enums for Operation and Condition
// @complexity
//   Time: O(n)
//   Space: O(r) where r = number of registers
// @notes
//   - Missing registers are treated as value 0
//   - Part2 updates the max *before* storing the new value

await Part1();
await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var instuctions = lines.Select(ParseInstruction).ToArray();
    var registers = new Dictionary<string, int>();
    foreach (var instruction in instuctions)
    {
        registers.TryGetValue(instruction.Query, out var q);
        if (!CheckCondition(q, instruction.Condition, instruction.Value)) continue;
        
        registers.TryGetValue(instruction.Register, out var reg);
        var val = instruction.Quantity * (instruction.Op == Operation.Dec ? -1 : 1);
        registers[instruction.Register] = reg + val;
    }
    Console.WriteLine(registers.Values.Max());
}

async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var instuctions = lines.Select(ParseInstruction).ToArray();
    var registers = new Dictionary<string, int>();
    var max = 0;
    foreach (var instruction in instuctions)
    {
        registers.TryGetValue(instruction.Query, out var q);
        if (!CheckCondition(q, instruction.Condition, instruction.Value)) continue;
        
        registers.TryGetValue(instruction.Register, out var reg);
        var val = instruction.Quantity * (instruction.Op == Operation.Dec ? -1 : 1);
        max = Math.Max(max, reg + val);
        registers[instruction.Register] = reg + val;
    }
    Console.WriteLine(max);
}

Instruction ParseInstruction(string str)
{
    var split = str.Split(' ');
    return new Instruction()
    {
        Register = split[0],
        Op = ParseOperation(split[1]),
        Quantity = int.Parse(split[2]),
        Query = split[4],
        Condition = ParseCondition(split[5]),
        Value = int.Parse(split[6]),
    };
}


bool CheckCondition(int q, Condition con, int v)
{
    return con switch
    {
        Condition.Equal => q == v,
        Condition.NotEqual => q != v,
        Condition.LessThan => q < v,
        Condition.GreaterThan => q > v,
        Condition.LessThanOrEqual => q <= v,
        Condition.GreaterThanOrEqual => q >= v,
        _ => throw new ArgumentOutOfRangeException(nameof(con), con, null)
    };
}

Condition ParseCondition(string str)
{
    return str switch
    {
        "==" => Condition.Equal,
        "!=" => Condition.NotEqual,
        "<" => Condition.LessThan,
        ">" => Condition.GreaterThan,
        "<=" => Condition.LessThanOrEqual,
        ">=" => Condition.GreaterThanOrEqual,
        _ => throw new ArgumentOutOfRangeException(nameof(str), str, null)
    };
}

Operation ParseOperation(string str)
{
    return str switch
    {
        "inc" => Operation.Inc,
        "dec" => Operation.Dec,
        _ => throw new ArgumentOutOfRangeException(nameof(str), str, null)
    };
}

record struct Instruction
{
    public string Register;
    public Operation Op;
    public int Quantity;
    public string Query;
    public Condition Condition;
    public int Value;

}

enum Operation
{
    Inc,
    Dec
}

enum Condition
{
    Equal,
    NotEqual,
    LessThan,
    GreaterThan,
    LessThanOrEqual,
    GreaterThanOrEqual,
}