using AbilitySystem;
using Sirenix.OdinInspector;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Windows;

[CreateAssetMenu(fileName = "NewAbilityEffect", menuName = "Configs/Ability Effects/NewAbilityEffect")]
public class AbilityEffectConfig : ScriptableObject
{
    public string effectId;
    public string effectName;
    public string description;
    public EffectType effectType;
    public AbilityEffectInfluence AbilityEffectInfluence;
    //public Rarity rarity;
    [HideIf("effectType", EffectType.DamageOverTime)] public ParticleSystem Particle;
    [ShowIf("effectType", EffectType.DamageOverTime)] public GameObject BleendingGO;
    public float value;
    public float duration; 
    public float cooldown; 
    public float cost;
    [ShowIf("effectType", EffectType.ExplosionOnHit)]public DamageZone damageZone;
    public bool HasInputFlag(string byName,AbilityEffectConfig abilityEffect)
    {
        AbilityEffectInfluence flag;

        if (Enum.TryParse(byName, true, out flag))
        {
            if (flag == abilityEffect.AbilityEffectInfluence) return true;
        }

        return false;
    }
}

public enum EffectType
{
    Vampirism,
    DamageOverTime,
    ExplosionOnHit,
}

public enum Rarity
{
    Common,
    Rare,
    Epic,
    Legendary
}
[Flags]
public enum AbilityEffectInfluence
{
    None = 0,                     // 0b0000
    Attack = 1 << 0,              // 0b0001
    SuperAttack = 1 << 1,          // 0b0010
    Ability = 1 << 2,            // 0b0100
    UtilityAbility = 1 << 3,            // 0b1000
    Dash = 1 << 4,        // 0b10000
    All = Attack | SuperAttack | Ability | UtilityAbility | Dash // 0b0101
}