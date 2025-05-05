using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Perks/Perk Pool Config", fileName = "PerkPoolConfig")]
public class PerkPoolConfig : ScriptableObject
{
    [Serializable]
    public class PerkEntry
    {
        public Perk perk;
        [Range(0f, 1f)] public float chance;
    }

    [Header("Chance to roll a rarity (sum must be ? 1.0)")]
    [Range(0f, 1f)] public float commonChance = 0.7f;
    [Range(0f, 1f)] public float rareChance = 0.2f;
    [Range(0f, 1f)] public float legendaryChance = 0.1f;

    [Header("Perk Pools by Rarity")]
    public List<PerkEntry> commonPerks;
    public List<PerkEntry> rarePerks;
    public List<PerkEntry> legendaryPerks;

    public List<Perk> GeneratePerks(int count)
    {
        var result = new List<Perk>(count);

        for (int i = 0; i < count; i++)
        {
            result.Add(RollOnePerk());
        }

        return result;
    }

    private Perk RollOnePerk()
    {
        float rarityRoll = UnityEngine.Random.value;
        List<PerkEntry> pool = null;

        if (rarityRoll < legendaryChance)
            pool = legendaryPerks;
        else if (rarityRoll < legendaryChance + rareChance)
            pool = rarePerks;
        else
            pool = commonPerks;

        if (pool == null || pool.Count == 0)
            return null;

        float totalWeight = 0f;
        foreach (var entry in pool)
            totalWeight += entry.chance;

        float roll = UnityEngine.Random.value * totalWeight;
        float current = 0f;

        foreach (var entry in pool)
        {
            current += entry.chance;
            if (roll <= current)
                return entry.perk;
        }

        return pool[UnityEngine.Random.Range(0, pool.Count)].perk;
    }
}
