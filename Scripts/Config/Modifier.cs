using System;
using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.UI.Image;
using Leopotam.EcsLite;
using AbilitySystem;
using Client;

[CreateAssetMenu(fileName = "Modifier", menuName = "Modifier/New")]
public class Modifier : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeReference] public ModifierTags ModifierTags;
    [SerializeReference] public IModifierType ModifierType;
    [SerializeReference] public ITargetModifier Target;

    public int Value;


    public void OnAfterDeserialize()
    {

    }

    public void OnBeforeSerialize()
    {
        // if (string.IsNullOrEmpty(name)) return;

        // KEY_ID = name;

    }
}

[Flags] public enum ModifierTags{
    Fire = 1, Physical = 2, Projectile = 4, Attack = 8, Spell = 16
}
public interface ITargetModifier{
    void Init(int abilityEntity, int requestEntity, EcsWorld world, Modifier modifier);
}
public interface IModifierType{
    void Init(int entity, EcsWorld world);
}
public struct AdditiveModifier : IModifierType
{
    public void Init(int entity, EcsWorld world)
    {
        world.GetPool<AdditiveModifier>().Add(entity);
    }
}
public struct MultiplicativeModifier : IModifierType
{
    public void Init(int entity, EcsWorld world)
    {
        world.GetPool<MultiplicativeModifier>().Add(entity);
    }
}
public struct DamageModifier : ITargetModifier
{
    [HideInInspector] public Modifier Modifier;
    [HideInInspector] public int AbilityEntity;
    public void Init(int abilityEntity, int requestEntity, EcsWorld world, Modifier modifier)
    {
        if(!world.GetPool<DamageModifier>().Has(requestEntity)) world.GetPool<DamageModifier>().Add(requestEntity);
        ref var damageModifierComp = ref world.GetPool<DamageModifier>().Get(requestEntity);
        damageModifierComp.AbilityEntity = abilityEntity;
        damageModifierComp.Modifier = modifier;
    }
}
public struct AddAbilityEffect : ITargetModifier
{
    [SerializeReference] public IAbilityEffect Effect;
    [HideInInspector] public int AbilityEntity;
    [HideInInspector] public Modifier Modifier;
    public void Init(int abilityEntity, int requestEntity, EcsWorld world, Modifier modifier)
    {
        ref var requestAddAbilityEffectComp = ref world.GetPool<AddAbilityEffect>().Add(requestEntity);
        requestAddAbilityEffectComp.Effect = Effect;
        requestAddAbilityEffectComp.AbilityEntity = abilityEntity;
        requestAddAbilityEffectComp.Modifier = modifier;
    }
}

