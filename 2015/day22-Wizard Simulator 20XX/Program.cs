await Part1();
await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var bossHp = int.Parse(lines[0].Split(':')[1].Trim());
    var bossD = int.Parse(lines[1].Split(':')[1].Trim());

    var initialState = new State(
        BossHp: bossHp,
        BossDamage: bossD,
        PlayerHp: 50,
        PlayerMana: 500,
        ShieldTimer: 0,
        PoisonTimer: 0,
        RechargeTimer: 0,
        ManaSpent: 0,
        IsPlayerTurn: true
    );

    var pq = new PriorityQueue<State, int>();
    pq.Enqueue(initialState, 0);

    var bestMana = int.MaxValue;
    var seen = new Dictionary<(int, int, int, int, int, int, int, bool), int>();

    while (pq.TryDequeue(out var current, out _))
    {
        if (current.ManaSpent >= bestMana)
            continue;

        var key = (current.BossHp, current.PlayerHp, current.PlayerMana, current.ShieldTimer, current.PoisonTimer, current.RechargeTimer, current.BossDamage, current.IsPlayerTurn);
        if (seen.TryGetValue(key, out var prev) && prev <= current.ManaSpent)
            continue;
        seen[key] = current.ManaSpent;

        // Apply effects first
        var afterEffects = TickEffects(current);

        // Check win condition after effects
        if (afterEffects.BossHp <= 0)
        {
            bestMana = Math.Min(bestMana, afterEffects.ManaSpent);
            continue;
        }

        if (afterEffects.PlayerHp <= 0) continue;

        if (afterEffects.IsPlayerTurn)
        {
            // Player turn: try all spells
            foreach (var spell in Enum.GetValues<Spell>())
            {
                if (!CanCast(afterEffects, spell)) continue;
                var nextState = CastSpell(afterEffects, spell);
                pq.Enqueue(nextState with { IsPlayerTurn = false }, nextState.ManaSpent);
            }
        }
        else
        {
            // Boss turn: attack and switch to player
            var nextState = BossAttack(afterEffects);
            pq.Enqueue(nextState with { IsPlayerTurn = true }, nextState.ManaSpent);
        }
    }

    Console.WriteLine($"{bestMana}");
}

bool CanCast(State s, Spell spell) =>
    spell switch
    {
        Spell.MagicMissile => s.PlayerMana >= 53,
        Spell.Drain => s.PlayerMana >= 73,
        Spell.Shield => s.PlayerMana >= 113 && s.ShieldTimer <= 0,
        Spell.Poison => s.PlayerMana >= 173 && s.PoisonTimer <= 0,
        Spell.Recharge => s.PlayerMana >= 229 && s.RechargeTimer <= 0,
        _ => false
    };

State TickEffects(State s)
{
    var bossHp = s.BossHp;
    var playerMana = s.PlayerMana;
    var shieldTimer = s.ShieldTimer;
    var poisonTimer = s.PoisonTimer;
    var rechargeTimer = s.RechargeTimer;

    if (poisonTimer > 0) bossHp -= 3;
    if (rechargeTimer > 0) playerMana += 101;

    shieldTimer = Math.Max(0, shieldTimer - 1);
    poisonTimer = Math.Max(0, poisonTimer - 1);
    rechargeTimer = Math.Max(0, rechargeTimer - 1);

    return s with
    {
        BossHp = bossHp,
        PlayerMana = playerMana,
        ShieldTimer = shieldTimer,
        PoisonTimer = poisonTimer,
        RechargeTimer = rechargeTimer
    };
}

State CastSpell(State s, Spell spell)
{
    return spell switch
    {
        Spell.MagicMissile => s with { BossHp = s.BossHp - 4, PlayerMana = s.PlayerMana - 53, ManaSpent = s.ManaSpent + 53 },
        Spell.Drain => s with { BossHp = s.BossHp - 2, PlayerHp = s.PlayerHp + 2, PlayerMana = s.PlayerMana - 73, ManaSpent = s.ManaSpent + 73 },
        Spell.Shield => s with { ShieldTimer = 6, PlayerMana = s.PlayerMana - 113, ManaSpent = s.ManaSpent + 113 },
        Spell.Poison => s with { PoisonTimer = 6, PlayerMana = s.PlayerMana - 173, ManaSpent = s.ManaSpent + 173 },
        Spell.Recharge => s with { RechargeTimer = 5, PlayerMana = s.PlayerMana - 229, ManaSpent = s.ManaSpent + 229 },
        _ => throw new ArgumentOutOfRangeException()
    };
}

State BossAttack(State s)
{
    var damage = Math.Max(1, s.BossDamage - (s.ShieldTimer > 0 ? 7 : 0));
    return s with { PlayerHp = s.PlayerHp - damage };
}

async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var bossHp = int.Parse(lines[0].Split(':')[1].Trim());
    var bossD = int.Parse(lines[1].Split(':')[1].Trim());

    var initialState = new State(
        BossHp: bossHp,
        BossDamage: bossD,
        PlayerHp: 50,
        PlayerMana: 500,
        ShieldTimer: 0,
        PoisonTimer: 0,
        RechargeTimer: 0,
        ManaSpent: 0,
        IsPlayerTurn: true
    );

    var pq = new PriorityQueue<State, int>();
    pq.Enqueue(initialState, 0);

    var bestMana = int.MaxValue;
    var seen = new Dictionary<(int, int, int, int, int, int, int, bool), int>();

    while (pq.TryDequeue(out var current, out _))
    {
        if (current.ManaSpent >= bestMana)
            continue;

        var key = (current.BossHp, current.PlayerHp, current.PlayerMana, current.ShieldTimer, current.PoisonTimer, current.RechargeTimer, current.BossDamage, current.IsPlayerTurn);
        if (seen.TryGetValue(key, out var prev) && prev <= current.ManaSpent)
            continue;
        seen[key] = current.ManaSpent;

        // Apply effects first
        
        var afterEffects = TickEffects(current);
        
        // Check win condition after effects
        if (afterEffects.BossHp <= 0)
        {
            bestMana = Math.Min(bestMana, afterEffects.ManaSpent);
            continue;
        }

        if (afterEffects.PlayerHp <= 0) continue;

        if (afterEffects.IsPlayerTurn)
        {
            afterEffects = afterEffects with { PlayerHp = afterEffects.PlayerHp - 1 };
            if (afterEffects.PlayerHp <= 0) continue;
            
            // Player turn: try all spells
            foreach (var spell in Enum.GetValues<Spell>())
            {
                if (!CanCast(afterEffects, spell)) continue;
                var nextState = CastSpell(afterEffects, spell);
                pq.Enqueue(nextState with { IsPlayerTurn = false }, nextState.ManaSpent);
            }
        }
        else
        {
            // Boss turn: attack and switch to player
            var nextState = BossAttack(afterEffects);
            pq.Enqueue(nextState with { IsPlayerTurn = true }, nextState.ManaSpent);
        }
    }

    Console.WriteLine($"{bestMana}");
}

record State(
    int BossHp,
    int BossDamage,
    int PlayerHp,
    int PlayerMana,
    int ShieldTimer,
    int PoisonTimer,
    int RechargeTimer,
    int ManaSpent,
    bool IsPlayerTurn
);

enum Spell
{
    MagicMissile,
    Drain,
    Shield,
    Poison,
    Recharge
}
