using UnityEngine;
using Leopotam.EcsLite;
using Client;
using Statement;

public interface IPerkResolveEffect{
    void ResolvePerk(int helperEntity, EcsWorld world);
    void DisposePerk(int helperEntity, EcsWorld world);
}
public struct AddModifierEffect : IPerkResolveEffect
{
    public Modifier Modifier;

    public void DisposePerk(int entity, EcsWorld world)
    {
        ref var delModifierRequestComp = ref world.GetPool<RequestDelModifier>().Add(world.NewEntity());
        delModifierRequestComp.Modifier = Modifier;
        delModifierRequestComp.UnitPackedEntity = world.PackEntity(BattleState.Instance.GetEntity("PlayerEntity"));

        
    }

    public void ResolvePerk(int entity, EcsWorld world)
    {
        ref var AddModifierComp = ref world.GetPool<AddModifierEffect>().Add(entity);
        AddModifierComp.Modifier = Modifier;
        ref var addModifierRequestComp = ref world.GetPool<RequestAddModifier>().Add(world.NewEntity());
        addModifierRequestComp.Modifier = Modifier;
        addModifierRequestComp.UnitPackedEntity = world.PackEntity(BattleState.Instance.GetEntity("PlayerEntity"));

        
    }
}