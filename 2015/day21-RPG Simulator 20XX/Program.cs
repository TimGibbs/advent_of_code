// @algorithm BruteForce / Simulation
// @category GameTheory
// @data-structure Array
// @complexity 
//   Time: O(W * A * R²)
//   Space: O(1)
// @technique ExhaustiveSearch, TurnBasedCombatMath
// @variant 
//   Part1: MinCostWinningLoadout
//   Part2: MaxCostLosingLoadout

Equipment[] weapons =
[
    new(8 , 4, 0),
    new(10, 5, 0),
    new(25, 6, 0),
    new(40, 7, 0),
    new(74, 8, 0),
];

Equipment[] armors =
[
    new(0, 0, 0),
    new(13, 0, 1),
    new(31, 0, 2),
    new(53, 0, 3),
    new(75, 0, 4),
    new(102, 0, 5),
];

Equipment[] rings =
[
    new(0, 0, 0),
    new(25, 1, 0),
    new(50, 2, 0),
    new(100, 3, 0),
    new(20, 0, 1),
    new(40, 0, 2),
    new(80, 0, 3),
];


await Part1();
await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var bossHp = int.Parse(lines[0].Split(':')[1].TrimStart());
    var bossD = int.Parse(lines[1].Split(':')[1].TrimStart());
    var bossA = int.Parse(lines[2].Split(':')[1].TrimStart());
    
    var hp = 100;
    var stop = false;
    var min = int.MaxValue;
    foreach (var weapon in weapons)
    {
        foreach (var armor in armors)
        {
            foreach (var ring1 in rings)
            {
                foreach (var ring2 in rings)
                {
                    if(ring1==ring2) continue;
                    var d = weapon.damage + ring1.damage + ring2.damage;
                    var a = armor.armor + ring1.armor + ring2.armor;
                    if (IsSuccess(bossHp, bossD, bossA, hp, d, a))
                    {
                        min = Math.Min(min, weapon.cost + armor.cost + ring1.cost + ring2.cost);
                    }
                }
            }
        }   
    }
    
    Console.WriteLine(min);
}

async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var bossHp = int.Parse(lines[0].Split(':')[1].TrimStart());
    var bossD = int.Parse(lines[1].Split(':')[1].TrimStart());
    var bossA = int.Parse(lines[2].Split(':')[1].TrimStart());
    
    var hp = 100;
    var stop = false;
    var max = int.MinValue;
    foreach (var weapon in weapons)
    {
        foreach (var armor in armors)
        {
            foreach (var ring1 in rings)
            {
                foreach (var ring2 in rings)
                {
                    if(ring1==ring2) continue;
                    var d = weapon.damage + ring1.damage + ring2.damage;
                    var a = armor.armor + ring1.armor + ring2.armor;
                    if (!IsSuccess(bossHp, bossD, bossA, hp, d, a))
                    {
                        max = Math.Max(max, weapon.cost + armor.cost + ring1.cost + ring2.cost);
                    }
                }
            }
        }   
    }
    
    Console.WriteLine(max);
}

bool IsSuccess(int bossHp, int bossD, int bossA, int hp, int d, int a)
{
    var damageDealt = Math.Max(d - bossA, 1);
    var damageTaken = Math.Max(bossD - a, 1);

    var playerTurns = (bossHp + damageDealt - 1) / damageDealt;
    var bossTurns   = (hp + damageTaken - 1) / damageTaken;

    return playerTurns <= bossTurns;
}

record Equipment(int cost, int damage, int armor);