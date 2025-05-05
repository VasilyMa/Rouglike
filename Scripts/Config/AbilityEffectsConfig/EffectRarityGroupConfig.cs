using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EffectRarityGroupConfig", menuName = "Configs/Ability Effects/Effect Rarity Group Config")]
public class EffectRarityGroupConfig : ScriptableObject
{
    [SerializeField]private EffectGroup[] _effectGroups;
    [SerializeField]private bool _normalizeChance;

    [System.Serializable]
    public class EffectGroup
    {
        public Rarity rarity;
        public AbilityEffectConfig[] effects; 
        [Range(0,1)]public float dropChance;
    }
    private void OnValidate()
    {
       if(_normalizeChance) NormalizeDropChances();
    }
    private void NormalizeDropChances()
    {
        float totalDropChance = 0f;

        foreach (var group in _effectGroups)
        {
            totalDropChance += group.dropChance;
        }

        if (totalDropChance > 0)
        {
            foreach (var group in _effectGroups)
            {
                group.dropChance /= totalDropChance;
            }
        }
    }

    public AbilityEffectConfig GetRandomEffect()
    {
        float totalDropChance = 0f;
        foreach (var group in _effectGroups)
        {
            totalDropChance += group.dropChance;
        }

        float randomValue = Random.Range(0f, totalDropChance);

        foreach (var group in _effectGroups)
        {
            if (randomValue <= group.dropChance)
            {
                if (group.effects.Length > 0)
                {
                    int randomEffectIndex = Random.Range(0, group.effects.Length);
                    return group.effects[randomEffectIndex]; 
                }
                break; 
            }
            randomValue -= group.dropChance; 
        }

        return null; 
    }
}
