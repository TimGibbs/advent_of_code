// @algorithm StateSpaceSearch
// @category Graph Search / Puzzle Solving
// @input
//   Textual description of items on floors
// @model
//   Elevator + Floors + Items (Generators / Microchips)
// @state-representation
//   Elevator position
//   Item floor assignments
//   Canonicalized via paired (generator, microchip) positions
// @technique
//   Breadth-First Search (BFS)
//   State Canonicalization
//   Pruning via visited-state hashing
// @constraints
//   Floor safety rules (microchip shielding)
//   Elevator carries 1 or 2 items
// @operations
//   State expansion
//   Validity checking
//   Goal detection
// @data-structures
//   Queue (BFS frontier)
//   HashSet (visited states)
//   Dictionary (floors, item pairing)
// @variant
//   Part1: Base item set
//   Part2: Extended item set (extra elements)
// @complexity
//   Time: Exponential (pruned by canonical state keys)
//   Space: Exponential (visited states)

await Part1();
await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var floors = ParseInput(lines);
    var start = new State(1, floors);
    Console.WriteLine(Solve(start));   
    
}


async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var floors = ParseInput(lines);
    floors[1].Items.Add(new Item("elerium", ItemType.Microchip));
    floors[1].Items.Add(new Item("elerium", ItemType.Generator));
    floors[1].Items.Add(new Item("dilithium", ItemType.Microchip));
    floors[1].Items.Add(new Item("dilithium", ItemType.Generator));
    var start = new State(1, floors);
    Console.WriteLine(Solve(start)); 

}

Dictionary<int, Floor> ParseInput(string[] strings)
{
    var dictionary = new Dictionary<int,Floor>();
    foreach (var line in strings)
    {
        var split = line.Split(" ");
        var id  = split[1] switch
        {
            "first" => 1,
            "second" => 2,
            "third" => 3,
            "fourth" => 4,
            _ => throw new Exception()
        };
        if (split[4] == "nothing")
        {
            dictionary.Add(id ,new Floor(id,[]));
            continue;
        }

        var l = new List<Item>();
        for (int i = 5; i < split.Length;)
        {
            if (split[i] == "a") i++;
            var element = split[i].Split('-')[0];
            var itemType     = split[i + 1].TrimEnd(',');
            l.Add(new Item(element, itemType == "generator" ? ItemType.Generator : ItemType.Microchip));
            i = i + 3;
        }
        dictionary.Add(id,  new Floor(id, l));
    }

    return dictionary;
}
bool ValidFloorComp(List<Item> items)
{
    var generators = items.Where(x => x.Type == ItemType.Generator).ToList();
    if (!generators.Any()) return true;
    var unshieldedChips = items.Where(x => x.Type == ItemType.Microchip && generators.All(g => g.Element != x.Element)).ToList();
    if (unshieldedChips.Any()) return false;
    return true;
}

string StateKey(State s)
{
    var pairs = new Dictionary<string, (int g, int m)>();

    foreach (var floor in s.Floors)
    {
        foreach (var item in floor.Value.Items)
        {
            if (!pairs.ContainsKey(item.Element))
                pairs[item.Element] = (-1, -1);

            var p = pairs[item.Element];
            pairs[item.Element] = item.Type == ItemType.Generator
                ? (floor.Key, p.m)
                : (p.g, floor.Key);
        }
    }

    return s.Elevator + "|" +
           string.Join(";",
               pairs.Values
                   .OrderBy(p => p.g)
                   .ThenBy(p => p.m)
                   .Select(p => $"{p.g},{p.m}")
           );
}

bool IsValidState(State s)
{
    return s.Floors.Values.All(f => ValidFloorComp(f.Items));
}

IEnumerable<State> NextStates(State s)
{
    var currentFloor = s.Elevator;
    var items = s.Floors[currentFloor].Items;

    var combinations =
        items.Select(i => new[] { i })
            .Concat(
                items.SelectMany((a, i) =>
                    items.Skip(i + 1).Select(b => new[] { a, b }))
            );

    foreach (var dir in new[] { -1, 1 })
    {
        int nextFloor = currentFloor + dir;
        if (!s.Floors.ContainsKey(nextFloor)) continue;

        foreach (var combo in combinations)
        {
            var newFloors = s.Floors.ToDictionary(
                f => f.Key,
                f => new Floor(f.Key, new List<Item>(f.Value.Items))
            );

            foreach (var item in combo)
            {
                newFloors[currentFloor].Items.Remove(item);
                newFloors[nextFloor].Items.Add(item);
            }

            var next = new State(nextFloor, newFloors);
            if (IsValidState(next))
                yield return next;
        }
    }
}

bool IsGoal(State s)
{
    return s.Floors
        .Where(f => f.Key != 4)
        .All(f => f.Value.Items.Count == 0);
}
int Solve(State start)
{
    var q = new Queue<(State state, int steps)>();
    var seen = new HashSet<string>();

    q.Enqueue((start, 0));
    seen.Add(StateKey(start));

    while (q.Count > 0)
    {
        var (cur, steps) = q.Dequeue();
        if (IsGoal(cur)) return steps;

        foreach (var n in NextStates(cur))
        {
            var key = StateKey(n);
            if (seen.Add(key))
                q.Enqueue((n, steps + 1));
        }
    }

    throw new Exception("No solution");
}


public enum ItemType
{
    Generator,
    Microchip
}

public record Item(string Element, ItemType Type);

public record Floor(int Number, List<Item> Items);

public record State(int Elevator, IReadOnlyDictionary<int, Floor> Floors);

