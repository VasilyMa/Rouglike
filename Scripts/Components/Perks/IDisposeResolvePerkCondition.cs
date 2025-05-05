using Leopotam.EcsLite;
using Client;
using Statement;
using UnityEngine;
public interface IDisposeResolvePerkCondition
{
    void InitCondition(int helperEntity, EcsWorld world);
}
public struct AfterAbilityTagDispose : IDisposeResolvePerkCondition
{
    public ModifierTags AbilityTargetTag;
    public void InitCondition(int helperEntity, EcsWorld world)
    {
        ref var afterAbilityTagComp = ref world.GetPool<AfterAbilityTagDispose>().Add(helperEntity);
        afterAbilityTagComp.AbilityTargetTag = AbilityTargetTag;
    }
}
public struct AfterCounterDispose : IDisposeResolvePerkCondition
{
    public int TargetCount;
    public int CurrentCount;
    public void InitCondition(int helperEntity, EcsWorld world)
    {
        ref var afterCounterComp = ref world.GetPool<AfterCounterDispose>().Add(helperEntity);
        afterCounterComp.TargetCount = TargetCount;
        afterCounterComp.CurrentCount = 0;
    }
}
public struct AfterTimerDispose : IDisposeResolvePerkCondition
{
    public float TargetTime;
    [HideInInspector] public float CurrentTime;
    public void InitCondition(int helperEntity, EcsWorld world)
    {
        ref var afterTimePerkComp = ref world.GetPool<AfterTimerDispose>().Add(helperEntity);
        afterTimePerkComp.TargetTime = TargetTime;
        afterTimePerkComp.CurrentTime = 0f;
    }
}
public struct NotDispose : IDisposeResolvePerkCondition
{
    public void InitCondition(int helperEntity, EcsWorld world)
    {
        if (!world.GetPool<NotDispose>().Has(helperEntity))
            world.GetPool<NotDispose>().Add(helperEntity);
    }
}
