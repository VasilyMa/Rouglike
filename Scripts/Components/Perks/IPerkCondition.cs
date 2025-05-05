using Leopotam.EcsLite;
using Client;
using Statement;
using UnityEngine;
public interface IPerkCondition{
    void InitPerkComponent(int perkEntity, EcsWorld world);
}
public struct AfterTimePerk : IPerkCondition
{
    public float TargetTime;
    [HideInInspector] public float CurrentTime;
    public void InitPerkComponent(int perkEntity, EcsWorld world)
    {
        ref var afterTimePerkComp = ref world.GetPool<AfterTimePerk>().Add(perkEntity);
        afterTimePerkComp.TargetTime = TargetTime;
        afterTimePerkComp.CurrentTime = 0f;
    }
}
public struct AfterDamagePerk : IPerkCondition
{
    //todo damageType?
    public float DamageTargetValue;
    public float CurrentDamage;
    public void InitPerkComponent(int perkEntity, EcsWorld world)
    {
        ref var afterDamagePerkComp = ref world.GetPool<AfterDamagePerk>().Add(perkEntity);
        afterDamagePerkComp.DamageTargetValue = DamageTargetValue;
        afterDamagePerkComp.CurrentDamage = 0f;
    }
}
public struct AfterAbilityTag : IPerkCondition
{
    public ModifierTags AbilityTargetTag;
    public void InitPerkComponent(int perkEntity, EcsWorld world)
    {   
        ref var afterAbilityTagComp = ref world.GetPool<AfterAbilityTag>().Add(perkEntity);
        afterAbilityTagComp.AbilityTargetTag = AbilityTargetTag;
    }
}
public struct AfterCounter : IPerkCondition
{
    public int TargetCount;
    public int CurrentCount;
    public void InitPerkComponent(int perkEntity, EcsWorld world)
    {
        ref var afterCountComp = ref world.GetPool<AfterCounter>().Add(perkEntity);
        afterCountComp.TargetCount = TargetCount;
        afterCountComp.CurrentCount = 0;
    }
}
public struct AfterReceivingPerk : IPerkCondition
{
    public void InitPerkComponent(int perkEntity, EcsWorld world)
    {
        if(!world.GetPool<AfterCounter>().Has(perkEntity))
            world.GetPool<AfterCounter>().Add(perkEntity);
    }
}
public struct ReceivingRelic { }