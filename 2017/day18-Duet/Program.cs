// @algorithm Duet Assembly Interpreter
// @category Virtual Machine / Concurrency Simulation
// @input Assembly-like instruction list (snd, set, add, mul, mod, rcv, jgz)
// @problem
//   Simulate custom instruction set
// @parts
//   Part1: Recover last played sound when rcv executes with non-zero value
//   Part2: Run two concurrent programs and count how many times program 1 sends a value
// @model
//   - 26 registers (a–z) mapped to long[]
//   - Instruction pointer (IP)
//   - Parsed instruction representation with register/immediate resolution
// @instructions
//   snd X → play/send value
//   set X Y → X = Y
//   add X Y → X += Y
//   mul X Y → X *= Y
//   mod X Y → X %= Y
//   rcv X → recover (Part1) / receive from queue (Part2)
//   jgz X Y → if X > 0 jump by offset Y
// @technique
//   Part1:
//     - Single program execution loop
//     - Track last sound played
//     - Stop when rcv condition satisfied
//   Part2:
//     - Two ProgramState instances
//     - Message passing via Queue<long>
//     - Step-based cooperative execution
//     - Deadlock detection when both programs cannot progress
// @concurrency-model
//   - Cooperative multitasking
//   - Blocking on empty receive
//   - No threads; deterministic stepping
// @data-structures
//   - long[26] registers
//   - Queue<long> for message passing
//   - Parsed Instruction record struct
// @complexity
//   Time: O(instruction executions)
//   Space: O(queue size + registers)
// @notes
//   - Program 1 initialized with register p = 1
//   - Deadlock occurs when both programs attempt rcv with empty queue
//   - jgz performs relative jump without automatic IP increment

await Part1();
await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var instructions = lines.Select(ParseLine).ToArray();
    var registers = new long[26];
    long output = 0;
    for (int i = 0; i >= 0 && i < instructions.Length;)
    {
        var instruction = instructions[i];
        var v1 = instruction.IsRegister1 ? registers[instruction.Value1] : instruction.Value1;
        long? v2 = null;
        if (instruction.Value2.HasValue)
        {
            v2 = instruction.IsRegister2
                ? registers[instruction.Value2.Value]
                : instruction.Value2.Value;
        }
        switch (instruction.InstructionCode)
        {
            case InstructionCode.snd:
                output = v1;
                break;
            case InstructionCode.set:
                registers[instruction.Value1] = v2.Value;
                break;
            case InstructionCode.add:
                registers[instruction.Value1] += v2.Value;
                break;
            case InstructionCode.mul:
                registers[instruction.Value1] *= v2.Value;
                break;
            case InstructionCode.mod:
                registers[instruction.Value1] %= v2.Value;
                break;
            case InstructionCode.rcv:
                if (v1 != 0)
                {
                    Console.WriteLine(output);
                    return;
                }
                break;
            case InstructionCode.jgz:
                if (v1 > 0)
                {
                    i += (int)v2.Value;
                    continue;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        i++;
    }
}
async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var instructions = lines.Select(ParseLine).ToArray();

    var prog0 = new ProgramState(0);
    var prog1 = new ProgramState(1);

    while (true)
    {
        var progressed0 = Step(prog0, prog1, instructions);
        var progressed1 = Step(prog1, prog0, instructions);

        // deadlock detection
        if (!progressed0 && !progressed1)
            break;
    }

    Console.WriteLine(prog1.SendCount);
}

bool Step(ProgramState self, ProgramState other, Instruction[] instructions)
{
    if (self.IP < 0 || self.IP >= instructions.Length)
        return false;

    var inst = instructions[self.IP];

    long v1 = inst.IsRegister1
        ? self.Registers[inst.Value1]
        : inst.Value1;

    long? v2 = null;
    if (inst.Value2.HasValue)
    {
        v2 = inst.IsRegister2
            ? self.Registers[inst.Value2.Value]
            : inst.Value2.Value;
    }

    switch (inst.InstructionCode)
    {
        case InstructionCode.snd:
            other.Queue.Enqueue(v1);
            self.SendCount++;
            break;

        case InstructionCode.set:
            self.Registers[inst.Value1] = v2.Value;
            break;

        case InstructionCode.add:
            self.Registers[inst.Value1] += v2.Value;
            break;

        case InstructionCode.mul:
            self.Registers[inst.Value1] *= v2.Value;
            break;

        case InstructionCode.mod:
            self.Registers[inst.Value1] %= v2.Value;
            break;

        case InstructionCode.rcv:
            if (self.Queue.Count == 0)
            {
                self.Waiting = true;
                return false; // cannot progress
            }

            self.Waiting = false;
            self.Registers[inst.Value1] = self.Queue.Dequeue();
            break;

        case InstructionCode.jgz:
            if (v1 > 0)
            {
                self.IP += (int)v2.Value;
                return true;
            }
            break;
    }

    self.IP++;
    return true;
}


Instruction ParseLine(string line)
{
    var split = line.Split(' ');
    var isnt = split[0] switch
    {
        "snd" => InstructionCode.snd,
        "set" => InstructionCode.set,
        "add" => InstructionCode.add,
        "mul" => InstructionCode.mul,
        "mod" => InstructionCode.mod,
        "rcv" => InstructionCode.rcv,
        "jgz" => InstructionCode.jgz,
        _ => throw new ArgumentOutOfRangeException()
    };
    long value1 = 0;
    var isRegister1 = true;
    if (long.TryParse(split[1], out value1))
    {
        isRegister1 = false;
    }
    else
    {
        value1 = split[1][0] - 'a';
    }
    long? value2 = null;
    var isRegister2 = true;
    if (split.Length > 2)
    {
        if (long.TryParse(split[2], out var val))
        {
            value2 = val;
            isRegister2 = false;
        }
        else
        {
            value2 = split[2][0] - 'a';
        }
    }

    return new Instruction(isnt, value1, isRegister1, value2, isRegister2);
}

enum InstructionCode
{
    snd,
    set,
    add,
    mul,
    mod,
    rcv,
    jgz,
}

record struct Instruction(InstructionCode InstructionCode, long Value1, bool IsRegister1, long? Value2, bool IsRegister2);

class ProgramState
{
    public long[] Registers = new long[26];
    public Queue<long> Queue = new();
    public int IP;
    public bool Waiting = false;
    public long SendCount;

    public ProgramState(long id)
    {
        Registers['p' - 'a'] = id;
    }
}
